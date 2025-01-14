using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Kattle : MonoBehaviour, IInteractable
{
    public bool IsConnected { get; set; } = false;
    public  GameObject m_KattleSmoke;
    public  GameObject m_Mirror;
    public  Sprite m_MirrorWithCodeSprite;
    public  GameObject m_ButtonRightArrow;
    private GameObject m_SafeBoxClosed;
    public SpriteRenderer m_PasswordOnMirrorSpriteRenderer;

    void Start()
    {
        m_SafeBoxClosed = GameObject.Find("/Interactables_2/SafeBox/SafeBox_Closed");
        if(m_SafeBoxClosed == null)
        {
            Debug.LogError("m_SafeBoxClosed is null");
        }
        SwitchManager switchManager = GameObject.Find("/Interactables_2/Switch").GetComponent<SwitchManager>();
        switchManager.OnSwitch += OnSwitchChanged;
    }

    public void Interact(DisplayManagerLevel1 currDisplay)
    {
        if(IsConnected)
        {
            StartCoroutine(ApplyKattleSmokeAndPassword(currDisplay));
        }
        else
        {
            CommunicationUtils.FindCommunicationManagerAndShowMsg("It seems you need to plug in the kettle first.");
        }
    }

    IEnumerator ApplyKattleSmokeAndPassword(DisplayManagerLevel1 currDisplay)
    {
        SoundManager soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        m_ButtonRightArrow.GetComponent<Button>().interactable = false;
        if(m_SafeBoxClosed != null)
        {
            m_SafeBoxClosed.layer = 2;
        }

        soundManager.PlaySound(SoundManager.k_SwitchSoundName);
        soundManager.PlaySound(SoundManager.k_KattleBoilSoundName);
        yield return new WaitForSeconds(3);
        m_KattleSmoke.SetActive(true);
        yield return new WaitForSeconds(2);

        for(float i = 0.05f; i < 1f; i += 0.05f) 
        {
            Color c = m_PasswordOnMirrorSpriteRenderer.color;
            c.a = i;
            m_PasswordOnMirrorSpriteRenderer.color = c;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(3);
        m_KattleSmoke.SetActive(false);
        m_ButtonRightArrow.GetComponent<Button>().interactable = true;
        if (m_SafeBoxClosed != null)
        {
            m_SafeBoxClosed.layer = 0;
        }
    }

    public void OnSwitchChanged(bool i_IsOn)
    {
        if (i_IsOn)
        {
            gameObject.layer = 0;
        }
        else
        {
            gameObject.layer = 2;
        }
    }
}
