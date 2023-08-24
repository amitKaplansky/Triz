using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMissionHandler : MonoBehaviour
{
    public TMPro.TextMeshProUGUI m_ScreenText;
    public Action doorWasOpendEvent;

    private GameObject m_OpenDoor;
    private string m_CorrectCode = "2468";
    private string m_CurrentCode = null;
    private int m_CurrentIndex = 0;

    private SoundManager m_SoundManager;

    void Start()
    {
        m_SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_OpenDoor = GameObject.Find("DoorOpen");
        Debug.Log(m_OpenDoor);
    }

    void Update()
    {
  
    }

    public void OnClickNumberBtn(int i_Number)
    {
        m_SoundManager.PlaySound(SoundManager.k_ButtonSoundName);
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
                doorWasOpendEvent?.Invoke();
                m_SoundManager.PlaySound(SoundManager.k_CorrectPasswordSoundName);
                GameObject.Find("Door").GetComponent<SpriteRenderer>().enabled = false;
                m_OpenDoor.GetComponent<SpriteRenderer>().enabled = true;
                NextLevelLoader nextLevelLoader = GameObject.Find("Next_Level_Loader").GetComponent<NextLevelLoader>();
                nextLevelLoader.LoadNextLevel();
            }
            else
            {
                m_SoundManager.PlaySound(SoundManager.k_WorngPasswordSoundName);
                m_ScreenText.text = string.Empty;
                m_CurrentIndex = 0;
                m_CurrentCode = string.Empty;
            }
        }
    }

}
