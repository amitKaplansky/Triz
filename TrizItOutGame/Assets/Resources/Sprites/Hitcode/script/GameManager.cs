using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

//using AppAdvisory.Ads;
//using Umeng;
namespace Hitcode_RoomEscape
{
    public class GameManager
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }





        public static GameManager instance;
        public static GameManager getInstance()
        {
            if (instance == null)
            {
                instance = new GameManager();
                instance.init();
            }
            return instance;
        }



        GameObject music;//sound control instance
                         /// <summary>
                         /// Plaies the music.
                         /// </summary>
                         /// <param name="str">String.</param>
                         /// <param name="isforce">If set to <c>true</c> isforce.</param>
        public void playMusic(string str, bool isforce = false)
        {

            //do not play the same music againDebug.Log (musicName+"__"+str);
            if (!isforce)
            {
                if (bgMusic != null && musicName == str)
                {
                    return;
                }
            }


            if (!music)
                return;


            AudioSource tmusic = null;

            AudioClip clip = (AudioClip)Resources.Load("sound\\" + str, typeof(AudioClip));

            //Debug.Log(clip);
            if (GameData.getInstance().isSoundOn == 0)
            {
                if (bgMusic)
                {

                    if (isforce)
                    {
                       
                    }//force

                    DOTween.To(() => bgMusic.volume,
           (v) => bgMusic.volume = v, 0, 1).OnComplete(() =>
           {
               bgMusic.Stop();
               musicName = "";

               //continue
              
               tmusic = music.GetComponent<musicScript>().PlayAudioClip(clip, true);

               if (str.Substring(0, 2) == "bg")
               {
                   tmusic.volume = 0;
                   DOTween.To(() => bgMusic.volume,
(v) => bgMusic.volume = v, 1, 1).OnComplete(() =>
{ });
                   musicName = str;
                   bgMusic = tmusic;

               }
           });
                }
                else
                {
                    //directly continue
                    tmusic = music.GetComponent<musicScript>().PlayAudioClip(clip, true);
                    
    if (str.Substring(0, 2) == "bg")
                    {
                        tmusic.volume = 0;
                        DOTween.To(() => bgMusic.volume,
    (v) => bgMusic.volume = v, 1, 1).OnComplete(() =>
    { });
                        musicName = str;
                        bgMusic = tmusic;

                    }
                }


            }

        }






        List<AudioSource> currentSFX = new List<AudioSource>();
        Dictionary<string, int> sfxdic = new Dictionary<string, int>();

        AudioSource cWalk = new AudioSource(); //sometime for continous sound like walk steps.
        /// <summary>
        /// Plaies the sfx.
        /// </summary>
        /// <returns>The sfx.</returns>
        /// <param name="str">String.</param>
        public AudioSource playSfx(string str, string idNumber = "", bool isloop = false)
        {
            AudioSource sfxSound = null;

            if (!music)
                return null;
           

            AudioClip clip = (AudioClip)Resources.Load("sound\\" + str, typeof(AudioClip));
            if (GameData.getInstance().isSfxOn == 0)
            {
                Debug.Log($"How is audio clip {music.GetComponent<musicScript>().PlayAudioClip(clip)}");
                sfxSound = music.GetComponent<musicScript>().PlayAudioClip(clip);

                sfxSound.loop = isloop;
                if (sfxSound != null)
                {
                    if (sfxdic.ContainsKey(str + idNumber) == false || sfxdic[str + idNumber] != 1)
                    {
                        currentSFX.Add(sfxSound);

                        sfxdic[str + idNumber] = 1;

                    }
                }
            }

            return sfxSound;


        }


        AudioSource bgMusic = new AudioSource();//record background music
        public string musicName = "";
        /// <summary>
        /// Stops the background music.
        /// </summary>
        public void stopBGMusic()
        {
            if (bgMusic)
            {
                if (bgMusic)
                {
                    DOTween.To(() => bgMusic.volume,
            (v) => bgMusic.volume = v, 0, 1).OnComplete(() => {
                bgMusic.Stop();
                musicName = "";
            }).SetEase(Ease.OutSine);

                }

            }
        }
        /// <summary>
        /// Stops all sound effect.
        /// </summary>
        public void stopAllSFX()
        {
            foreach (AudioSource taudio in currentSFX)
            {
                if (taudio != null)
                {
                    taudio.Stop();

                }
            }
            currentSFX.Clear();
            sfxdic.Clear();
        }
        /// <summary>
        /// Stops a certain sound effect.
        /// </summary>
        /// <param name="sfxName"></param>
        public void stopSfx(string sfxName, string id = "")
        {
            foreach (AudioSource taudio in currentSFX)
            {
                if (taudio != null && taudio.clip.name == sfxName)
                {
                    taudio.Stop();
                    sfxdic.Remove(sfxName + id);


           
                    AudioSource[] as1 = music.GetComponentsInChildren<AudioSource>();
                    foreach (AudioSource tas in as1)
                    {
                        if (tas && tas.clip)
                        {
                            string clipname = (tas.clip.name);
                            if (clipname == sfxName)
                            {
                                GameObject.Destroy(tas);
                                break;
                            }
                        }
                    }


                    break;
                }
            }



        }


        public void stopSfx(AudioSource csfx, string sfxName = "")
        {
            if (csfx != null)
            {
                csfx.Stop();
                sfxdic.Remove(sfxName);


            }

        }


        /// <summary>
        /// detect a certain sound whether is playing
        /// </summary>
        /// <returns><c>true</c>, if playing sfx was ised, <c>false</c> otherwise.</returns>
        /// <param name="str">String.</param>
        public bool isPlayingSfx(string str)
        {
            bool isPlaying = false;
            if (sfxdic.ContainsKey(str) && sfxdic[str] == 1)
            {
                isPlaying = true;
            }
            return isPlaying;

        }

        /// <summary>
        /// Stops the music.
        /// </summary>
        /// <param name="musicName">Music name.</param>
        public void stopMusic(string musicName = "")
        {
            if (music)
            {
                AudioSource[] as1 = music.GetComponentsInChildren<AudioSource>();
                foreach (AudioSource tas in as1)
                {
                    if (musicName == "")
                    {
                        tas.Stop();
                        break;
                    }
                    else
                    {
                        if (tas && tas.clip)
                        {
                            string clipname = (tas.clip.name);
                            if (clipname == musicName)
                            {
                                tas.Stop();


                                musicName = "";
                                if (sfxdic.ContainsKey(clipname))
                                {
                                    sfxdic[clipname] = 0;
                                    if (clipname == "walk")
                                    {
                                        if (cWalk != null)
                                        {
                                            cWalk.Stop();
                                            cWalk = null;
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// switch the sound.
        /// </summary>
        public void toggleSound()
        {


            int soundState = GameData.getInstance().isSoundOn;




        }



        /// <summary>
        /// Submits the game center.
        /// </summary>
        public void submitGameCenter()
        {
            if (!isAuthored)
            {
                //Debug.Log("authenticating...");
                //initGameCenter();
            }
            else
            {
                //Debug.Log("submitting score...");
                //			int totalScore = getAllScore();
                int tbestScore = GameData.getInstance().bestscore;
                ReportScore(Const.LEADER_BOARD_ID, tbestScore);

            }

        }


        public void init()
        {
            music = GameObject.Find("music") as GameObject;
            //get data
            initAds();
            initRate();
            //Localization.Instance.SetLanguage(GameData.Instance.GetSystemLaguage());
            
            int allScore = 0;


            GameData.getInstance().bestscore = allScore;
            GameData.getInstance().isSoundOn = (int)PlayerPrefs.GetInt("sound", 0);
            GameData.getInstance().isSfxOn = (int)PlayerPrefs.GetInt("sfx", 0);
            //Debug.Log("soundstate:" + GameData.getInstance().isSoundOn + "sfxstate:" + GameData.getInstance().isSfxOn);

          

            initGameCenter();
            
            //initStore();

        }
        public bool noToggleSound = false;
        /// <summary>
        /// Sets the state of the toggle buttons.
        /// </summary>
        public void setToggleState()
        {
            //this section will trigger the click itself.So force not play the sound.(if notogglesound is true)
            noToggleSound = true;
            GameObject checkMusicG = GameObject.Find("toggleMusic");
            if (checkMusicG)
            {

                noToggleSound = false;

            }
        }


        //=================================GameCenter======================================
        public void initGameCenter()
        {
            Social.localUser.Authenticate(HandleAuthenticated);
        }


        private bool isAuthored = false;
        private void HandleAuthenticated(bool success)
        {
            //        Debug.Log("*** HandleAuthenticated: success = " + success);
            if (success)
            {
                Social.localUser.LoadFriends(HandleFriendsLoaded);
                Social.LoadAchievements(HandleAchievementsLoaded);
                Social.LoadAchievementDescriptions(HandleAchievementDescriptionsLoaded);


                isAuthored = true;

                submitGameCenter();

            }



        }

        private void HandleFriendsLoaded(bool success)
        {
            //        Debug.Log("*** HandleFriendsLoaded: success = " + success);
            foreach (IUserProfile friend in Social.localUser.friends)
            {
                //            Debug.Log("*   friend = " + friend.ToString());
            }
        }

        private void HandleAchievementsLoaded(IAchievement[] achievements)
        {
            //        Debug.Log("*** HandleAchievementsLoaded");
            foreach (IAchievement achievement in achievements)
            {
                //            Debug.Log("*   achievement = " + achievement.ToString());
            }
        }

        private void HandleAchievementDescriptionsLoaded(IAchievementDescription[] achievementDescriptions)
        {
            //        Debug.Log("*** HandleAchievementDescriptionsLoaded");
            foreach (IAchievementDescription achievementDescription in achievementDescriptions)
            {
                //            Debug.Log("*   achievementDescription = " + achievementDescription.ToString());
            }
        }

        // achievements

        public void ReportProgress(string achievementId, double progress)
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportProgress(achievementId, progress, HandleProgressReported);
            }
        }

        private void HandleProgressReported(bool success)
        {
            //        Debug.Log("*** HandleProgressReported: success = " + success);
        }

        public void ShowAchievements()
        {
            if (Social.localUser.authenticated)
            {
                Social.ShowAchievementsUI();
            }
        }

        // leaderboard

        public void ReportScore(string leaderboardId, long score)
        {
            //Debug.Log("submitting score to GC...");
#if UNITY_IOS
            if (Social.localUser.authenticated)
            {
                Social.ReportScore(score, leaderboardId, HandleScoreReported);
            }
#endif
        }
            

        public void HandleScoreReported(bool success)
        {
            //        Debug.Log("*** HandleScoreReported: success = " + success);
        }

        public void ShowLeaderboard()
        {
            Debug.Log("showLeaderboard");
            if (Social.localUser.authenticated)
            {
                Social.ShowLeaderboardUI();
            }
        }

        //=============================================GameCenter=========================

        public void buyFullVersion()
        {
            //		UnityPluginForWindowsPhone.Class1.BuyFullVersion(Const.wp8ID);
        }


        //rate

        void initRate()
        {






        }
        public void rate()
        {

            Application.OpenURL("itms-apps://itunes.apple.com/app/" + "id" + Const.AppId);
        }


        //ads
        void initAds()
        {


            //		hideBanner (true);
            //Chartboost.didCacheInterstitial += cbInterestitialReady;
            //Chartboost.didFailToLoadRewardedVideo += didFailToLoadRewardedVideo;
            //Chartboost.didDismissRewardedVideo += didDismissRewardedVideo;
            //Chartboost.didCloseRewardedVideo += didCloseRewardedVideo;
            //Chartboost.didClickRewardedVideo += didClickRewardedVideo;
            //Chartboost.didCacheRewardedVideo += didCacheRewardedVideo;
            //Chartboost.shouldDisplayRewardedVideo += shouldDisplayRewardedVideo;
            //Chartboost.didCompleteRewardedVideo += didCompleteRewardedVideo;
            //Chartboost.didDisplayRewardedVideo += didDisplayRewardedVideo;



            //				requestVideoAds ();
            //show on startup;


            //GameData.isAds = PlayerPrefs.GetInt("noAds", 0) == 0 ? true : false;


            //CacheInterestial();

            if (isfirst)
            {
                //ShowInterestitial();
                isfirst = false;
            }

            showBanner();
        }

        public void hideBanner(bool isHidden)
        {

        }

        public void showBanner()
        {
            //if (GameData.isAds)
            //{

            //}
        }


        //public void CacheInterestial(){
        //		if (GameData.isAds) {
        //				Chartboost.cacheInterstitial (CBLocation.Default);

        //		}
        //}

        bool isfirst = true;
        public void ShowInterestitial()
        {

            //if (GameData.isAds)
            //{

            //    //if (AdsManager.instance.IsReadyInterstitial()){
            //    //    AdsManager.instance.ShowInterstitial();
            //    //}



            //}
        }



        public void ShowRewardedAd()//for unity ads only;
        {
            //				#if UNITY_IPHONE || UNITY_ANDROID
            //				if (Advertisement.IsReady(""))
            //				{
            //						var options = new ShowOptions { resultCallback = HandleShowResult };
            //						Advertisement.Show("", options);
            //				}
            //				#endif
        }

        //handle unity ads callback
#if UNITY_IPHONE || UNITY_ANDROID
        //		private void HandleShowResult(ShowResult result)
        //		{
        //				switch (result)
        //				{
        //				case ShowResult.Finished:
        //						Debug.Log("The ad was successfully shown.");
        //						//
        //						// YOUR CODE TO REWARD THE GAMER
        //						// Give coins etc.
        //						makeReward();
        //						break;
        //				case ShowResult.Skipped:
        //						Debug.Log("The ad was skipped before reaching the end.");
        //						break;
        //				case ShowResult.Failed:
        //						Debug.LogError("The ad failed to be shown.");
        //						break;
        //				}
        //		}
#endif
        //	public void CacheVideo(){
        //		Chartboost.cacheRewardedVideo(CBLocation.Default);
        //	}

        //chartboost
        //void cbInterestitialReady(CBLocation location){
        //		if (isfirst) {
        //				Chartboost.showInterstitial (CBLocation.Default);
        //				isfirst = false;
        //		}
        //}	

        //void didFailToLoadRewardedVideo(CBLocation location, CBImpressionError error) {
        //		Debug.Log(string.Format("didFailToLoadRewardedVideo: {0} at location {1}", error, location));
        //}

        //void didDismissRewardedVideo(CBLocation location) {
        //		Debug.Log("didDismissRewardedVideo: " + location);
        //}

        //void didCloseRewardedVideo(CBLocation location) {
        //		Debug.Log("didCloseRewardedVideo: " + location);
        //}

        //void didClickRewardedVideo(CBLocation location) {
        //		Debug.Log("didClickRewardedVideo: " + location);
        //}
        //public bool isCachedVideo = false;
        //void didCacheRewardedVideo(CBLocation location) {
        //		Debug.Log("didCacheRewardedVideo: " + location);
        //		isCachedVideo = true;
        //}

        //bool shouldDisplayRewardedVideo(CBLocation location) {
        //		Debug.Log("shouldDisplayRewardedVideo: " + location);
        //		return true;
        //}

        //void didCompleteRewardedVideo(CBLocation location, int reward) {
        //		Debug.Log(string.Format("didCompleteRewardedVideo: reward {0} at location {1}", reward, location));


        //		makeReward ();

        //}

        //void didDisplayRewardedVideo(CBLocation location){
        //		Debug.Log("didDisplayRewardedVideo: " + location);
        //}


        //void makeReward()
        //{
        //    //add 10 coins;
        //    GameData.getInstance().coin += 1000;
        //    PlayerPrefs.SetInt("coin", GameData.getInstance().coin);

        //    GameObject topBar = GameObject.Find("PanelTopBar");
        //    if (topBar != null)
        //    {
        //        topBar.SendMessage("refreshView");
        //    }
        //    GameObject txtcoinNum = GameObject.Find("txtCoinNum");
        //    if (txtcoinNum != null)
        //    {
        //        //						txtcoinNum.GetComponent<Text>().text = GameData.getInstance().coin.ToString();
        //    }
        //    PlayerPrefs.Save();


        //}


        //=============================================GameCenter=========================


        //in app
        //		public const string NON_CONSUMABLE0 = "com.xxx.unlockall";//only use this for this version
        //public const string CONSUMABLE0 = "hwd120Coin";
        //public const string CONSUMABLE1 = "hwd300Coins";
        //public const string CONSUMABLE2 = "hwd500Coins";
        //public const string CONSUMABLE3 = "td15612.coin4";//not used

        //public static Purchaser purchaser;
        //void initStore()
        //{

        //    GameObject music = GameObject.Find("music");
        //    if (music != null)
        //    {
        //        purchaser = music.GetComponent<Purchaser>();
        //    }
        //}


        //only for google store if have one.Otherwise just ignore.
        public const string publishKey = "";
        public bool test = true;//set it to false when you publish to test for real.
                                /// <summary>
                                /// Buy item
                                /// </summary>
                                /// <param name="index">Index.</param>
        //public void buy(int index)
        //{
        //    if (test)
        //    {
        //        purchansedCallback("pack" + index);
        //    }
        //    else
        //    {
        //        switch (index)
        //        {
        //            case 0:
        //                purchaser.BuyConsumable("pack0");
        //                break;
        //            case 1:
        //                purchaser.BuyConsumable("pack1");
        //                break;
        //            case 2:
        //                purchaser.BuyConsumable("pack2");
        //                break;
        //            case 3:
        //                purchaser.BuyConsumable("pack3");
        //                break;



        //        }
        //    }
        //}

        //public void restore()
        //{
        //    purchaser.RestorePurchases();
        //}

        /// <summary>
        /// This will be called when a purchase completes.
        /// </summary>
        //public void purchansedCallback(string id)
        //{

        //    bool buyenough = false;
        //    switch (id)
        //    {
        //        case "pack0":
        //            buyenough = true;
        //            GameData.getInstance().coin += 120;
        //            break;
        //        case "pack1":
        //            buyenough = true;
        //            GameData.getInstance().coin += 300;
        //            break;
        //        case "pack2":
        //            buyenough = true;
        //            GameData.getInstance().coin += 500;
        //            break;
        //        case "pack3":
        //            //buyenough = true;
        //            GameData.getInstance().coin += 20;
        //            break;
        //    }

        //    PlayerPrefs.SetInt("coin", GameData.getInstance().coin);
        //    GameObject txtCoin = GameObject.Find("txtCoin");
        //    if (txtCoin != null)
        //    {
        //        txtCoin.GetComponent<Text>().text = GameData.getInstance().coin.ToString();
        //    }
        //    if (buyenough)
        //    {
        //        GameData.isAds = false;
        //        PlayerPrefs.SetInt("noAds", 1);
        //    }


        //}

        //public string getPrice(int index)
        //{
        //    if (purchaser == null) return null;
        //    string[] ids = { CONSUMABLE0, CONSUMABLE1, CONSUMABLE2, CONSUMABLE3 };

        //    string tprice = purchaser.getPrice(ids[index]);
        //    //Debug.Log(tprice);
        //    return tprice;
        //}



    }
}