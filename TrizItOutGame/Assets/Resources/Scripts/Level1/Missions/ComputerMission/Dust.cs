using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dust : MonoBehaviour, IInteractable
{
    public delegate void CleanDustAction();

    private string m_UnlockItem1 = "Spray_With_Straw";
    private string m_UnlockItem2 = "Broom";
    private GameObject m_inventory;

    public event CleanDustAction OnCleanUp;

    void Start()
    {
        m_inventory = GameObject.Find("Inventory");
    }

    public void Interact(DisplayManagerLevel1 currDisplay)
    {
        GameObject currentSelectedSlot = m_inventory.GetComponent<InventoryManager>().CurrentSelectedSlot;
        SoundManager soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        if (currentSelectedSlot != null)
        {
            string selectedSlotSpriteName = currentSelectedSlot.gameObject.transform.GetChild(0).GetComponent<Image>().sprite.name;
            if (selectedSlotSpriteName == m_UnlockItem1 || selectedSlotSpriteName == m_UnlockItem2)
            {
                if (selectedSlotSpriteName == m_UnlockItem1)
                {
                    soundManager.PlaySound(SoundManager.k_SpraySoundName);
                }
                else if(selectedSlotSpriteName == m_UnlockItem2)
                {
                    soundManager.PlaySound(SoundManager.k_BroomSoundName);
                }

                Destroy(gameObject);
                OnCleanUp?.Invoke();
                m_inventory.GetComponent<InventoryManager>().CurrentSelectedSlot.GetComponent<SlotManager>().ClearSlot();
            }
            else 
            {
                CommunicationUtils.FindCommunicationManagerAndShowMsg("This object cannot clean the dust..");
            }
        }
        else 
        {
            CommunicationUtils.FindCommunicationManagerAndShowMsg("You need an object to help you clean the dust..");
        }
    }
}