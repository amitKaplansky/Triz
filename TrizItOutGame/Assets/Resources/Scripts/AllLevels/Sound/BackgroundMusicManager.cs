using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    public static BackgroundMusicManager s_Instance;
    private AudioSource m_AudioSource;
    private int m_PrevSceneIndex = 0;
    private bool m_QuizMusicAlreadyStarted = false;
    public static bool QuizStartWorking { get; set; } = false;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if(s_Instance == null)
        {
            s_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        m_AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        checkIfNeedToPlayQuizMusic(currentSceneIndex);
       
        if (currentSceneIndex != m_PrevSceneIndex)
        {
            switch (currentSceneIndex)
            {
                case 0: // MainMenu
                    {
                        m_AudioSource.clip = SoundManager.FindAudioClip("Zephyr");
                        m_AudioSource.volume = 0.6f;
                        m_AudioSource.Play();
                        break;
                    }
                case 1: // Levels
                    {
                        break;
                    }
                case 2: // Level 1
                    {
                        m_AudioSource.clip = SoundManager.FindAudioClip("backgroundMusicLevel1");
                        m_AudioSource.volume = 0.6f;
                        m_AudioSource.Play();
                        break;
                    }
                case 3: // Level 2
                    {
                        m_AudioSource.clip = SoundManager.FindAudioClip("backgroundMusicLevel2");
                        m_AudioSource.volume = 0.6f;
                        m_AudioSource.Play();
                        break;
                    }
                case 4: // Level 3
                    {
                        m_AudioSource.clip = SoundManager.FindAudioClip("level4BackgroundMusic");
                        m_AudioSource.volume = 0.6f;
                        m_AudioSource.Play();
                        break;
                    }
                case 5://level 4
                    {
                        m_AudioSource.clip = SoundManager.FindAudioClip("level4BackgroundMusic");
                        m_AudioSource.volume = 0.6f;
                        m_AudioSource.Play();
                        break;
                    }
            }
            
            m_PrevSceneIndex = currentSceneIndex;
        }
    }

    private void checkIfNeedToPlayQuizMusic(int i_SceneIndex)
    {
        if(i_SceneIndex == 3)
        {
            if (QuizStartWorking && !m_QuizMusicAlreadyStarted)
            {
                m_AudioSource.clip = SoundManager.FindAudioClip("quizBackgroundMusic");
                m_AudioSource.volume = 1;
                m_QuizMusicAlreadyStarted = true;
                m_AudioSource.Play();
            }
        }
    }

    public void OnToggleChanged()
    {
        m_AudioSource.mute = !m_AudioSource.mute;
    }
}
