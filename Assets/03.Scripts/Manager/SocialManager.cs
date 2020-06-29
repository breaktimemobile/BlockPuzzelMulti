using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

using System;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using System.Collections;
#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class SocialManager : MonoBehaviour
{

    public static SocialManager Instance;

    public bool isLogin = false;

    string save_data;

    public string _IDtoken = null;

    //인증코드 받기
    public string _authCode = null;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        //구글 서비스 활성화

#if UNITY_ANDROID

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

#else

        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
 
#endif

        VersionCheck();

    }

   
    public void DoAutoLogin()
    {
        Debug.Log("자동 로그인 " + PlayerPrefs.GetInt("Login", 0));
        //구글 로그인이 되어있지 않다면
        if (PlayerPrefs.GetInt("Login", 0).Equals(1))
        {
            if (!Social.localUser.authenticated)
            {

                Social.localUser.Authenticate((authenticateCallBck));
            }
        }

    }


//    public void Btn_Login()
//    {
//        //로그인

//        if (!Social.localUser.authenticated)
//        {

//            Debug.Log("로그인 시작");
//            Social.localUser.Authenticate((authenticateCallBck));

//        }

//    }

//    /// <summary>
//    /// 로그아웃
//    /// </summary>
//    public void Btn_Logout()
//    {
//#if UNITY_ANDROID
//        ((PlayGamesPlatform)Social.Active).SignOut();
//        isLogin = false;
//        PlayerPrefs.SetInt("Login", 0);

//        UIManager.Instance.PopPopup();
//#endif
//    }


    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            PlayerPrefs.SetInt("Login", Social.localUser.authenticated ? 1 : 0);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Login", Social.localUser.authenticated ? 1 : 0);
    }

    public void ShowSaveSelectUI()
    {
#if UNITY_ANDROID

        if (!Social.localUser.authenticated)
        {
            UIManager.Instance.Set_Google_Txt();
            UIManager.Instance.PushPopup(UIManager.Instance.GooglePopup);
        }
        else
        {

            UIManager.Instance.Set_Google_Txt();

            uint maxNumToDisplay = 5;
            bool allowCreateNew = true;
            bool allowDelete = true;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ShowSelectSavedGameUI("Select saved game",
                maxNumToDisplay,
                allowCreateNew,
                allowDelete,
                OnSavedGameSelected);
        }
#else


#endif
    }

    public void ShowLoadSelectUI()
    {

#if UNITY_ANDROID

        if (!Social.localUser.authenticated)
        {
            UIManager.Instance.Set_Google_Txt();
            UIManager.Instance.PushPopup(UIManager.Instance.GooglePopup);
        }
        else
        {

            UIManager.Instance.Set_Google_Txt();

            uint maxNumToDisplay = 5;
            bool allowCreateNew = false;
            bool allowDelete = false;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ShowSelectSavedGameUI("Select saved game",
                maxNumToDisplay,
                allowCreateNew,
                allowDelete,
                OnLoadGameSelected);
        }

#else


#endif
    }

    public void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {

        switch (status)
        {
            case SelectUIStatus.SavedGameSelected:

                Player_Data_Save();

                break;
            case SelectUIStatus.UserClosedUI:
                break;
            case SelectUIStatus.InternalError:
                break;
            case SelectUIStatus.TimeoutError:
                break;
            case SelectUIStatus.AuthenticationError:
                break;
            case SelectUIStatus.BadInputError:
                break;
            default:
                break;
        }
    }

    public void OnLoadGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {

        switch (status)
        {
            case SelectUIStatus.SavedGameSelected:

                //open the data.
                Player_Data_Load();
                break;
            case SelectUIStatus.UserClosedUI:
                break;
            case SelectUIStatus.InternalError:
                break;
            case SelectUIStatus.TimeoutError:
                break;
            case SelectUIStatus.AuthenticationError:
                break;
            case SelectUIStatus.BadInputError:
                break;

            default:
                break;
        }
    }

    void authenticateCallBck(bool sucess)
    {
        if (sucess)
        {
            isLogin = true;
            PlayerPrefs.SetInt("Login", 1);
            Debug.Log("로그인 성공 " + PlayerPrefs.GetInt("Login", 0));

            UIManager.Instance.Set_Google_Txt();

        }
        else
        {
            isLogin = false;
            PlayerPrefs.SetInt("Login", 0);
            Debug.Log("로그인 실패 " + PlayerPrefs.GetInt("Login", 0));

            UIManager.Instance.Set_Google_Txt();
        }
    }
    
