using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisplayManagerLevel1 : MonoBehaviour
{
    public delegate void PowerFellOffDelegate();

    public   GameObject m_Missions;
    public   int m_CurrentWall;
    public   int m_PreviousWall;
    public   GameObject m_Furniture1;
    public   GameObject m_Furniture2;
    public   GameObject m_Interactables1;
    public   GameObject m_Interactables2;
    public   GameObject[] m_UiRenderObject;
    public   GameObject m_DarkMode;
    public   GameObject m_ComputerCable;
    public   Sprite m_TornComputerCableSprite;
    public   GameObject m_ComputerCableHolder;
    public   GameObject m_FuzeHolder;
    private  bool m_FuzeIsSpawned = false;
    public   bool m_ComputerCableIsSpawned = false;
    public   NextLevelLoader m_NextLevelLoader;
    public GameObject m_ComputerScreen;
    public GameObject m_ComputerWindowsLogo;
    public GameObject m_ComputerDesktopPicture;
    public Button m_LeftButton;

    public GameObject m_Lightning1, m_Lightning2;
    private const string k_BackgroundPath = "Sprites/Level1/Main_Backgrounds/Background";

    public event PowerFellOffDelegate PowerFellOff;

    public GameObject m_ReturnBtn;

    public enum State
    {
        normal, zoom, busy
    };

    public static bool m_AlreadyLoadingNextLevel = false;

    [SerializeField]
    private State m_CurrentState;

    public State CurrentState
    {
        get { return m_CurrentState; }
        set { m_CurrentState = value; }
    }
   
    public int CurrentWall
    {
        get { return m_CurrentWall; }
        set
        {
            if (value == 3)
            {
                m_CurrentWall = 1;
            }
            else if (value == 0)
            {
                m_CurrentWall = 2;
            }
            else
            {
                m_CurrentWall = value;
            }
        }
    }

    void Start()
    {
        m_PreviousWall = 0;
        m_CurrentWall = 1;
        m_CurrentState = State.normal;
        m_DarkMode.SetActive(false);
        RenderUI();
        StartCoroutine(WaitBeforeDarkMode(2));
        m_ComputerCableHolder.GetComponent<PlaceHolder>().OnPrefabSpawned += OnComputerCableSpawned;
        m_FuzeHolder.GetComponent<PlaceHolder>().OnPrefabSpawned += OnFuzeSpawned;
    }

    void Update()
    {
        if (m_CurrentWall != m_PreviousWall)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(k_BackgroundPath + CurrentWall.ToString());
        }

        m_PreviousWall = CurrentWall;
        showRelevantPickUpItems();
        showRelevantInteractableItems();

        if (m_CurrentState == State.normal)
        {
            m_Missions.GetComponent<MissionsManager>().TurnOff();
        }
        else
        {
            m_Missions.SetActive(true);
            string missionName = GetComponent<SpriteRenderer>().sprite.name;
            m_Missions.GetComponent<MissionsManager>().ActiveRelevantMission(missionName);
        }

        CheckIfFinishedLevel();
    }

    public void OnFuzeSpawned()
    {
        m_FuzeIsSpawned = true;
    }

    public void OnComputerCableSpawned()
    {
        m_ComputerCableIsSpawned = true;

        GameObject computerCable = GameObject.Find("/Furniture_1/Computer_Cable");

        if(computerCable == null)
        {
            Debug.Log("computerCable is null.");
        }
        else
        {
            computerCable.GetComponent<PickUpItem>().OnPickUp += OnComputerCablePickedUp;
        }
    }

    private void OnComputerCablePickedUp()
    {
        m_ComputerCableIsSpawned = false;
    }



    private void showRelevantInteractableItems()
    {
        if (m_CurrentWall == 1)
        {
            m_Interactables2.SetActive(false);
            if (m_CurrentState == State.normal)
            {
                m_Interactables1.SetActive(true);
            }
            else
            {
                m_Interactables1.SetActive(false);
            }
        }
        else if (m_CurrentWall == 2)
        {
            m_Interactables1.SetActive(false);
            if (m_CurrentState == State.normal)
            {
                m_Interactables2.SetActive(true);
            }
            else
            {
                m_Interactables2.SetActive(false);
            }
        }
    }

    private void showRelevantPickUpItems()
    {
        if (m_CurrentWall == 1)
        {
            m_Furniture2.SetActive(false);
            if (m_CurrentState == State.normal)
            {
                m_Furniture1.SetActive(true);
            }
            else
            {
                m_Furniture1.SetActive(false);
            }
        }

        else if (m_CurrentWall == 2)
        {
            m_Furniture1.SetActive(false);
            if (m_CurrentState == State.normal)
            {
                m_Furniture2.SetActive(true);
            }
            else
            {
                m_Furniture2.SetActive(false);
            }
        }
    }

    void RenderUI()
    {
        for (int i = 0; i < m_UiRenderObject.Length; i++)
        {
            m_UiRenderObject[i].SetActive(false);
        }
    }

    public void ChangeToNormalBackgroundAfterReturnFromZoom()
    {
        m_CurrentState = State.normal;
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(k_BackgroundPath + CurrentWall.ToString());
    }

    IEnumerator WaitBeforeDarkMode(int sec)
    {
        m_LeftButton.interactable = false;
        m_ComputerCable.layer = 2;
        yield return new WaitForSeconds(sec);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.k_ElectricFallSoundName);
        m_Lightning1.SetActive(true);
        m_Lightning2.SetActive(true);
        CommunicationUtils.FindCommunicationManagerAndShowMsg("Looks like the power went out..");
        m_ComputerScreen.SetActive(false);
        m_ComputerDesktopPicture.SetActive(false);
        m_ComputerCable.GetComponent<SpriteRenderer>().sprite = m_TornComputerCableSprite;
        m_DarkMode.SetActive(true);
        PowerFellOff?.Invoke();
        yield return new WaitForSeconds(3);
        m_LeftButton.interactable = true;
        m_ComputerCable.layer = 0;
    }

    public void CheckIfFinishedLevel()
    {
        if(m_FuzeIsSpawned && m_ComputerCableIsSpawned && !m_AlreadyLoadingNextLevel)
        {
            m_AlreadyLoadingNextLevel = true;

            if (m_CurrentState == State.zoom)
            {
                m_ReturnBtn.GetComponent<Button>().onClick.Invoke();
            }

            StartCoroutine(WaitBeforeLoadLevel2());
        }
    }

    
    IEnumerator WaitBeforeLoadLevel2()
    {
        HintsManagerLevel1.m_IsLoadingNextLevel = true;
        yield return new WaitForSeconds(1);
        m_ComputerScreen.SetActive(true);
        GameObject.Find("SoundManager").GetComponent<SoundManager>().PlaySound(SoundManager.k_WindowsStartupSoundName);
        m_ComputerDesktopPicture.SetActive(false);
        m_ComputerWindowsLogo.SetActive(true);
        yield return new WaitForSeconds(3);
        m_ComputerWindowsLogo.SetActive(false);
        m_ComputerDesktopPicture.SetActive(true);
        yield return new WaitForSeconds(3);
        m_NextLevelLoader.LoadNextLevel();
    }

    public static void onClickLevelOne()
    {
        m_AlreadyLoadingNextLevel = false;
    }
}
