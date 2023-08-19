using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class openDoor : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SoundManager m_SoundManager;
    public Camera nextRoomCamera;
    private string m_UnlockItem = "room1_key1";
    private GameObject m_Inventory;
    public bool isDoorLocked;
    // Start is called before the first frame update
    void Start()
    {
        m_SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_Inventory = GameObject.Find("Inventory");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact(DisplayManagerLevel1 currDisplay)
    {
        if (isDoorLocked)
        {
            if (m_Inventory.GetComponent<InventoryManager>().CurrentSelectedSlot != null)
            {
                string name = m_Inventory.GetComponent<InventoryManager>().CurrentSelectedSlot.gameObject.transform.GetChild(0).GetComponent<Image>().sprite.name;

                Debug.Log(name);
                if (name == m_UnlockItem)
                {
                    m_SoundManager.PlaySound(SoundManager.k_OpenDoorSound);
                    SwitchCamera();
                    m_Inventory.GetComponent<InventoryManager>().CurrentSelectedSlot.GetComponent<SlotManager>().ClearSlot();
                    isDoorLocked = false;
                }
            }
        }
        else
        {
            SwitchCamera();
        }
    }

    public void SwitchCamera()
    {
     
            // Disable the current camera
            Camera currentCamera = Camera.main;
            currentCamera.enabled = false;

            // Enable the 'NextRoom' camera
            nextRoomCamera.enabled = true;
        
    }
       
}
