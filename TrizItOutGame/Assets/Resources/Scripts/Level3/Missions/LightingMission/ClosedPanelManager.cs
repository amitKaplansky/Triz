using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClosedPanelManager : MonoBehaviour, IInteractable
{
    public delegate void PanelOpenedDelegate();
    public PanelOpenedDelegate panelOpenedHandler;

    [SerializeField]
    private string m_UnlockItem = "panel_key";
    private GameObject m_Inventory;

    // Start is called before the first frame update
    void Start()
    {
        m_Inventory = GameObject.Find("Inventory");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(DisplayManagerLevel1 currDisplay)
    {
        Debug.Log("I clcik");

        if (m_Inventory.GetComponent<InventoryManager>().CurrentSelectedSlot != null)
        {
            string name = m_Inventory.GetComponent<InventoryManager>().CurrentSelectedSlot.gameObject.transform.GetChild(0).GetComponent<Image>().sprite.name;
            

            if (name == m_UnlockItem )
            {
                panelOpenedHandler?.Invoke();
                m_Inventory.GetComponent<InventoryManager>().CurrentSelectedSlot.GetComponent<SlotManager>().ClearSlot();
            }
        }
    }
}
