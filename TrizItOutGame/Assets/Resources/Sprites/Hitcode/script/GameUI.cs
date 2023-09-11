using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Hitcode_RoomEscape
{
    public class GameUI : MonoBehaviour
    {

        // Use this for initialization
        Image mask;
        Image currentUseImg;
        Image currentImg;
        float inventoryGridW;
        Image noneImg;
        void Start()
        {
            //fade out
            GameObject tmaskob = transform.Find("Mask").gameObject;
            if (tmaskob != null)
            {
                mask = tmaskob.GetComponent<Image>();
                mask.enabled = true;
                mask.DOFade(0, 1).OnComplete(() => { mask.enabled = false; });
            }
            currentImg = transform.Find("btnInventory").Find("currentItemImg").GetComponent<Image>();
            noneImg = transform.Find("btnInventory").Find("none").GetComponent<Image>();
            inventoryGridW = transform.Find("btnInventory").GetComponent<Image>().rectTransform.rect.width;

            StartCoroutine("waitaframe");


        }

        IEnumerator waitaframe()
        {
            yield return new WaitForEndOfFrame();
            GameData.Instance.gameUI = this;
        }

        public void initView()
        {

            //verfiry the currentItem's exist
            bool currentItemExist = false;
            foreach (Item tItem in GameData.getInstance().items)
            {
                if (tItem == null) return;
                if (tItem.itemName == GameData.Instance.currentItem)
                {
                    currentItemExist = true;
                }

            }
            if (currentItemExist && GameData.Instance.currentItem != null && GameData.Instance.currentItem != "")
            {

                currentImg.sprite = GameData.Instance.getItemByName(GameData.Instance.currentItem).itemIcon;

            }
            else
            {
                currentImg.sprite = noneImg.sprite;
                currentImg.SetNativeSize();



            }
            if (currentImg != null)
            {
                currentImg.SetNativeSize();
                currentImg.transform.localScale = Vector3.one;
                float tw = currentImg.rectTransform.rect.width;
                float th = currentImg.rectTransform.rect.height;

                float tsize = Mathf.Max(tw, th);
                float tradio = inventoryGridW / tsize;
                tradio *= 0.9f;//let it a bit smaller than the container

                currentImg.transform.localScale *= tradio;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }


        public GameObject panelInventory;
        public GameObject panelText;
        public GameObject itemTip;
        public GameObject panelPause;
        public GameObject panelReadJournal;
        //public GameObject btnExitSubCam;

        public void OnClick(GameObject g)
        {
            //if (GameData.Instance.rm != null)
            //{
            //    GameData.Instance.rm.clearText();
            //}
            switch (g.name)
            {
                case "btnInventory":
                    if (GameData.Instance.locked) return;
                    panelInventory.SetActive(!panelInventory.activeSelf);
                    GameManager.getInstance().playSfx("flip");
                    break;
                case "btnExitPreviousScene":
                    Debug.Log("btnExitPreviousScene clicked");
                    GameObject.Find("btnExitPreviousScene").GetComponent<Image>().enabled = false;

                    if (GameData.Instance.cameraList != null)
                    {
                        Debug.Log("btnExitPreviousScene clicked");
                        if (GameData.Instance.locked) return;
                        print(GameData.Instance.cameraList.Count);
                        //if not at start scene
                        if (GameData.Instance.cameraList.Count > 1)
                        {
                            Camera tPrevCam = GameData.Instance.cameraList[GameData.Instance.cameraList.Count - 2];
                            Camera tCurrentCam = GameData.Instance.cameraList[GameData.Instance.cameraList.Count - 1];
                            if (tCurrentCam != null && tPrevCam != null)
                            {
                                tCurrentCam.enabled = false;
                                GameData.Instance.cameraList.RemoveAt(GameData.Instance.cameraList.Count - 1);


                               Debug.Log("tPrevCam:" + tPrevCam.name);
                                if (tPrevCam.name == "appendCamera" && tCurrentCam.name == "zoomQuiz")
                                {

                                    GameObject.Find("btnExitSubScene").GetComponent<Image>().enabled = true;
                                    GameObject submask = GameObject.Find("SubSceneMask");
                                    submask.GetComponent<Image>().enabled = true;
                                    tPrevCam.enabled = true;

                                }
                                else
                                {
                                    GameData.Instance.cameraList[0].enabled = true;
                                }
                                Debug.Log("last camera : " + tPrevCam.name + "carr cemara   "+ tCurrentCam.name);

                                if (tPrevCam.name == "appendCamera" || tPrevCam.name == "miniGame" || tPrevCam.name == "closeToTheBooks" || (tPrevCam.name == "newsPaperCamera" && tCurrentCam.name != "zoomQuiz"))
                                {
                                    Debug.Log("!remove last camera : " + tPrevCam.name + GameData.Instance.cameraList[GameData.Instance.cameraList.Count - 1].name);

                                    GameData.Instance.cameraList.RemoveAt(GameData.Instance.cameraList.Count - 2);

                                }
                                if (tCurrentCam.name == "zoomQuiz")
                                {
                                    Debug.Log(GameData.Instance.cameraList.Count - 1 +":"+ GameData.Instance.cameraList[GameData.Instance.cameraList.Count - 1].name+ "2" + GameData.Instance.cameraList[GameData.Instance.cameraList.Count - 2].name);

                                    GameData.Instance.cameraList.RemoveAt(GameData.Instance.cameraList.Count - 1);
                                    Debug.Log(GameData.Instance.cameraList[GameData.Instance.cameraList.Count - 1].name);
                                    tPrevCam = GameData.Instance.cameraList[GameData.Instance.cameraList.Count - 1];

                                }   
                                Debug.Log(tPrevCam.name);
                                if (tPrevCam.name == "Main Camera")
                                {
                                    enambelComponnet();

                                }
                                //Debug.Log("lastInTheList:" + GameData.Instance.cameraList[GameData.Instance.cameraList.Count - 2].name);

                            }

                        }
                        //only remain one or no camera,hide the back button
                        if (GameData.Instance.cameraList.Count <= 1)
                        {
                            g.GetComponent<Image>().enabled = false;

                        }
                        //clear text
                        GameData.Instance.rm.clearText();
                        GameManager.getInstance().playSfx("flip");
                        g.GetComponent<Image>().enabled = false;
                        // todo added line 142

                    }
                    break;
                case "btnExitSubScene":
                    if (GameData.Instance.currentSubCam != null)
                    {
                        if (GameData.Instance.locked) return;
                        GameData.Instance.currentSubCam.enabled = false;
                        GameData.Instance.currentSubCam = null;
                        //only remain one or no camera,hide the back button
                        if (GameData.Instance.cameraList.Count <= 1)
                        {

                            GameObject.Find("btnExitPreviousScene").GetComponent<Image>().enabled = false;

                        }
                        else//got more than more cam in list,can undo
                        {
                            GameObject.Find("btnExitPreviousScene").GetComponent<Image>().enabled = false;
                        }
                    }
                    Debug.Log($"Camera list{GameData.Instance.cameraList}");
                    Camera tPrevCam1 = GameData.Instance.cameraList[GameData.Instance.cameraList.Count - 2];
                    Camera tCurrentCam1 = GameData.Instance.cameraList[GameData.Instance.cameraList.Count - 1];
                    if (tCurrentCam1 != null && tPrevCam1 != null)
                    {

                        Debug.Log("tPrevCam:" + tPrevCam1.name);
                        Debug.Log("tCurrentCam1:" + tCurrentCam1.name); 


                        if (tPrevCam1.name == "Main Camera")
                        {
                            enambelComponnet();

                        }
                        if (tCurrentCam1.name == "appendCamera" || tCurrentCam1.name == "miniGame" || tCurrentCam1.name == "closeToTheBooks" || tCurrentCam1.name == "newsPaperCamera")
                        {
                            Debug.Log("get in to the econdition");
                            GameData.Instance.cameraList.RemoveAt(GameData.Instance.cameraList.Count - 1);

                        }
                    }
                    GameObject.Find("SubSceneMask").GetComponent<Image>().enabled = false;
                    GameObject.Find("btnExitSubScene").GetComponent<Image>().enabled = false;
                    GameData.Instance.areaGame = false;
                    //clear text
                    GameData.Instance.rm.clearText();
                    //lock game to forbid too fast next command
                    GameData.Instance.locked = true;
                    GameData.Instance.rm.delayUnlock();
                    GameManager.getInstance().playSfx("flip");
                    GameObject tg =  GameObject.Find("gameContainer");
                    if (tg != null)
                    {
                        tg.BroadcastMessage("subSceneClosed",SendMessageOptions.DontRequireReceiver);
                    }
                    break;
                case "btnPause":
                    if (GameData.Instance.locked) return;
                    GameData.Instance.locked = true;
                    Time.timeScale = 0;

                    panelPause.SetActive(true);
                    GameManager.getInstance().playSfx("flip");
                    break;
                //case "btnExitSubCam":
                //    CameraMotor tPerviousCam = GameObject.Find(VariablesManager.GetGlobal("perviousCam") as string).GetComponent<CameraMotor>();
                //    GameObject.Find("MainCamera").GetComponent<CameraController>().currentCameraMotor = tPerviousCam;
                //    VariablesManager.SetGlobal("isSubCam", false);
                //    g.SetActive(false);
                //    GameObject.Find("exitSubCam(controlPart)").GetComponent<GameCreator.Core.Actions>().Execute();
                //    break;
            }
        }

        private void enambelComponnet()
        {
            GameObject stoneObject = GameObject.Find("stone");

            if (stoneObject != null)
            {
                stoneObject.GetComponent<PolygonCollider2D>().enabled = true;
            }
            if (GameObject.Find("passwordGame") != null)
            {
                GameObject.Find("passwordGame").GetComponent<BoxCollider>().enabled = true;
            }
            if (GameObject.Find("Key") != null)
            {
                GameObject.Find("Key").GetComponent<PolygonCollider2D>().enabled = true;
            }
            if (GameObject.Find("breakMeBox") != null)
            {
                GameObject.Find("breakMeBox").GetComponent<BoxCollider2D>().enabled = true;
            }
            if (GameObject.Find("Door") != null)
            {
                GameObject.Find("Door").GetComponent<BoxCollider2D>().enabled = true;
            }
            if (GameObject.Find("newspaper") != null)
            {
                GameObject.Find("newspaper").GetComponent<PolygonCollider2D>().enabled = true;
            }
            if (GameObject.Find("books") != null)
            {
                GameObject.Find("books").GetComponent<PolygonCollider2D>().enabled = true;
            }
            if (GameObject.Find("zoomToPaint") != null)
            {
                GameObject.Find("zoomToPaint").GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
}
