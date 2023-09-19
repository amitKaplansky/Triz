using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Hitcode_RoomEscape
{
    public class PanelInventory : MonoBehaviour
    {

        // Use this for initialization
        public GameObject iconFrame;
        int perpage = 10;
        int cpage = 0;
        int pages;

        List<GameObject> slots = new List<GameObject>();
        List<Item> itemsCpage = new List<Item>();

        GameObject panel;
        Button btnCombine;
        Button btnUse;
        Button btnRight;
        Button btnLeft;
        Text itemText;
        private void Start()
        {
            panel = transform.Find("panel").gameObject;
            btnCombine = panel.transform.Find("btnCombine").GetComponent<Button>();
            btnUse = panel.transform.Find("btnUse").GetComponent<Button>();
            btnLeft = panel.transform.Find("btnLeft").GetComponent<Button>();
            btnRight = panel.transform.Find("btnRight").GetComponent<Button>();

            Button btnClose = panel.transform.Find("btnClose").GetComponent<Button>();

            btnCombine.GetComponentInChildren<Text>().text = Localization.Instance.GetString("btnCombine");
            btnUse.GetComponentInChildren<Text>().text = Localization.Instance.GetString("btnUse");
            btnClose.GetComponentInChildren<Text>().text = Localization.Instance.GetString("btnClose");


            btnCombine.interactable = false;
            btnUse.interactable = false;
            pages = Mathf.FloorToInt((GameData.Instance.items.Count - 1) / 8f) + 1;

            btnRight.gameObject.SetActive(cpage < pages-1);
            btnLeft.gameObject.SetActive(cpage > 0);
            itemText = panel.transform.Find("itemTip").GetComponent<Text>();
        }

        void OnEnable()
        {
            resetPage();
            GameData.Instance.locked = true;

            //init slot

        }
        
        void resetPage()
        {

            pages = Mathf.FloorToInt((GameData.Instance.items.Count - 1) / (float)perpage) + 1;
            slots = new List<GameObject>();
            for (int i = 0; i < perpage; i++)
            {

                Transform tSlot = Instantiate(iconFrame).transform;
                tSlot.parent = iconFrame.transform.parent;
                tSlot.gameObject.name = i.ToString();//dont change this.Here I use name as attribute
                tSlot.gameObject.SetActive(true);
                tSlot.transform.localScale = Vector3.one;
                slots.Add(tSlot.gameObject);

                int tslotIndex = i + perpage * cpage;
                if(tslotIndex == activeIndex1 || tslotIndex == activeIndex2)
                {
                    tSlot.GetComponent<Image>().color = Color.red;
                }

                

            }





            //add item to slots
            itemsCpage = new List<Item>();
            int n = 0;
            for (int i = cpage * perpage; i <cpage*perpage+perpage ; i++)
            {
                
                if (i >= GameData.Instance.items.Count) break;
                //print(i + ">>>" + cpage);
                

                Item tItem = GameData.Instance.items[i];
                if(tItem == null)
                {
                    //you did not clear the old project's data and use as in new project.
                    GameData.Instance.clearSlot();
      
                    return;
                }
                Sprite image = tItem.itemIcon;//Resources.Load<Sprite>("icons/" + tItem.itemName);
                GameObject tItemIcon = new GameObject(); //
                Image timg = tItemIcon.AddComponent<Image>();
                if (image != null)
                {
                    timg.sprite = image;

                }
                else
                {
                    //timg.sprite = new Sprite();//test
                    timg.color = new Color(1, 1, 1, .4f);
                    
                   

                }

                tItemIcon.transform.parent = slots[n].transform;
                timg.transform.position = slots[n].transform.position;
                tItemIcon.transform.localScale = Vector3.one;
                timg.transform.localScale = Vector3.one;
                timg.SetNativeSize();

                float tgridW = timg.transform.parent.GetComponent<Image>().rectTransform.rect.width;
                float tw = timg.rectTransform.rect.width;
                float th = timg.rectTransform.rect.height;

                float tsize = Mathf.Max(tw, th);
                timg.transform.localScale *= tgridW/tsize  ;
                

                itemsCpage.Add(tItem);
                n++;
            }

           

            if(btnRight!=null)btnRight.gameObject.SetActive(cpage < pages - 1);
            if (btnLeft != null) btnLeft.gameObject.SetActive(cpage > 0);
        }

        void clearPage(bool _clear = true)
        {
            //unclick
            if (_clear)
            {
                unCheckPage();
            }

            //remove all slots
            foreach (GameObject s in slots)
            {
                DestroyImmediate(s);

            }
        }

        void unCheckPage()
        {
            foreach (Transform tSlot in iconFrame.transform.parent)
            {
                tSlot.GetComponent<Image>().color = Color.white;
            }
            active1 = null;
            active2 = null;
            activeIndex1 = -1;
            activeIndex2 = -1;
            nActive = 0;

            if (btnCombine != null) btnCombine.interactable = false;
            if (btnUse != null) btnUse.interactable = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClick(GameObject g)
        {
            switch (g.name)
            {
                case "btnClose":

                    clearPage();

                    gameObject.SetActive(false);
                    GameData.Instance.rm.delayUnlock();
                    //GameManager.getInstance().playSfx("flip");
                    break;
                case "btnUse":
                    if (active1 != null) {
                        GameData.Instance.currentItem = active1.itemName;
                    }
                    else
                    {
                        GameData.Instance.currentItem = active2.itemName;
                    }
                    PlayerPrefs.SetString("currentItem" + 0, GameData.Instance.currentItem);//auto save current item info on slot 0
                    clearPage();
                    gameObject.SetActive(false);
                    transform.parent.GetComponent<GameUI>().initView();
                    GameData.Instance.rm.delayUnlock();
                   // GameManager.getInstance().playSfx("flip");
                    break;
                case "btnCombine":
                    bool combinable = false;
                    for (int i = 0;i< GameData.Instance.bluePrints.Count; i++) 
                    {
                        Blueprint tblue = GameData.Instance.bluePrints[i];
                        if(tblue.ingredientsName.Count==2){//a league blueprint
                            print(tblue.ingredientsName[0]+"_____"+ tblue.ingredientsName[1]);
                            if (tblue.ingredientsName[0] != null && tblue.ingredientsName[1] !=null && (tblue.ingredientsName[0]== active1.itemName && tblue.ingredientsName[1] == active2.itemName) ||
                                (tblue.ingredientsName[1] == active1.itemName && tblue.ingredientsName[0] == active2.itemName))
                            {

                                combinable = true;

                                GameData.Instance.items.Remove(active1);
                                GameData.Instance.items.Remove(active2);
                                Item tfinal = GameData.Instance.getItemByName(tblue.finalItemName);
                                GameData.Instance.items.Add(tfinal);
                                GameData.Instance.SaveGame();
                                clearPage();
                                resetPage();
                                GameData.Instance.currentItem = "";
                                PlayerPrefs.SetString("currentItem" + 0, GameData.Instance.currentItem);//auto save current item info on slot 0
                                GameData.Instance.gameUI.initView();
                                break;
                            }
                        }
                    }
                    if (!combinable)
                    {
                        unCheckPage();
                    }

                    //GameManager.getInstance().playSfx("flip");
                    break;
                case "btnRight":
                    if(cpage < pages)
                    {
                        cpage++;
                        clearPage(false);
                        resetPage();
                    }
                    //GameManager.getInstance().playSfx("flip");
                    break;
                case "btnLeft":
                    if (cpage > 0)
                    {
                        cpage--;
                        clearPage(false);
                        resetPage();
                    }
                   // GameManager.getInstance().playSfx("flip");
                    break;
            }
            showItemDesc();

        }




        Item active1 = null;
        Item active2 = null;

        int activeIndex1 = -1;
        int activeIndex2 = -1;
        int nActive = 0;
        public void clickSlot(GameObject g)
        {
            if (g.transform.childCount == 0)
            {
                return;
            }

            int tslotItemId = int.Parse(g.name);
            Item cSlotItem = itemsCpage[tslotItemId];
            //print(cSlotItem.itemID + "____________id");

            if (nActive >= 2)//over 2,reset
            {
                //if (active1.itemName == cSlotItem.itemName || active2.itemName == cSlotItem.itemName)
                if(activeIndex1 == cpage*perpage+tslotItemId || activeIndex2 == cpage * perpage+tslotItemId)
                {
                    //not do anything if click an acitve icon
                }
                else
                {
                    foreach (Transform tSlot in iconFrame.transform.parent)
                    {
                        tSlot.GetComponent<Image>().color = Color.white;
                    }
                    active1 = null;
                    active2 = null;
                    activeIndex1 = -1;
                    activeIndex2 = -1;
                    nActive = 0;
                }
             
            }
            Image tImage = g.GetComponent<Image>();
            if (tImage.color == Color.white)
            {
                tImage.color = Color.red;
                if (nActive < 2)
                {
                    //if (active1 == null)
                    if (activeIndex1 == -1) 
                    {
                        active1 = cSlotItem;
                        activeIndex1 = cpage * perpage+tslotItemId;
                        nActive++;

                    }
                    else
                    //if (active2 == null)
                    if(activeIndex2 == -1)
                    {
                        active2 = cSlotItem;
                        activeIndex2 = cpage * perpage + tslotItemId;
                        nActive++;

                    }
                
                }
            }
            else//already actived
            {
                tImage.color = Color.white;
                if (nActive > 0)
                {
                    //if (active1 == cSlotItem)
                    if(activeIndex1 == cpage * perpage + tslotItemId)
                    {
                        active1 = null;
                        activeIndex1 = -1;
                        nActive--;
                        

                    }
                    else if (activeIndex2 == cpage * perpage + tslotItemId)//if (active2 == cSlotItem)
                    {
                        active2 = null;
                        activeIndex2 = -1;
                        nActive--;
                        
                    }

                }


            }

            showItemDesc();

            if (btnCombine!=null)btnCombine.interactable = nActive==2;
            if(btnUse!=null)btnUse.interactable = nActive == 1;

        }

        void showItemDesc()
        {
            //set item description text
            if (nActive == 0)
            {
                //clear text
                itemText.text = "";
            }
            else if (nActive == 1)
            {
                //if only select one button
                if (active1 != null)
                {

                    Item titem = GameData.Instance.getItemByName(active1.itemName);
                    showItemDesc(titem);
                }
                //if only select one button
                if (active2 != null)
                {

                    Item titem = GameData.Instance.getItemByName(active2.itemName);
                    showItemDesc(titem);
                }
            }
            else if (nActive > 1)
            {
                itemText.text = Localization.Instance.GetString("multiselected");
            }
        }
        void showItemDesc(Item tItem)
        {
            //show item description text
            if (tItem.localId == "")
            {
                itemText.text = tItem.itemDesc;
            }
            else
            {
               
                itemText.text = Localization.Instance.GetString(tItem.localId);
            }
        }
    }
}
