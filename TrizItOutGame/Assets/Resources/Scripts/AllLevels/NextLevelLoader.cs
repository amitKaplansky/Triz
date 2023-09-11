using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextLevelLoader : MonoBehaviour
{
    public delegate void LoadingNextLevelDelegate(int i_Level);

    public Animator m_Transition;
    public float m_TransitionTime = 1f;
    private int m_NextLevelIndex;
    public Text m_TitleText;
    public Text m_NextLvlDescriptionText;
    public GameObject m_ContinueBtn;
    public GameObject m_QuitBtn;
    public TextWriter m_TextWriter;

    private List<string> m_LvlsDescriptions = new List<string>();

    public event LoadingNextLevelDelegate LoadingNextLevel;

    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");

        if (gameManager != null)
        {
            LoadingNextLevel += gameManager.GetComponent<Game>().OnLevelLoading;
        }

        m_NextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        fillLvlsDescriptions();
    }

    private void fillLvlsDescriptions()
    {
        m_LvlsDescriptions.Add(string.Empty);
        m_LvlsDescriptions.Add("The computer does turn on but it seems as if some of its components were damaged during the power outage. It will probably be easier to fix them from the inside, the question is will you be able to get back out?");
        m_LvlsDescriptions.Add("A raging fire fills the room. Your mission: extinguish the flames, your only path to freedom.");

    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        m_TitleText.text = "Level " + (m_NextLevelIndex - 1);
        m_Transition.SetTrigger("Start");
        m_ContinueBtn.SetActive(true);
        m_QuitBtn.SetActive(true);
        m_TextWriter.AddWriter(m_NextLvlDescriptionText, m_LvlsDescriptions[m_NextLevelIndex - 2], 0.05f);
        yield return new WaitForSeconds(m_TransitionTime);
        LoadingNextLevel?.Invoke(m_NextLevelIndex - 1);
    }

    public void OnClickContinueBtn()
    {
        SceneManager.LoadScene(m_NextLevelIndex);
    }

    public void OnClickQuitBtn()
    {
        SceneManager.LoadScene(0);
    }
}
