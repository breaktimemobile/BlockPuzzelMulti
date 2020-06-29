using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class State_Player
{
    public int crystal = 0;                        //크리스탈            
    public List<Clear_Stage_Info> clear_Stage      //클리어 스테이지 정보
        = new List<Clear_Stage_Info>();   
    public Item_Info item_info = new Item_Info();  //가지고 있는 아이템 정보
    public int starGift = 1;                       //누적 별 보상 몇번 받았는지
    public bool noAds = false;                     //광고 구매 여부
    public string giftTime = "";                   //선물 받은 시간
    public string shopgiftTime = "";               //선물 받은 시간
    public string daily = "";                      //데일리 받은 시간
    public bool isDaily = false;                   //오늘 데일리 받은 여부
    public int daily_Num = 0;                      //데일리 몇번째인지
    public int language = -1;                      //계정 언어 저장
    public int Rank = 0;                           //멀티 랭킹 점수
    public bool Check_Privacy = false;             //이용약관 체크
    public int Classic = 0;                        //클래식 점수
    public bool Check_Review = false;              //리뷰 체크
    public int Timer_Score = 0;                    //클래식 점수

}

[Serializable]
public class Clear_Stage_Info
{
    public int Stage_Id = 0;                        //스테이지 번호
    public int Stage_Star = 0;                      //스테이지 
}

[Serializable]
public class Item_Info
{
    public int[] Items = new int[5];                //아이템 개수

}

public enum Item
{
    crystal,
    rotation,
    Recover,
    change,
    boom,
    multi_double
}

public class DataManager : MonoBehaviour
{

    public static DataManager Instance;

    public List<Dictionary<string, object>> stage_data;         //스테이지 정보
    public List<Dictionary<string, object>> shop_data;          //상점 정보
    public List<Dictionary<string, object>> tutorial_data;      //튜토리얼 블럭 정보
    public List<Dictionary<string, object>> dailygift_data;      //튜토리얼 블럭 정보
    public List<Dictionary<string, object>> global_data;      //통화 정보

    public State_Player state_Player = new State_Player();      //플레이어 정보

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        Get_Csv_Data();
    }

    /// <summary>
    /// csv 데이터 가져오기
    /// </summary>
    void Get_Csv_Data()
    {

        stage_data = CSVReader.Read("stage");
        shop_data = CSVReader.Read("shop");
        tutorial_data = CSVReader.Read("tutorial");
        dailygift_data = CSVReader.Read("dailygift");
        global_data = CSVReader.Read("global");

    }

    /// <summary>
    /// json 데이터 가져오기
    /// </summary>
    public void Get_Json_Data()
    {
        if (PlayerPrefs.GetInt("Load",0).Equals(0))
        {
            JsonRead.Instance.CheckPlayer();
            PlayerPrefs.SetInt("Load", 1);
        }

        state_Player = JsonRead.Instance.Load_Player();

    }

    /// <summary>
    /// 플레이어 데이터 저장
    /// </summary>
    public void Save_Player_Data()
    {
        Debug.Log("데이터 저장 " + state_Player.daily);
        JsonRead.Instance.Save(state_Player);
    }

    /// <summary>
    /// 아이템 개수
    /// </summary>
    /// <param name="_item">아이템 정보</param>
    /// <param name="val">아이템 수량</param>
    public void Get_Item(Item _item, int val)
    {
        switch (_item)
        {
            case Item.crystal:
                state_Player.crystal += val;

                break;
            case Item.rotation:
            case Item.Recover:
            case Item.change:
            case Item.boom:
            case Item.multi_double:

                state_Player.item_info.Items[(int)_item - 1] += val;

                break;

            default:
                break;
        }

        Save_Player_Data();
        UIManager.Instance.Set_Item_Txt(_item);
    }

    /// <summary>
    /// 크리스탈 계산 체크
    /// </summary>
    /// <param name="val">가격</param>
    /// <returns></returns>
    public bool Check_Crystal(int val)
    {
        if (state_Player.crystal >= val) {
            return true;
        }

        UIManager.Instance.PushPopup(UIManager.Instance.ShopPopup);
        AdsManager.Instance.BannerShow(true);

        return false;
    }

    public void ResetData()
    {
        state_Player = new State_Player();
        Save_Player_Data();

        Language.GetInstance().Set(Application.systemLanguage);
        UIManager.Instance.SetUi();

        UIManager.Instance.Check_Daily();
    }
}
