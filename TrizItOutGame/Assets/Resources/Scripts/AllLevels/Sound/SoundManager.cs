using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager m_Instance;

    public static AudioClip s_ButtonSound, s_WorngPasswordSound,
                            s_CorrectPasswordSound, s_SwitchSound,
                            s_SpraySound, s_DoorOpenSound,
                            s_TakeItemSound, s_DrawerSound,
                            s_KattleBoilSound, s_MoveBigPotSound,
                            s_ElectricFallSound, s_PiggyBankBreakSound,
                            s_ScrewOpenSound, s_QuizCorrectAnswerSound, s_QuizWrongAnswerSound,
                            s_PiggyBankKnockSound, s_FanSound, s_ElectricitySound, s_WindowsStartupSound,
                            s_HintSound, s_BroomSound,s_BreakGlassSound, s_OpenDoorSound;

    public AudioSource m_AudioSource;

    public const string k_ButtonSoundName = "buttonSound";
    public const string k_WorngPasswordSoundName = "worngPasswordSound";
    public const string k_CorrectPasswordSoundName = "correctPasswordSound";
    public const string k_SwitchSoundName = "switchSound";
    public const string k_SpraySoundName = "spraySound";
    public const string k_DoorOpenSoundName = "doorOpenSound";
    public const string k_TakeItemSoundName = "takeItemSound";
    public const string k_DrawerSoundName = "drawerSound";
    public const string k_KattleBoilSoundName = "kattleBoilSound";
    public const string k_MoveBigPotSoundName = "moveBigPotSound";
    public const string k_ElectricFallSoundName = "electricFallSound";
    public const string k_PiggyBankBreakSoundName = "piggyBankBreakSound";
    public const string k_ScrewOpenSoundName = "screwOpenSound";
    public const string k_QuizCorrectAnswerSoundName = "quizCorrectAnswerSound";
    public const string k_QuizWrongAnswerSoundName = "quizWrongAnswerSound";
    public const string k_PiggyBankKnockSoundName = "piggyBankKnockSound";
    public const string k_FanSoundName = "fanSound";
    public const string k_ElectricitySoundName = "electricitySound";
    public const string k_WindowsStartupSoundName = "windowsStartupSound";
    public const string k_HintSoundName = "hintSound";
    public const string k_BroomSoundName = "broomSound";
    public const string k_BreakGlassSound = "BreakingGlass";
    public const string k_OpenDoorSound = "openDoor";



    public static readonly string sr_SoundPath = "Sounds/";

    public static bool s_IsMuted = false;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        m_AudioSource = GetComponent<AudioSource>();  

        s_ButtonSound = Resources.Load<AudioClip>(sr_SoundPath + k_ButtonSoundName);
        s_WorngPasswordSound = Resources.Load<AudioClip>(sr_SoundPath + k_WorngPasswordSoundName);
        s_CorrectPasswordSound = Resources.Load<AudioClip>(sr_SoundPath + k_CorrectPasswordSoundName);
        s_SwitchSound = Resources.Load<AudioClip>(sr_SoundPath + k_SwitchSoundName);
        s_SpraySound = Resources.Load<AudioClip>(sr_SoundPath + k_SpraySoundName);
        s_DoorOpenSound = Resources.Load<AudioClip>(sr_SoundPath + k_DoorOpenSoundName);
        s_TakeItemSound = Resources.Load<AudioClip>(sr_SoundPath + k_TakeItemSoundName);
        s_DrawerSound = Resources.Load<AudioClip>(sr_SoundPath + k_DrawerSoundName);
        s_KattleBoilSound = Resources.Load<AudioClip>(sr_SoundPath + k_KattleBoilSoundName);
        s_MoveBigPotSound = Resources.Load<AudioClip>(sr_SoundPath + k_MoveBigPotSoundName);
        s_ElectricFallSound = Resources.Load<AudioClip>(sr_SoundPath + k_ElectricFallSoundName);
        s_PiggyBankBreakSound = Resources.Load<AudioClip>(sr_SoundPath + k_PiggyBankBreakSoundName);
        s_ScrewOpenSound = Resources.Load<AudioClip>(sr_SoundPath + k_ScrewOpenSoundName);
        s_QuizCorrectAnswerSound = Resources.Load<AudioClip>(sr_SoundPath + k_QuizCorrectAnswerSoundName);
        s_QuizWrongAnswerSound = Resources.Load<AudioClip>(sr_SoundPath + k_QuizWrongAnswerSoundName);
        s_PiggyBankKnockSound = Resources.Load<AudioClip>(sr_SoundPath + k_PiggyBankKnockSoundName);
        s_FanSound = Resources.Load<AudioClip>(sr_SoundPath + k_FanSoundName);
        s_ElectricitySound = Resources.Load<AudioClip>(sr_SoundPath + k_ElectricitySoundName);
        s_WindowsStartupSound = Resources.Load<AudioClip>(sr_SoundPath + k_WindowsStartupSoundName);
        s_HintSound = Resources.Load<AudioClip>(sr_SoundPath + k_HintSoundName);
        s_BroomSound = Resources.Load<AudioClip>(sr_SoundPath + k_BroomSoundName);
        s_BreakGlassSound = Resources.Load<AudioClip>(sr_SoundPath + k_BreakGlassSound);
        s_OpenDoorSound = Resources.Load<AudioClip>(sr_SoundPath + k_OpenDoorSound);


    }

    public void PlaySound(string i_Clip)
    {
        if (!s_IsMuted)
        {
            switch (i_Clip)
            {
                case k_ButtonSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_ButtonSound);
                        break;
                    }
                case k_WorngPasswordSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_WorngPasswordSound);
                        break;
                    }
                case k_CorrectPasswordSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_CorrectPasswordSound);
                        break;
                    }
                case k_SwitchSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_SwitchSound);
                        break;
                    }
                case k_SpraySoundName:
                    {
                        m_AudioSource.PlayOneShot(s_SpraySound);
                        break;
                    }
                case k_DoorOpenSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_DoorOpenSound);
                        break;
                    }
                case k_TakeItemSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_TakeItemSound);
                        break;
                    }
                case k_DrawerSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_DrawerSound);
                        break;
                    }
                case k_KattleBoilSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_KattleBoilSound);
                        break;
                    }
                case k_MoveBigPotSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_MoveBigPotSound);
                        break;
                    }
                case k_ElectricFallSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_ElectricFallSound, 0.5f);
                        break;
                    }
                case k_PiggyBankBreakSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_PiggyBankBreakSound, 0.7f);
                        break;
                    }
                case k_ScrewOpenSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_ScrewOpenSound);
                        break;
                    }
                case k_QuizCorrectAnswerSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_QuizCorrectAnswerSound);
                        break;
                    }
                case k_QuizWrongAnswerSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_QuizWrongAnswerSound);
                        break;
                    }
                case k_PiggyBankKnockSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_PiggyBankKnockSound);
                        break;
                    }
                case k_FanSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_FanSound);
                        break;
                    }
                case k_ElectricitySoundName:
                    {
                        m_AudioSource.PlayOneShot(s_ElectricitySound);
                        break;
                    }
                case k_WindowsStartupSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_WindowsStartupSound);
                        break;
                    }
                case k_HintSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_HintSound);
                        break;
                    }
                case k_BroomSoundName:
                    {
                        m_AudioSource.PlayOneShot(s_BroomSound);
                        break;
                    }
                case k_BreakGlassSound:
                    {
                        m_AudioSource.PlayOneShot(s_BreakGlassSound);
                        break;
                    }
                case k_OpenDoorSound:
                    {
                        m_AudioSource.PlayOneShot(s_OpenDoorSound);
                        break;
                    }
            }
        }
    }

    public void StopSound()
    {
        m_AudioSource.Stop();
    }

    public void OnToggleChanged()
    {
        m_AudioSource.mute = !m_AudioSource.mute;
    }

    public static AudioClip FindAudioClip(string i_Name)
    {
        return Resources.Load<AudioClip>(sr_SoundPath + i_Name);
    }
}

