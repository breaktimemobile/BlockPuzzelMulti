using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private Text txtScore,highScore;
    [SerializeField] private Text TimerScore, TimerhighScore;

    [SerializeField] private GameObject[] scoreAnimator;
	[SerializeField] private Text txtAnimatedText;
	private int Score = 0;

    private Stack<int> RecoverScore = new Stack<int>();

    private int Check = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void Set_Score()
	{
        Check = 0;
           Score = 0;
        RecoverScore.Push(Score);

        switch (GamePlay.instance.gameMode)
        {
            case GameMode.Classic:
                txtScore.text = Score.ToString();
                highScore.text = DataManager.Instance.state_Player.Classic.ToString();
                break;
            case GameMode.Stage:
                break;
            case GameMode.Multi:
                break;
            case GameMode.Timer:
                TimerScore.text = Score.ToString();
                TimerhighScore.text = DataManager.Instance.state_Player.Timer_Score.ToString();
                break;
            default:
                break;
        }

	}

	public void AddScore(int scoreToAdd, bool doAnimate = true)
	{
		int oldScore = Score;
        RecoverScore.Push(Score);
        Score += scoreToAdd;

        switch (GamePlay.instance.gameMode)
        {
            case GameMode.Classic:
            case GameMode.Stage:

                break;
            case GameMode.Multi:

                Hashtable customRoomProperties = new Hashtable() { { "Score", Score } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(customRoomProperties);

                PhotonManager.Instance.Rpc_Score();

                break;
            default:
                break;
        }

        StartCoroutine(SetScore(oldScore, Score));

		if (doAnimate) {

            foreach (var item in scoreAnimator)
            {
                if (!item.activeSelf)
                {
                    item.transform.position = GamePlay.instance.score_pos;
                    item.GetComponentInChildren<Text>().text = "0";

                    item.SetActive(true);
                    StartCoroutine(Co_Active(item, scoreToAdd));
                    break;
                }     
            }
          
        }

        if (GamePlay.instance.gameMode.Equals(GameMode.Timer))
        {
            if (Check.Equals(1) && Score >= 10000)
            {
                Check++;
                BlockShapeSpawner.Instance.CreateShapeBlockProbabilityList();
            }

            if (Check.Equals(0) && Score >= 5000)
            {
                Check++;
                BlockShapeSpawner.Instance.CreateShapeBlockProbabilityList();
            }
        }
    }

    IEnumerator Co_Active(GameObject item, int currentScore)
    {

        item.GetComponentInChildren<Text>().text = string.Format("{0}", currentScore);

        yield return new WaitForSeconds(1F);
        item.SetActive(false);
    }

	public int GetScore()
	{
		return Score;
	}

	IEnumerator SetScore(int lastScore, int currentScore)
	{
		int IterationSize = (currentScore - lastScore) / 10;

		for (int index = 1; index < 10; index++) {
			lastScore += IterationSize;
            switch (GamePlay.instance.gameMode)
            {
                case GameMode.Classic:
                    txtScore.text = string.Format("{0}", lastScore);

                    break;
                case GameMode.Stage:
                    break;
                case GameMode.Multi:
                    break;
                case GameMode.Timer:
                    TimerScore.text = string.Format("{0}", lastScore);

                    break;
                default:
                    break;
            }
            yield return new WaitForEndOfFrame ();
		}
        switch (GamePlay.instance.gameMode)
        {
            case GameMode.Classic:
                txtScore.text = string.Format("{0}", currentScore);

                break;
            case GameMode.Stage:
                break;
            case GameMode.Multi:
                break;
            case GameMode.Timer:
                TimerScore.text = string.Format("{0}", currentScore);

                break;
            default:
                break;
        }
    }

    public void Check_HighScore()
    {
        switch (GamePlay.instance.gameMode)
        {
            case GameMode.Classic:

                int Old = DataManager.Instance.state_Player.Classic;
                if (Old <= Score)
                {
                    DataManager.Instance.state_Player.Classic = Score;
                    DataManager.Instance.Save_Player_Data();

                }
                break;
            case GameMode.Stage:
                break;
            case GameMode.Multi:
                break;
            case GameMode.Timer:

                Old = DataManager.Instance.state_Player.Timer_Score;
                if (Old <= Score)
                {
                    DataManager.Instance.state_Player.Timer_Score = Score;
                    DataManager.Instance.Save_Player_Data();

                }
                break;
            default:
                break;
        }

    }

    public void Recover()
    {
        if (RecoverScore.Count >= 1)
        {
            Score = RecoverScore.Pop();

            switch (GamePlay.instance.gameMode)
            {
                case GameMode.Classic:
                    txtScore.text = Score.ToString();

                    break;
                case GameMode.Stage:
                    break;
                case GameMode.Multi:
                    break;
                case GameMode.Timer:
                    TimerScore.text = Score.ToString();

                    break;
                default:
                    break;
            }
        }
    }
}