#region Google Cloud Save

    public void Player_Data_Save(bool Popup = false)
    {

        string jsonStr = JsonUtility.ToJson(DataManager.Instance.state_Player);
        string aes = AESCrypto.instance.AESEncrypt128(jsonStr);

        StartCoroutine(Save(aes));
    }

    IEnumerator Save(string _data)
    {
#if UNITY_EDITOR

        UIManager.Instance.Start_TxtStat(true);
        UIManager.Instance.End_TxtStat(true);

#endif
        while (!isLogin)
        {
            CloudOnceManager.Instance.Login();
            yield return new WaitForSeconds(2f);
        }

        UIManager.Instance.Start_TxtStat(true);

        string id = Social.localUser.id;
        string filename = string.Format("{0}Bolck", id);
        save_data = _data;

        OpenSaveGame(filename, true);
    }

    void OpenSaveGame(string _fileName, bool _saved)
    {
#if UNITY_ANDROID

        ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;

        //요청
        if (_saved)
        {

            //save
            savedGame.OpenWithAutomaticConflictResolution(_fileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGamePendedTOsave);
        }
        else
        {
            //load
            savedGame.OpenWithAutomaticConflictResolution(_fileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGamePendedTOLoad);
        }
#endif
    }

    public void OnSavedGamePendedTOsave(SavedGameRequestStatus _states, ISavedGameMetadata _data)
    {

        if (_states == SavedGameRequestStatus.Success)
        {

            byte[] b = Encoding.UTF8.GetBytes(save_data);
            //ToastManager.instance.ShowToast(b);

            SaveGame(_data, b, DateTime.Now.TimeOfDay);

        }
        else
        {
            Debug.Log("Save Fail");

        }
    }

    public void SaveGame(ISavedGameMetadata _data, byte[] _byte, TimeSpan _playTime)
    {
#if UNITY_ANDROID

        ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;
        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

        builder = builder.WithUpdatedPlayedTime(_playTime).WithUpdatedDescription("Saved at " + DateTime.Now);

        SavedGameMetadataUpdate updateData = builder.Build();
        savedGame.CommitUpdate(_data, updateData, _byte, OnSacedGameWritten);
#endif

    }

    //세이브 저장 여부
    public void OnSacedGameWritten(SavedGameRequestStatus _state, ISavedGameMetadata _data)
    {
        if (_state == SavedGameRequestStatus.Success)
        {
            Debug.Log("save Complete");
            UIManager.Instance.End_TxtStat(true);

        }
        else
        {
            Debug.Log("Save Fail");

        }
    }

#endregion

#region Google Cloud Load

#region old
    public void Player_Data_Load()
    {

        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
#if UNITY_EDITOR
        UIManager.Instance.Start_TxtStat(false);
        yield return new WaitForSeconds(2f);
        UIManager.Instance.End_TxtStat(false);

#else
            while (!isLogin)
            {
                CloudOnceManager.Instance.Login();
                yield return new WaitForSeconds(2f);
            }

            UIManager.Instance.Start_TxtStat(false);

            string id = Social.localUser.id;
            string filename = string.Format("{0}Bolck", id);

            OpenSaveGame(filename, false);
#endif
    }

    public void OnSavedGamePendedTOLoad(SavedGameRequestStatus _states, ISavedGameMetadata _data)
    {
        if (_states == SavedGameRequestStatus.Success)
        {
            LoadGameData(_data);
        }
        else
        {
            Debug.Log("Load Fail");

        }
    }

    //세이브 저장 여부
    public void OnSacedGameRead(SavedGameRequestStatus _state, byte[] _byte)
    {
        if (_state == SavedGameRequestStatus.Success)
        {
            Debug.Log("save Complete");

            UIManager.Instance.End_TxtStat(false);

            string data = Encoding.Default.GetString(_byte);
            Player_Data_Load(data);

        }
        else
        {
            Debug.Log("load Fail");

        }
    }

    public void LoadGameData(ISavedGameMetadata _data)
    {
#if UNITY_ANDROID

        ISavedGameClient savedGame = PlayGamesPlatform.Instance.SavedGame;

        savedGame.ReadBinaryData(_data, OnSacedGameRead);
#endif

    }

