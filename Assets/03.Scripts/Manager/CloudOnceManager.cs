using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CloudOnce;

public class CloudOnceManager : MonoBehaviour
{
    public static CloudOnceManager Instance;

    private void Awake()
    {
        Instance = this;
        Cloud.OnInitializeComplete += CloudOnceInitializeComplete;
        Cloud.Initialize(true, false);
    }

    private void Start()
    {
        FireBaseManager.Instance.FirebaseNullLogin();

    }

    public void Login()
    {
        Cloud.SignIn(true, authenticateCallBck);

    }

    public void Logout()
    {
        Cloud.SignOut();
        UIManager.Instance.PopPopup();

    }

    void authenticateCallBck(bool sucess)
    {
        if (sucess)
        {
            PlayerPrefs.SetInt("Login", 1);
            Debug.Log("로그인 성공 " + PlayerPrefs.GetInt("Login", 0));

            UIManager.Instance.PopPopup();
            UIManager.Instance.Set_Google_Txt();
            //FireBaseManager.Instance.FireBaseGoogleLogin();

        }
        else
        {
            PlayerPrefs.SetInt("Login", 0);
            Debug.Log("로그인 실패 " + PlayerPrefs.GetInt("Login", 0));

            UIManager.Instance.Set_Google_Txt();

        }
    }

    public void CloudOnceInitializeComplete()
    {
        Cloud.OnInitializeComplete -= CloudOnceInitializeComplete;

        // Do anything that requires CloudOnce to be initialized,
        // for example disabling your loading screen
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
