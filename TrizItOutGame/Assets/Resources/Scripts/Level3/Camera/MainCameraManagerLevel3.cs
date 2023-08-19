using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCameraManagerLevel3 : MonoBehaviour
{
    public static int m_CurrentWallIndex = 1;
    private int m_PreviousWallIndex = 1;
    private int m_WallBeforeMission = 0;
    private float m_DistanceToMoveXOfCamera = 17.8f;
    private bool m_CanGoRight = false;

    [SerializeField]
    private GameObject m_LeftBtn, m_RightBtn, m_GoBackBtn;

    private static readonly int sr_MostRightWallIndex = 4;

    public int CurrentWallIndex
    {
        get { return m_CurrentWallIndex; }
        set
        {
            if (value == sr_MostRightWallIndex + 1)
            {
                m_CurrentWallIndex = 1;
            }
            else if (value == 0)
            {
                m_CurrentWallIndex = sr_MostRightWallIndex;
            }
            else
            {
                m_CurrentWallIndex = value;
            }
        }
    }

    private void Update()
    {
        manageCameraPosition();
        manageLeftAndRightsBtnsActivation();
    }

    private void Start()
    {
        GameObject.Find("Door").GetComponent<ChangeToMission>().MissionWasChosen += Mission_Interact;
        GameObject.Find("SafeBox_Close").GetComponent<ChangeToMission>().MissionWasChosen += Mission_Interact;
        GameObject.Find("OpenPanel").GetComponent<ChangeToMission>().MissionWasChosen += Mission_Interact;
        GameObject.Find("Door_Mission").GetComponent<DoorMissionHandler>().doorWasOpendEvent += onDoorWasOpen;
        GameObject.Find("Screen_ZoomOut").GetComponent<ChangeToMission>().MissionWasChosen += Mission_Interact;

    }

    private void manageCameraPosition()
    {
        Vector3 currentPosition = gameObject.transform.position;

        if (m_CurrentWallIndex != m_PreviousWallIndex)
        {
            gameObject.transform.position = new Vector3((m_CurrentWallIndex - 1) * m_DistanceToMoveXOfCamera, currentPosition.y, currentPosition.z);
            m_PreviousWallIndex = m_CurrentWallIndex;
        }
    }

    private void manageLeftAndRightsBtnsActivation()
    {
        bool leftBtnShouldAppear = m_CurrentWallIndex > 1;
        bool rightBtnShouldApper = m_CurrentWallIndex >= 1 && m_CurrentWallIndex < sr_MostRightWallIndex;
        if (m_CurrentWallIndex == 2 && m_CanGoRight == false)
        {
            rightBtnShouldApper = false;
        }
        bool backBtnShouldAppear = m_CurrentWallIndex < 1;

        m_LeftBtn.SetActive(leftBtnShouldAppear);
        m_RightBtn.SetActive(rightBtnShouldApper);
        m_GoBackBtn.SetActive(backBtnShouldAppear);
    }

    public void OnClickRightChangeBackgroundBtn()
    {
        m_CurrentWallIndex++;
    }

    public void OnClickLeftChangeBackgroundBtn()
    {
        m_CurrentWallIndex--;
    }

    public void OnClickBackBtn()
    {
        m_CurrentWallIndex = m_WallBeforeMission;
    }

    public void Mission_Interact(int i_MissionWall)
    {
        Debug.Log("In mission_Interact");
        m_WallBeforeMission = m_CurrentWallIndex;
        while (m_CurrentWallIndex != i_MissionWall)
        {
            m_LeftBtn.GetComponent<Button>().onClick.Invoke();
        }
    }

    private void onDoorWasOpen()
    {
        m_CanGoRight = true;

    }
}
