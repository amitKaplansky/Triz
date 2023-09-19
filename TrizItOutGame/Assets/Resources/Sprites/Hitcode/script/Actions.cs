using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
namespace Hitcode_RoomEscape
{
    //[System.Serializable]
    public class Actions : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            //testgs = new List<GameObject>();
            //myactions = new List<Action>();
            //print(myactions.Count);
        }

        // Update is called once per frame
        void Update()
        {

        }

        //	[SerializeField]
        public List<Action> actionSteps;

        public int actionsId;







        public List<GameObject> testgs;



        //here deal with the play action broadcast
        public void playActionNow(int param)
        {


            int cIndex = param;

            Action taction = actionSteps[cIndex];
            //iterate all action type in this action
            if (taction.showHides.show_hide != null)
            {
                for (int ii = 0; ii < taction.showHides.show_hide.Count; ii++)
                {
                    GameObject tshowObj = taction.showHides.show_hide[ii].object2Show;
                    if (tshowObj != null) tshowObj.SetActive(true);
                    GameObject thideObj = taction.showHides.show_hide[ii].object2Hide;
                    if (thideObj != null) thideObj.SetActive(false);
                }



            }
            //move
            if (taction.transfroms != null && taction.transfroms._transfrom != null)
            {
                for (int ii = 0; ii < taction.transfroms._transfrom.Count; ii++)
                {
                    GameObject tobj = taction.transfroms._transfrom[ii].transformGameObject;
                    if (tobj != null)
                    {
                        //tween is not effective
                        if (taction.transfroms._transfrom[ii].tween <= 0)
                        {
                            tobj.transform.Translate(taction.transfroms._transfrom[ii].offsets);
                        }
                        else//tween if effective
                        {
                            GameData.Instance.locked = true;
                            tobj.transform.DOMove(tobj.transform.position + taction.transfroms._transfrom[ii].offsets, taction.transfroms._transfrom[ii].tween).SetEase(Ease.Linear).OnComplete(() => { GameData.Instance.locked = false; });
                        }
                    }
                }
            }


            //rotate
            if (taction.rotates != null && taction.rotates._transfrom != null)
            {
                for (int ii = 0; ii < taction.rotates._transfrom.Count; ii++)
                {
                    GameObject tobj = taction.rotates._transfrom[ii].transformGameObject;
                    if (tobj != null)
                    { //tween is not effective
                        if (taction.rotates._transfrom[ii].tween <= 0)
                        {
                            tobj.transform.localEulerAngles += (taction.rotates._transfrom[ii].offsets);
                        }
                        else
                        {
                            GameData.Instance.locked = true;
                            tobj.transform.DOLocalRotate(tobj.transform.localEulerAngles + taction.rotates._transfrom[ii].offsets, taction.rotates._transfrom[ii].tween).SetEase(Ease.Linear).OnComplete(() => { GameData.Instance.locked = false; });
                        }
                    }
                }
            }


            //pickup
            if (taction.pickups != null)
            {
                for (int ii = 0; ii < taction.pickups._pickups.Count; ii++)
                {
                    GameObject tobj = taction.pickups._pickups[ii].pickupTarget;
                    if (tobj != null)
                    {
                        if (!taction.pickups._pickups[ii].infinitive)
                        {
                            tobj.SetActive(false);
                        }
                        GameData.Instance.AddItemByName(tobj.name);
                        GameData.Instance.SaveGame();

                        GameData.Instance.rm.playGetItemTip(tobj.name);
                    }
                }
            }

            //playanim
            if (taction.playanims != null)
            {
                for (int ii = 0; ii < taction.playanims.playAnim.Count; ii++)
                {
                    GameObject tobj = taction.playanims.playAnim[ii].animGameObject;
                    string animName = taction.playanims.playAnim[ii].animName;
                    if (tobj != null)
                    {
                        Animator tanim = tobj.GetComponent<Animator>();
                        if (tanim != null)
                        {
                            tanim.SetTrigger(animName);
                        }
                    }
                }
            }


            //Switch Camera
            if (taction.switchCameras != null)
            {
                Debug.Log("switch camares !!!!" + taction.switchCameras.swtichCamera.Count);

                for (int i = 0; i < taction.switchCameras.swtichCamera.Count; i++)
                {
                    Camera targetCam = taction.switchCameras.swtichCamera[i].TargetCamera;
                    bool isAppend = taction.switchCameras.swtichCamera[i].append;
                    bool isRecord = taction.switchCameras.swtichCamera[i].record;

                    if (targetCam != null)
                    {
                        if (!isAppend)
                        {
                            Debug.Log("not append");

                            Camera tnewCam = targetCam;//targetCam.GetComponent<Camera>();
                            if (tnewCam != null)
                            {
                                Debug.Log(tnewCam +"camare count "+ GameData.Instance.cameraList.Count);

                                if (GameData.Instance.cameraList.Count > 0)
                                {
                                    Camera lastCam = GameData.Instance.cameraList[GameData.Instance.cameraList.Count - 1];
                                    Debug.Log("last camera name:" + lastCam.name +" this camera :"+ tnewCam.transform.name);

                                    if (lastCam != null && lastCam.name != tnewCam.transform.name)
                                    {
                                        Debug.Log("In if condition");

                                        if (tnewCam.name == "zoomQuiz")
                                        {
                                            GameObject submask = GameObject.Find("SubSceneMask");
                                            submask.GetComponent<Image>().enabled = false;

                                        }
                                        Debug.Log("The last cam is: " + lastCam.name);
                                        lastCam.enabled = false;//set the last camera disable
                                    }
                                    else
                                    {
                                        continue;//duplicated camera,ignore
                                    }
                                }

                                  tnewCam.enabled = true;

                                if (isRecord)
                                {
                                    Debug.Log("Is record");
                                    GameData.Instance.cameraList.Add(tnewCam);

                                    //enable previous button
                                    if (GameData.Instance.cameraList.Count > 1)
                                    {
                                        if(tnewCam.name == "zoomCamera" || tnewCam.name == "zoomQuiz")
                                          GameObject.Find("btnExitPreviousScene").GetComponent<Image>().enabled = true;

                                        if(tnewCam.name == "zoomQuiz")
                                          GameObject.Find("btnExitSubScene").GetComponent<Image>().enabled = false;

                                    }
                                }
                                else
                                {
                                    Debug.Log("ImWasHere");
                                    //disable previous button,clear cam list
                                    if (GameData.Instance.cameraList.Count > 1)
                                    {
                                          Debug.Log("cam now "+tnewCam.name);

                                        if (tnewCam.name == "zoomCamera" || tnewCam.name == "zoomQuiz")
                                            GameObject.Find("btnExitPreviousScene").GetComponent<Image>().enabled = false;

                                       
                                    }
                                    if (tnewCam.name != "appendCamera")
                                    {
                                        GameData.Instance.cameraList = new List<Camera>();
                                        GameData.Instance.cameraList.Add(tnewCam);
                                    }
                                }

                            }

                        }
                        else//append mode is quite different
                        {
                            Debug.Log("append" );


                            Camera tnewCam = targetCam.GetComponent<Camera>();
                            Debug.Log(tnewCam.name);

                            if (tnewCam != null)
                            {
                                tnewCam.enabled = true;

                                GameData.Instance.currentSubCam = tnewCam;
                                GameObject submask = GameObject.Find("SubSceneMask");
                                submask.GetComponent<Image>().enabled = true;

                               GameObject.Find("btnExitPreviousScene").GetComponent<Image>().enabled = false;
                               GameObject.Find("btnExitSubScene").GetComponent<Image>().enabled = true;

                                GameData.Instance.areaGame = true;

                               // GameManager.getInstance().playSfx("flip");
                                GameData.Instance.cameraList.Add(tnewCam);
                            }
                        }
                        
                        if (targetCam.name != "Main Camera")
                        {
                            disableComponnent();
                        }
                    }
                }

            }

            //Switch Scene
            if (taction.switchScenes != null)
            {
                for (int ii = 0; ii < taction.switchScenes.SwitchScene.Count; ii++)
                {
                    //record last camera;
                    string cName = SceneManager.GetActiveScene().name;

                    //not record sub secene cemera state
                    if (GameData.Instance.cameraList.Count == 1)
                    {
                        GameData.Instance.CameraDic[cName] = GameData.Instance.cameraList[0].transform.position;
                    }


                    string tsceneName = taction.switchScenes.SwitchScene[ii].sceneName;
                    Image tmask = GameObject.Find("Mask").GetComponent<Image>();
                    tmask.enabled = true;
                    //GameData.Instance.locked = true;
                    tmask.DOFade(1, .4f).OnComplete(() => {
                        SceneManager.LoadScene(tsceneName);
                        //GameData.Instance.locked = false;


                    });

                }
            }


            //show Text
            if (taction.playTexts != null && taction.playTexts.playTexts != null && taction.playTexts.playTexts.Count > 0)
            {
                for (int ii = 0; ii < taction.playTexts.playTexts.Count; ii++)
                {
                    string tPlayText;
                    string tlocalid = taction.playTexts.playTexts[ii].localId.Trim();
                    if (tlocalid != "")
                    {
                        tPlayText = Localization.Instance.GetString(tlocalid);
                    }
                    else
                    {
                        tPlayText = taction.playTexts.playTexts[ii].text;
                    }

                    string[] tPlayTexts = tPlayText.Split(";"[0]);
                    GameData.Instance.PlayTexts = new List<string>();
                    if (tPlayTexts.Length == 0)
                    {
                        GameData.Instance.PlayTexts.Add(tPlayText);
                    }
                    else
                    {
                        for (int kk = 0; kk < tPlayTexts.Length; kk++)
                        {
                            GameData.Instance.PlayTexts.Add(tPlayTexts[kk]);
                        }
                    }

                    //active modal text UI
                    GameData.Instance.gameUI.panelText.SetActive(true);

                    GameData.Instance.typeGap = taction.playTexts.playTexts[ii].typeWrite;
                    GameData.Instance.rm.playText();






                }
            }


            //play sound
            if (taction.playSounds != null)
            {
                for (int ii = 0; ii < taction.playSounds.playSounds.Count; ii++)
                {
                    string tsoundName = taction.playSounds.playSounds[ii].soundName;
                    bool isMute = taction.playSounds.playSounds[ii].mute;
                    bool isBG = taction.playSounds.playSounds[ii].isBG;
                    bool isLoop = taction.playSounds.playSounds[ii].isLoop;
                    if (tsoundName.Trim() != "")
                    {
                        if (isMute)
                        {
                            if (isBG)
                            {
                                if (tsoundName.Trim() != "")
                                {
                                    GameManager.getInstance().stopBGMusic();

                                }
                                else
                                {
                                    GameManager.getInstance().stopMusic(tsoundName);
                                }

                            }
                            else
                            {
                                if (tsoundName.Trim() != "")
                                {
                                    GameManager.getInstance().stopAllSFX();
                                }
                                else
                                {
                                    GameManager.getInstance().stopSfx(tsoundName);
                                }
                            }



                        }
                        else
                        {
                            if (isBG)
                            {
                                //GameManager.getInstance().stopBGMusic();
                                GameManager.getInstance().playMusic(tsoundName);
                            }
                            else
                            {

                               // GameManager.getInstance().playSfx(tsoundName, "", isLoop);

                            }
                        }
                    }


                }
            }

            //state
            if (taction.setStates != null)
            {
                for (int ii = 0; ii < taction.setStates.stateList.Count; ii++)
                {
                    string tStateName = taction.setStates.stateList[ii].stateName;
                    int tStateValue = taction.setStates.stateList[ii].stateValue;

                    //if (PlayerPrefs.HasKey(tStateName))//only deal with exist values(prevent data polution)
                    //{
                    PlayerPrefs.SetInt(tStateName + 0, tStateValue);
                    //}
                }
            }


            //send message
            if (taction.sendMsgs != null)
            {
                for (int ii = 0; ii < taction.sendMsgs.sendMsgList.Count; ii++)
                {

                    GameObject tg = taction.sendMsgs.sendMsgList[ii].msgTarget;
                    if (tg != null)
                    {
                        string tfuncName = taction.sendMsgs.sendMsgList[ii].MsgName;
                        string tparam = taction.sendMsgs.sendMsgList[ii].param;
                        tg.SendMessage(tfuncName, tparam, SendMessageOptions.DontRequireReceiver);
                    }

                }
            }


            //unlock delay
            if (taction.lockDelays != null && taction.lockDelays.lockDelays != null)
            {
                for (int ii = 0; ii < taction.lockDelays.lockDelays.Count; ii++)
                {
                    GameObject tTarget = taction.lockDelays.lockDelays[ii].target;
                    float tdelay = taction.lockDelays.lockDelays[ii].delayTime;
                    int tActionId = taction.lockDelays.lockDelays[ii].actionId;
                    if (tActionId != -1)
                    {
                        GameData.Instance.rm.lockDelay(tdelay, tTarget, tActionId);
                    }

                }
            }


            //Jounal
            if (taction.journals != null && taction.journals.journals != null)
            {
                for (int ii = 0; ii < taction.journals.journals.Count; ii++)
                {
                    string tTaked_param = taction.journals.journals[ii].taked_param;
                    
                    GameData.Instance.gameUI.panelReadJournal.GetComponent<PanelReadJournal>().showPanel(gameObject, tTaked_param);
                    
                    
                }
            }
        }

        private void disableComponnent()
        {

            GameObject stoneObject = GameObject.Find("stone");

            if (stoneObject != null)
            {
                stoneObject.GetComponent<PolygonCollider2D>().enabled = false;
            }
            if (GameObject.Find("passwordGame") != null)
            {
                GameObject.Find("passwordGame").GetComponent<BoxCollider>().enabled = false;
            }
            if (GameObject.Find("Key") != null)
            {
                GameObject.Find("Key").GetComponent<PolygonCollider2D>().enabled = false;
            }
            if (GameObject.Find("breakMeBox") != null)
            {
                GameObject.Find("breakMeBox").GetComponent<BoxCollider2D>().enabled = false;
            }
            if (GameObject.Find("Door") != null)
            {
                GameObject.Find("Door").GetComponent<BoxCollider2D>().enabled = false;
            }
            if (GameObject.Find("newspaper") != null)
            {
                GameObject.Find("newspaper").GetComponent<PolygonCollider2D>().enabled = false;
            }
            if (GameObject.Find("books") != null)
            {
                GameObject.Find("books").GetComponent<PolygonCollider2D>().enabled = false;
            }
            if (GameObject.Find("zoomToPaint") != null)
            {
                GameObject.Find("zoomToPaint").GetComponent<BoxCollider2D>().enabled = false;
            }
        }

    }






    }

    public enum actionTypes
    {
        showhide,
        Move,
        Rotate,
        pickup,
        playAnim,
        swtichCamera,
        switchScene,
        playText,
        playSound,
        setState,
        sendMessage,
        lockDelay,
        journal,
        comment

    }

    public enum valueOp
    {
        add,//if you wanna minus add -1
        equal
    }


