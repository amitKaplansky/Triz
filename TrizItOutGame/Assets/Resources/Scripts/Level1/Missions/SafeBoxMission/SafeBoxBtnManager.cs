using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SafeBoxBtnManager : MonoBehaviour
{
    public TextMeshProUGUI m_ScreenText;
    public GameObject m_SafeBoxMission;
    public GameObject m_SwitchManager;

    private readonly string m_CorrectCode = "7135";
    private string m_CurrentPassCode = string.Empty;
    private int m_Index = 0;
    private SoundManager m_SoundManager;

    public void Start()
    {
        m_SwitchManager.GetComponent<SwitchManager>().OnSwitch += OnSwitchChanged;
        m_SoundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void Update()
    {
        manageKeyboardNumbersInput();
    }

    private void manageKeyboardNumbersInput()
    {
        if (Input.inputString != "")
        {
            int number;
            bool isANumber = Int32.TryParse(Input.inputString, out number);
            if (isANumber && number >= 0 && number < 10)
            {
                OnClickSafeBoxNumberBtn(number.ToString());
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnClickOkBtn();
        }
    }

    public void OnClickSafeBoxNumberBtn(string Number)
    {
        m_SoundManager.PlaySound(SoundManager.k_ButtonSoundName);
        if (m_Index < 4)
        {
            m_Index++;
            m_CurrentPassCode = m_CurrentPassCode + Number;
            m_ScreenText.text = m_CurrentPassCode;
        }
    }

    public void OnClickClearBtn()
    {
        m_SoundManager.PlaySound(SoundManager.k_ButtonSoundName);
        m_Index = 0;
        m_CurrentPassCode = null;
        m_ScreenText.text = m_CurrentPassCode;
    }

    public void OnClickOkBtn()
    {
        SafeBoxMission safeBoxMissionScript = m_SafeBoxMission.GetComponent<SafeBoxMission>();

        if (safeBoxMissionScript == null)
        {
            Debug.LogError("safeBoxMissionScript is null on SafeBoxBtnManager.");
        }
        else
        {
            if (m_CurrentPassCode == m_CorrectCode)
            {
                m_SoundManager.PlaySound(SoundManager.k_CorrectPasswordSoundName);
                safeBoxMissionScript.ApplyPasswordCorrect();
            }
            else
            {
                m_SoundManager.PlaySound(SoundManager.k_WorngPasswordSoundName);
                OnClickClearBtn();
                safeBoxMissionScript.ApplyPasswrodInCorrect();
            }
        }
    }

    public void OnSwitchChanged(bool i_IsOn)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.GetComponent<Button>().enabled = i_IsOn;
        }

        m_ScreenText.text = string.Empty;
        m_CurrentPassCode = string.Empty;
        m_Index = 0;
    }
}
