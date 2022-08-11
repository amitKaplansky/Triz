using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButtonManager : MonoBehaviour
{
    [SerializeField]
    private Sprite m_MutedSprite;
    [SerializeField]
    private Sprite m_NotMutedSprite;

    public void OnClickMuteButton()
    {
        SoundManager.s_IsMuted = !SoundManager.s_IsMuted;

        if(SoundManager.s_IsMuted)
        {
            gameObject.GetComponent<Image>().sprite = m_MutedSprite;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = m_NotMutedSprite;
        }
    }
}
