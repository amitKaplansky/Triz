using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotManager : MonoBehaviour, IPointerClickHandler
{
    public delegate void ZoomBtnPressedDelegate();

    public enum eProperty { Usable, Displayable, Empty };

    public string ItemNameForZoomInWindow { get; set; }
    public string DisplayImage { get; set; }
    public bool IsEmpty { get; set; } = true;
    public string CombinationItem { get; set; }
    public eProperty ItemProperty { get; set; }
    public int AmountOfUsage { get; set; }

    public GameObject m_SlotItemImage;
    public GameObject m_ZoomInWindow;
    public Text m_ZoomInWindowText;
    private InventoryManager m_InventoryManager;

    public event ZoomBtnPressedDelegate OnClickZoomBtn;

    void Start()
    {
        m_InventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
    }

    public void OnClickMagnifierGlass()
    {
        if(IsEmpty == false)
        {
            OnClickZoomBtn?.Invoke();
            if (ItemProperty == eProperty.Usable)
            {
                m_ZoomInWindow.transform.Find("Item").GetComponent<Image>().sprite = m_SlotItemImage.GetComponent<Image>().sprite;
             
            }
            else
            {
                m_ZoomInWindow.transform.Find("Item").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Level1/Displayables/" + DisplayImage);
            }

            m_ZoomInWindowText.text = ItemNameForZoomInWindow;
            m_ZoomInWindow.SetActive(true);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(this.gameObject == m_InventoryManager.CurrentSelectedSlot)
        {
            m_InventoryManager.CurrentSelectedSlot = null;
        }
        else if(IsEmpty == false)
        {
           m_InventoryManager.PreviouslySelectedSlot = m_InventoryManager.CurrentSelectedSlot;
           m_InventoryManager.CurrentSelectedSlot = this.gameObject;
           Combine();
        }
    }

    public void AssignProperty(string i_ItemName, int i_OrdrNumber, string i_DisplayImage, string i_CombinationItem, int i_AmountOfUsage)
    {
        ItemProperty = (eProperty)i_OrdrNumber;
        this.ItemNameForZoomInWindow = i_ItemName;
        this.DisplayImage = i_DisplayImage;
        this.CombinationItem = i_CombinationItem;
        this.AmountOfUsage = i_AmountOfUsage;
    }

    public void Combine()
    {
        if(m_InventoryManager.PreviouslySelectedSlot != null &&
            m_InventoryManager.PreviouslySelectedSlot.GetComponent<SlotManager>().CombinationItem == GetComponent<SlotManager>().CombinationItem 
            && GetComponent<SlotManager>().CombinationItem != string.Empty)
        {
            var combinedItem = Instantiate(Resources.Load<GameObject>("Combined Items/" + CombinationItem));

            m_InventoryManager.PreviouslySelectedSlot.GetComponent<SlotManager>().ClearSlot();
            ClearSlot();
            combinedItem.GetComponent<PickUpItem>().Interact(null);
        }
    }

    public void ClearSlot()
    {
        AmountOfUsage--;
        if(AmountOfUsage == 0)
        {
            ItemProperty = SlotManager.eProperty.Empty;
            DisplayImage = string.Empty;
            CombinationItem = string.Empty;
            IsEmpty = true;
            transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/AllLevels/Inventory/Empty_Item");
        }
    }

    public string GetItemName()
    {
        return m_SlotItemImage.GetComponent<Image>().sprite.name;
    }

    public void ResetSlot()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/AllLevels/Inventory/Empty_Item");
        IsEmpty = true;
        CombinationItem = string.Empty;
        ItemProperty = SlotManager.eProperty.Empty;
        AmountOfUsage = 0;
        DisplayImage = string.Empty;
    }

    public void SetSlotData(SlotTempData i_SlotData)
    {
      //  Debug.Log("Item name = " + i_SlotData.ItemName);
        ItemNameForZoomInWindow = i_SlotData.ItemName;
        m_SlotItemImage.GetComponent<Image>().sprite = i_SlotData.SlotImageSprite;
        IsEmpty = false;
        CombinationItem = i_SlotData.CombinationItem;
        ItemProperty = i_SlotData.Property;
        AmountOfUsage = i_SlotData.AmountOfUsage;
        DisplayImage = i_SlotData.DisplayImageName;
    }
}
