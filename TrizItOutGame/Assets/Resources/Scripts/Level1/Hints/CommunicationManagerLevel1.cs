using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CommunicationManagerLevel1 : MonoBehaviour
{
    public GameObject m_CommunicationWindow;
    public GameObject m_CommunicationText;
    public TextWriter m_TextWriter;
    private string m_currentMsgWriting = string.Empty;
  
    void Start()
    {
        ComputerMission computerMission = GameObject.Find("/Missions/Computer_Mission").GetComponent<ComputerMission>();
        computerMission.OnComputer += onComputerChanged;
    }

    public void ShowMsg(string i_Msg)
    {
        StartCoroutine(ShowMsgEnumerator(i_Msg));
    }

    IEnumerator ShowMsgEnumerator(string i_Msg)
    {
        if(m_currentMsgWriting != i_Msg)
        {
            m_currentMsgWriting = i_Msg;
            m_CommunicationWindow.SetActive(true);
            m_CommunicationText.SetActive(true);
            m_TextWriter.AddWriter(m_CommunicationText.GetComponent<Text>(), i_Msg, 0.05f);

            yield return new WaitForSeconds(6);

            if (m_CommunicationText.GetComponent<Text>().text == i_Msg)
            {
                m_CommunicationWindow.SetActive(false);
                m_CommunicationText.SetActive(false);
                m_CommunicationText.GetComponent<Text>().text = string.Empty;
                m_currentMsgWriting = string.Empty;
            }
        }
    }

    public void onComputerChanged()
    {
        ShowMsg("well, it's seems like it is open. Now we need to clean up the dust! But how?");
    }
}
