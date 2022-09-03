using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMissionHandler : MonoBehaviour
{
    public TMPro.TextMeshProUGUI m_ScreenText;

    private GameObject m_OpenDoor;
    private string m_CorrectCode = "2468";
    private string m_CurrentCode = null;
    private int m_CurrentIndex = 0;

    void Start()
    {
        m_OpenDoor = GameObject.Find("DoorOpen");
        Debug.Log(m_OpenDoor);
    }

    void Update()
    {
  
    }

    public void OnClickNumberBtn(int i_Number)
    {
        SoundManager.PlaySound(SoundManager.k_ButtonSoundName);
        if (m_CurrentIndex < 4)
        {
            m_CurrentIndex++;
            m_CurrentCode = m_CurrentCode + i_Number;
            m_ScreenText.text = m_CurrentCode;
        }

        if (m_CurrentIndex == 4)
        {
            if (m_CurrentCode == m_CorrectCode)
            {
                Debug.Log("Correct PASS!!!");
                SoundManager.PlaySound(SoundManager.k_CorrectPasswordSoundName);
                m_OpenDoor.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                SoundManager.PlaySound(SoundManager.k_WorngPasswordSoundName);
                Debug.Log("Wrong PASS!!!");
                m_ScreenText.text = string.Empty;
                m_CurrentIndex = 0;
                m_CurrentCode = string.Empty;
            }
        }
    }

}