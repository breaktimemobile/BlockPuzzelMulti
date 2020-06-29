﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudOnce;
using UnityEngine.UI;

public class CloudOnceManager : MonoBehaviour
{
    public static CloudOnceManager Instance;

    private void Awake()
    {
        Instance = this;
        Cloud.OnInitializeComplete += CloudOnceInitializeComplete;
        Cloud.OnCloudLoadComplete += CloudeLoad;

        Cloud.Initialize(true, false,false);
    }

    private void Start()
    {
        FireBaseManager.Instance.FirebaseNullLogin();
    }


    public void CloudOnceInitializeComplete()
    {
        Debug.Log("클라우드 완료");
        Cloud.OnInitializeComplete -= CloudOnceInitializeComplete;

    }
  
    public void Load()
    {

        if (!Cloud.IsSignedIn)
        {
            UIManager.Instance.Set_Google_Txt();
            UIManager.Instance.PushPopup(UIManager.Instance.GooglePopup);
        }
        else
        {
            Cloud.Storage.Load();
        }
    }

    public void CloudeLoad(bool success)
    {
        if (!success)
            return;

        string str = CloudVariables.Player_Data;

        if (str != "")
        {
            var aes = AESCrypto.instance.AESDecrypt128(str);
            var data = JsonUtility.FromJson<State_Player>(aes);
            DataManager.Instance.state_Player= data;
            DataManager.Instance.Save_Player_Data();

        }

        Language.GetInstance().Set(Application.systemLanguage);
        UIManager.Instance.SetUi();

        UIManager.Instance.Check_Daily();
    }

    public void Save()
    {
        Debug.Log("저장 세팅");

        if (!Cloud.IsSignedIn)
        {
            UIManager.Instance.Set_Google_Txt();
            UIManager.Instance.PushPopup(UIManager.Instance.GooglePopup);
        }
        else
        {
            string jsonStr = JsonUtility.ToJson(DataManager.Instance.state_Player);
            string aes = AESCrypto.instance.AESEncrypt128(jsonStr);

            CloudVariables.Player_Data = aes;

            Cloud.Storage.Save();

        }

    }

    public void Login()
    {
        Cloud.SignIn(true, authenticateCallBck);

    }

    public void Logout()
    {
        Cloud.SignOut();

        UIManager.Instance.PopPopup();

        DataManager.Instance.ResetData();
    }

    string token = "";

    void authenticateCallBck(bool sucess)
    {
        if (sucess)
        {

            PlayerPrefs.SetInt("Login", 1);

            UIManager.Instance.PopPopup();
            UIManager.Instance.Set_Google_Txt();

        }
        else
        {
            PlayerPrefs.SetInt("Login", 0);

  
        }
    }
    
    public void Report_Achievements()
    {
        switch (DataManager.Instance.state_Player.clear_Stage.Count)
        {
            case 10:
                Achievements.BestStage10.Unlock();

                break;
            case 30:
                Achievements.BestStage30.Unlock();

                break;
            case 50:
                Achievements.BestStage50.Unlock();

                break;
            case 100:
                Achievements.BestStage100.Unlock();

                break;
            case 200:
                Achievements.BestStage200.Unlock();

                break;
            case 300:
                Achievements.BestStage300.Unlock();

                break;
            case 500:
                Achievements.BestStage500.Unlock();

                break;

            default:
                break;
        }

        
        if (DataManager.Instance.state_Player.Classic >= 10000)
        {
            Achievements.BestClassic10000.Unlock();

        }

        if(DataManager.Instance.state_Player.Classic >= 5000)
        {
            Achievements.BestClassic5000.Unlock();

        }

        if (DataManager.Instance.state_Player.Classic >= 1000)
        {
            Achievements.BestClassic1000.Unlock();

        }

    }

    public void Report_Leaderboard(GameMode gameMode, int highScore = 0)
    {
        switch (gameMode)
        {
            case GameMode.Classic:

                Leaderboards.BestClassic.SubmitScore(highScore);

                break;
            case GameMode.Stage:
                Leaderboards.BestStage.SubmitScore(highScore);

                break;
            case GameMode.Multi:
                Leaderboards.BestMulti.SubmitScore(highScore);

                break;
            case GameMode.Timer:
                Leaderboards.BestTimer.SubmitScore(highScore);

                break;
            default:
                break;
        }
    }
    public void Show_Achievements()
    {
        if (!Social.localUser.authenticated)
        {
            UIManager.Instance.Set_Google_Txt();
            UIManager.Instance.PushPopup(UIManager.Instance.GooglePopup);
        }
        else
        {
            Cloud.Achievements.ShowOverlay();
        }
    }

    public void Show_Leaderboards()
    {
        if (!Social.localUser.authenticated)
        {
            UIManager.Instance.Set_Google_Txt();
            UIManager.Instance.PushPopup(UIManager.Instance.GooglePopup);
        }
        else
        {
            Cloud.Leaderboards.ShowOverlay();
        }

    }
}
