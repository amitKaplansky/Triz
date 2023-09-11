using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSoundManager : MonoBehaviour
{
    private float m_Timer;
    private SoundManager m_SoundManager;

    void Start()
    {
        m_SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    void Update()
    {
        if(!LightningManager.s_TornComputerCablePickedUp)
        {
            manageElectricSound();
        }       
    }

    private void manageElectricSound()
    {

        if (LightningManager.s_NeedToPlayElectricSound)
        {
            m_Timer = m_Timer + Time.deltaTime;
            if (m_Timer >= 2f)
            {
                m_SoundManager.PlaySound(SoundManager.k_ElectricitySoundName);
                m_Timer = 0;
            }
        }
        else
        {
            m_SoundManager.StopSound();
        }
    }
}