//===============================================
namespace Hitcode_RoomEscape
{
    [System.Serializable]
    public class Action
    {
        int actionId;
        public actionTypes cType;//for temp use
        public Show_hides showHides;
        public Transforms transfroms;
        public _Rotates rotates;
        public PickUps pickups;
        public PlayAnims playanims;
        public SwitchCameras switchCameras;
        public SwitchScenes switchScenes;
        public PlayTexts playTexts;
        public PlaySounds playSounds;

        public SetStates setStates;
        public SendMsgs sendMsgs;
        public Comments comments;
        public LockDelays lockDelays;
        public Journals journals;
    }
}

    [System.Serializable]
    public class Show_hides
    {
        public List<Show_hide> show_hide;
    }
    [System.Serializable]
    public class Show_hide
    {
        public GameObject object2Show;
        public bool ishowSelf;
        public GameObject object2Hide;
        public bool isHideSelf;
    }


    [System.Serializable]
    public class Transforms
    {
        public List<_Transform> _transfrom;
    }
    [System.Serializable]
    public class _Transform
    {
        public bool isSelf;
        public GameObject transformGameObject;
        public Vector3 offsets;
        public float tween;
    }


    [System.Serializable]
    public class _Rotates
    {
        public List<_Rotate> _transfrom;
    }
    [System.Serializable]
    public class _Rotate
    {
        public bool isSelf;
        public GameObject transformGameObject;
        public Vector3 offsets;
        public float tween;
    }


    [System.Serializable]
    public class PickUps
    {
        public List<PickUp> _pickups;
    }
    [System.Serializable]
    public class PickUp
    {
        public bool isSelf;
        public GameObject pickupTarget;
        public bool infinitive = false;
    }

    [System.Serializable]
    public class PlayAnims
    {
        public List<PlayAnim> playAnim;
    }
    [System.Serializable]
    public class PlayAnim
    {
        public bool isSelf;
        public GameObject animGameObject;
        public string animName;
    }

    [System.Serializable]
    public class SwitchCameras
    {
        public List<SwitchCamera> swtichCamera;
    }
    [System.Serializable]
    public class SwitchCamera
    {
        public Camera TargetCamera;
        public bool append;
        public bool record = true;
}


    [System.Serializable]
    public class SwitchScenes
    {
        public List<SwitchScene> SwitchScene;
    }
    [System.Serializable]
    public class SwitchScene
    {
        public string sceneName;
    }


    [System.Serializable]
    public class PlayTexts
    {
        public List<PlayText> playTexts;
    }
    [System.Serializable]
    public class PlayText
    {
        public string text;
        public float typeWrite;
        public string localId;
    }



    [System.Serializable]
    public class PlaySounds
    {
        public List<PlaySound> playSounds;
    }
    [System.Serializable]
    public class PlaySound
    {
        public string soundName;
        public bool isBG;
        public bool isLoop;
        public bool mute;
    }


    [System.Serializable]
    public class SetStates
    {
        public List<_State> stateList;

    }

    [System.Serializable]
    public class _State {
        public string stateName;
        public valueOp stateOp;
        public int stateValue;
    }



    [System.Serializable]
    public class SendMsgs
    {
        public List<SendMsg> sendMsgList;

    }

    [System.Serializable]
    public class SendMsg
    {
        public bool isSelf;
        public GameObject msgTarget;
        public string MsgName;
        public string param;
    }

    [System.Serializable]
    public class Comments
    {
        public List<string> comment;
    }

    [System.Serializable]
    public class LockDelays
    {
        public List<LockDelay> lockDelays;
    }
    [System.Serializable]
    public class LockDelay {
        public float delayTime;
        public GameObject target;
        public bool isSelf;
        public int actionId = -1;
    }
    [System.Serializable]
    public class Journals
    {
        public List<Journal> journals;
    }
    [System.Serializable]
    public class Journal
    {
        public string journalname;
        public string taked_param;
    }