#endregion

    public void Player_Data_Load(string str)
    {
        string aes = AESCrypto.instance.AESDecrypt128(str);

        var data = JsonUtility.FromJson<State_Player>(aes);

        DataManager.Instance.state_Player = data;

        DataManager.Instance.Save_Player_Data();

        UIManager.Instance.SetUi();

        UIManager.Instance.Check_Daily();

    }

#endregion


    private const string title_KR = "블록 퍼즐 보석 멀티플레이";
    private const string title_EN = "Block Puzzle Jewel Multiplay";
    private const string title_JP = "ブロックパズルジュエルマルチプレイ";

    private const string body = "https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlegame.multiplayer.tetris";

    public void Share()
    {

        string title = "";
        switch (Application.systemLanguage)
        {

            case SystemLanguage.Japanese:
                title = title_JP;
                break;
            case SystemLanguage.Korean:
                title = title_KR;
                break;

            default:
                title = title_EN;

                break;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
     if (!Application.isEditor) {
			//Create intent for action send
			AndroidJavaClass intentClass = 
				new AndroidJavaClass ("android.content.Intent");
			AndroidJavaObject intentObject = 
				new AndroidJavaObject ("android.content.Intent");
			intentObject.Call<AndroidJavaObject> 
				("setAction", intentClass.GetStatic<string> ("ACTION_SEND"));

			//put text and subject extra
			intentObject.Call<AndroidJavaObject> ("setType", "text/plain");
			intentObject.Call<AndroidJavaObject> 
				("putExtra", intentClass.GetStatic<string> ("EXTRA_SUBJECT"), title);
			intentObject.Call<AndroidJavaObject> 
				("putExtra", intentClass.GetStatic<string> ("EXTRA_TEXT"), body);

			//call createChooser method of activity class
			AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = 
				unity.GetStatic<AndroidJavaObject> ("currentActivity");
			AndroidJavaObject chooser = 
				intentClass.CallStatic<AndroidJavaObject> 
				("createChooser", intentObject, "Share");
			currentActivity.Call ("startActivity", chooser);
		}
#endif
    }

    public static bool Validator(
       object sender,
       X509Certificate certificate,
       X509Chain chain,
       SslPolicyErrors policyErrors)
    {

        //*** Just accept and move on...
        Debug.Log("Validation successful!");
        return true;
    }

    public static void Instate()
    {

        ServicePointManager.ServerCertificateValidationCallback = Validator;
    }

    private void VersionCheck()
    {
        //Instate();
        //string marketVersion = "";

        //string url = body;
        //HtmlWeb web = new HtmlWeb();
        //HtmlDocument doc = web.Load(url);

        //foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//div[@class='IQ1z0d']"))
        //{
        //    bool canConvert = false;

        //    string[] str = node.InnerText.Split('.');

        //    Debug.Log(node.InnerText);

        //    byte number2 = 0;

        //    if (str.Length == 3)
        //    {
        //        foreach (var item in str)
        //        {
        //            canConvert = byte.TryParse(item, out number2);
        //        }
        //    }

        //    if (canConvert)
        //        marketVersion = node.InnerText;
        //}

        //Debug.Log("marketVersion = " + marketVersion);
        //Debug.Log("appversion = " + Application.version);

    }


}
