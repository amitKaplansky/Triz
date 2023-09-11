using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class breakTheGlass : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SoundManager m_SoundManager;
    private GameObject m_brokenGlass;
    private string m_UnlockItem = "stoneAxe";
    private GameObject m_Inventory;
    private int newSortingLayer = 23;
   // private GameObject m_fireSaver;

    // Start is called before the first frame update
    void Start()
    {
        m_SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_brokenGlass = GameObject.Find("brokenGlass");
       // m_fireSaver = GameObject.Find("fireSaver");
        m_Inventory = GameObject.Find("Inventory");
    }

    // Update is called once per frame
    void Update()
    {
       

    }
    public void Interact(DisplayManagerLevel1 currDisplay)
    {
        if (m_Inventory.GetComponent<InventoryManager>().CurrentSelectedSlot != null)
        {
            string name = m_Inventory.GetComponent<InventoryManager>().CurrentSelectedSlot.gameObject.transform.GetChild(0).GetComponent<Image>().sprite.name;
            

            if (name == m_UnlockItem)
            {
                //   GameObject.Find("breakMeBox").GetComponent<SpriteRenderer>().enabled = false;
                m_SoundManager.PlaySound(SoundManager.k_BreakGlassSound);
                m_brokenGlass.GetComponent<SpriteRenderer>().sortingOrder = newSortingLayer;
                m_Inventory.GetComponent<InventoryManager>().CurrentSelectedSlot.GetComponent<SlotManager>().ClearSlot();

                GameObject.Find("fireSaver").GetComponent<SpriteRenderer>().sortingOrder = 20;

                GameObject.Find("fireSaver").tag = "Interactable";

            }
        }
    }
}
