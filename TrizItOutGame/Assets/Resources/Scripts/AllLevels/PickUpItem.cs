using System.Diagnostics;
using UnityEngine;

public class PickUpItem : MonoBehaviour, IInteractable, IRespondsCursor
{
    public delegate void PickUpAction();
    public enum eProperty { usable, displayable };

    public string m_ItemName;
    public string m_DisplaySpriteName;    
    public eProperty m_ItemProperty;
    public string m_ExtraDisplaySpriteName; 
    public string m_ResultOfCombinationItemName;
    public int m_AmountOfUsage;

    public event PickUpAction OnPickUp;

    private InventoryManager m_InventoryManager;

    void Start()
    {
        m_InventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
    }

    public void Interact(DisplayManagerLevel1 currDisplay)
    {
        UnityEngine.Debug.Log($"this is the interact of {m_InventoryManager}");

        itemPickUp();
    }

    private void itemPickUp()
    {
        m_InventoryManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
        m_InventoryManager.AddItemToInventory(this);
        OnPickUp?.Invoke();
    }

    public void OnMouseEnter()
    {
        UnityEngine.Debug.Log($"this is the mouse enter of {m_InventoryManager}");
        GameObject.Find("Cursor_Manager").GetComponent<CursorManager>().ChangeCursorToHighlight();
    }

    public void OnMouseExit()
    {
        GameObject.Find("Cursor_Manager").GetComponent<CursorManager>().ChangeCursorToNormal();
    }

    public void OnMouseDown()
    {
        UnityEngine.Debug.Log($"mouse down {m_InventoryManager}");

        GameObject.Find("Cursor_Manager").GetComponent<CursorManager>().ChangeCursorToNormal();
    }
}
