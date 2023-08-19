using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] m_Slots = new GameObject[6]; 
    public GameObject CurrentSelectedSlot { get; set; }
    public GameObject PreviouslySelectedSlot { get; set; }

    private Sprite m_EmptyItemSprite = null;

    public Sprite m_SlotSelectSprite;
    public Sprite m_SlotNormalSprite;

    public static readonly string sr_EmptyItemName = "Empty_Item";
    public static readonly string sr_InventoryItemSpritePath = "Sprites/AllLevels/Items/";
    public static readonly string sr_EmptyItemSpritePath = "Sprites/AllLevels/Inventory/";

    private SoundManager m_SoundManager;

    private void Start()
    {
         m_SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        Debug.Log("Start Inventory");
        m_EmptyItemSprite = Resources.Load<Sprite>(sr_EmptyItemSpritePath + sr_EmptyItemName);
        initializeInventory();
    }

    private void Update()
    {
        selectedSlot();
        arrangeInventory();
    }

    private void initializeInventory()
    {
        foreach(GameObject slot in m_Slots)
        { 
            Debug.Log("was here");

        slot.transform.GetChild(0).GetComponent<Image>().sprite = m_EmptyItemSprite;
        }
    }

    private void selectedSlot()
    {
        foreach(GameObject slot in m_Slots)
        {
            if(slot == CurrentSelectedSlot && slot.GetComponent<SlotManager>().ItemProperty == SlotManager.eProperty.Usable 
                && slot.GetComponent<SlotManager>().IsEmpty == false)
            {
                slot.GetComponent<Image>().sprite = m_SlotSelectSprite;
            }
            else
            {
                slot.GetComponent<Image>().sprite = m_SlotNormalSprite;
            }
        }
    }

    public void AddItemToInventory(PickUpItem i_Item)
    {
        m_SoundManager.PlaySound(SoundManager.k_TakeItemSoundName);
        if (i_Item.m_AmountOfUsage != 0)
        {
            Debug.Log("Add item");
            GameObject firstEmptySlot = findFirstEmpetySlot();

            firstEmptySlot.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(sr_InventoryItemSpritePath + i_Item.m_DisplaySpriteName);
            firstEmptySlot.GetComponent<SlotManager>().IsEmpty = false;
            firstEmptySlot.GetComponent<SlotManager>().AssignProperty(i_Item.m_ItemName, (int)i_Item.m_ItemProperty, i_Item.m_ExtraDisplaySpriteName, i_Item.m_ResultOfCombinationItemName, i_Item.m_AmountOfUsage);
            Destroy(i_Item.gameObject);
        }

        Destroy(i_Item.gameObject);
    }

    private GameObject findFirstEmpetySlot()
    {
        foreach(GameObject slot in m_Slots)
        {
            if(slot.GetComponent<SlotManager>().IsEmpty)
            {
                return slot;
            }
        }

        return null;
    }
    
    public bool DoesItemInInventory(string i_ItemName)
    {
        foreach(GameObject slot in m_Slots)
        {
            if(slot.GetComponent<SlotManager>().GetItemName() == i_ItemName)
            {
                return true;
            }
        }

        return false;
    }

    public bool RemoveFromInventory(string i_ItemName)
    {
        GameObject slotWithTheItem = findSlotWithItem(i_ItemName);

        if (slotWithTheItem != null)
        {
            slotWithTheItem.GetComponent<SlotManager>().AmountOfUsage = 1;
            slotWithTheItem.GetComponent<SlotManager>().ClearSlot();
            return true;
        }

        return false;
    }

    private GameObject findSlotWithItem(string i_ItemName)
    {
        foreach(GameObject slot in m_Slots)
        {
            if (slot.GetComponent<SlotManager>().GetItemName() == i_ItemName)
            {
                return slot;
            }
        }

        return null;
    }

    private void arrangeInventory()
    {
        List<SlotTempData> usedSlotsData = getUsedSlotsData();

        for (int i = 0; i < m_Slots.Length; i++)
        {
            SlotManager currentSlotManager = m_Slots[i].GetComponent<SlotManager>();
            if (i < usedSlotsData.Count)
            {

                currentSlotManager.SetSlotData(usedSlotsData[i]);
            }
            else
            {
                currentSlotManager.ResetSlot();
            }
        }
    }

    private List<SlotTempData> getUsedSlotsData()
    {
        List<SlotTempData> usedSlotsData = new List<SlotTempData>();

        foreach (GameObject slot in m_Slots)
        {
            SlotManager currentSlotManager = slot.GetComponent<SlotManager>();
            if (!currentSlotManager.IsEmpty)
            {
                usedSlotsData.Add(new SlotTempData(currentSlotManager.ItemNameForZoomInWindow, currentSlotManager.m_SlotItemImage.GetComponent<Image>().sprite,
                    currentSlotManager.CombinationItem, currentSlotManager.ItemProperty, currentSlotManager.AmountOfUsage, currentSlotManager.DisplayImage));
            }
        }

        return usedSlotsData;
    }
}
