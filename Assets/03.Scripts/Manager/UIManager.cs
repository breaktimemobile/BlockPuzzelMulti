using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum Game_Stat
{
    Main,
    Wait,
    Game,
    End
}

public enum enum_Msg
{
    Exit,
    Rematch,
    Cencel

}

public enum Enum_Gift
{
    Stay_Gift,
    Main_Gift,
    Push_Gift,
    Shop_Gift

}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Game_Stat game_Stat = Game_Stat.Main;
    public Enum_Gift enum_Gift = Enum_Gift.Main_Gift;


    private Transform Canvas;

    private GameObject Main;
    private GameObject Game;
    private GameObject Popup;

    #region Main

    private Button Btn_Classic;
    private Button Btn_Multi;
    private Button Btn_Stage;
    private Button Btn_Timer;
    private Button Btn_Setting;
    private Button Btn_Rank;
    private Button Btn_Shop;
    private Button Btn_Google;
    private Button Btn_Gift;
    private Text Txt_GiftTime;
    private Text Txt_Gift_Get;
    private Button Btn_AddDia;
    private Button Btn_Review;
    private Text Txt_Dia;
    public Button Btn_PushGift;
    private Button Btn_Achievements;
    private Button Btn_Ios;


    #endregion

    private Stack<GameObject> stack_Popup = new Stack<GameObject>();

    #region Game

    public Button Btn_Puase;
    private GameObject Top_Classic;
    private GameObject Top_Stage;
    private GameObject Top_Multi;
    private GameObject Top_Timer;


    private Text Txt_Top_Misison;
    private Transform Top_Mission_Grid;

    private GameObject Top_Mission_Block;
    private List<Image> Top_Mission_Boxs = new List<Image>();

    private GameObject Top_Mission_Time;
    private Slider Slider_Top_Mission_Time;
    private RectTransform Top_Mission_Clock;

    private GameObject Top_Mission_Score;
    private Text Txt_Top_Mission_Score;

    private List<Button> Btn_Items = new List<Button>();
    private List<Text> Txt_Item_Val = new List<Text>();

    private List<Button> Btn_Tuto_Items = new List<Button>();

    private Button Btn_Item_Multi;
    private GameObject Txt_Multi_Item_Free;
    private GameObject Img_Multi_Dia;
    public Text Txt_Multi_Sec;
    public Image Img_Multi_Item_Time;

    private Text Txt_Multi_Start_Time;
    private Text Txt_Multi_Title_Player;
    private Text Txt_Multi_Player;
    private Text Txt_Multi_Title_Enemy;
    private Text Txt_Multi_Enemy;

    private Slider Slider_Multi_Time;

    #endregion

    #region Popup

    private GameObject PausePopup;
    private GameObject Over_Popup;
    private GameObject StagePopup;
    private GameObject StageMissionPopup;
    private GameObject SettingPopup;
    private GameObject ExitPopup;
    private GameObject ReviewPopup;
    public GameObject ShopPopup;
    private GameObject HelpPopup;
    private GameObject HelpItemPopup;
    private GameObject HelpBlockPopup;
    private GameObject HelpModePopup;
    private GameObject HelpVisitPopup;
    public GameObject GooglePopup;
    private GameObject GiftPopup;
    private GameObject DailyPopup;
    private GameObject TutoPopup;
    private GameObject ColudPopup;
    private GameObject LanguagePopup;
    private GameObject MultiPopup;
    private GameObject MatcingPopup;
    private GameObject MsgPopup;
    private GameObject PrivacyPopup;
    private GameObject LoadingPopup;
    private GameObject NetworkPopup;
    private GameObject PushGiftPopup;
    private GameObject TutoMsgPopup;
    private GameObject BestPopup;
    private GameObject SelectPopup;

    #endregion

    #region PausePopup

    private Button Btn_Pause_Music;
    private Button Btn_Pause_Effect;
    private Button Btn_Pause_Share;
    private Button Btn_Pause_Main;
    private Button Btn_Pause_Play;
    private Button Btn_Pause_Replay;
    private Button Btn_Pause_Review;

    #endregion

    #region Over_Popup

    private GameObject Over_Classic_0;
    private GameObject Over_Classic_1;
    private GameObject Over_Stage;
    private GameObject Over_Multi;
    private GameObject Over_Timer_0;
    private GameObject Over_Timer_1;

    private Button Btn_Over_Classic_Rank;
    private Button Btn_Over_Classic_Main;
    private Button Btn_Over_Classic_Replay;
    private Button Btn_Over_Classic_Review;
    private Text Txt_Over_Classic_Score;
    private Text Txt_Over_Classic_Best;

    private Button Btn_Over_Classic_0;
    private Text Txt_Over_Classic_0_Price;
    private Button Btn_Over_Classic_0_Back;

    private Button Btn_Over_Stage_List;
    private Button Btn_Over_Stage_Replay;
    private Button Btn_Over_Stage_Next;
    private Button Btn_Over_Stage_Rank;
    private Button Btn_Over_Stage_Review;

    private Image Img_Over_Stage_Succece;
    private Text Txt_Over_Stage_Succece;
    private Image Img_Over_Stage_Fail;
    private Text Txt_Over_Stage_Fail;
    private Animator Pop;

    public Image Img_Over_Stage_Item;
    public Text Txt_Over_Stage_Item_Val;
    private List<Animation> Img_Stars = new List<Animation>();

    private Button Btn_Over_Up_Popup;
    private Button Btn_Over_Down_Popup;

    private Button Btn_Over_Multi_Back;
    private Image Img_Over_Multi_Player;
    private Text Txt_Over_Multi_Player_Name;
    private Text Txt_Over_Multi_Player_Rank;
    private Text Txt_Over_Multi_Player_Up;
    private Image Img_Over_Multi_Enemy;
    private Text Txt_Over_Multi_Enemy_Name;
    private Text Txt_Multi_Win;
    private Text Txt_Multi_Lose;

    private Image Img_Multi_Win;
    private Image Img_Multi_Lose;
    private Image Img_Multi_Draw;
    private List<Animation> Img_Multi_Stars = new List<Animation>();
    private List<GameObject> Img_None_Star = new List<GameObject>();


    public Button Btn_Multi_ReStart;

    private Button Btn_Over_Timer_0;
    private Text Txt_Over_Timer_0_Price;
    private Button Btn_Over_Timer_0_Back;

    private Button Btn_Over_Timer_1_Rank;
    private Button Btn_Over_Timer_1_Main;
    private Button Btn_Over_Timer_1_Replay;
    private Button Btn_Over_Timer_1_Review;
    private Text Txt_Over_Timer_1_Score;
    private Text Txt_Over_Timer_1_Best;

    #endregion

    #region StagePopup

    private Button Btn_Stage_Back;
    private Transform Tr_Stage_Content;
    private Text Txt_Star_Val;
    private Button Btn_Star_Get;

    #endregion

    #region StageMissionPopup

    private List<Button> Btn_Mission_Back = new List<Button>();
    private List<Button> Btn_Stage_Start = new List<Button>();

    private List<Text> Txt_Stage_Level_Val = new List<Text>();

    private List<GameObject> Mission_Bg = new List<GameObject>();

    private List<Image> Mission_Boxs = new List<Image>();

    private Transform Mission_Block;

    private Transform Mission_Time;
    private Text Txt_Mission_Time;

    private Transform Mission_Score;
    private Text Txt_Mission_Score;

    #endregion


    #region SettingPopup

    private Button Btn_Setting_Back;
    private Button Btn_Setting_Music;
    private Button Btn_Setting_Effect;
    private Button Btn_Setting_Save;
    private Button Btn_Setting_Load;
    private Button Btn_Setting_Share;
    private Button Btn_Setting_Review;
    private Button Btn_Setting_Help;
    private Button Btn_Setting_Exit;
    private Button Btn_Setting_Google_Login;
    private Button Btn_Setting_Google_Logout;
    private Button Btn_Setting_Language;
    private Text Txt_Setting_Ver;

    #endregion

    #region ExitPopup

    private Button Btn_Exit_Ok;
    private Button Btn_Exit_No;

    #endregion

    #region ReviewPopup

    private Button Btn_Review_Ok;
    private Button Btn_Review_Back;

    #endregion

    #region ShopPopup

    private Button Btn_Shop_Back;
    private Text Txt_Shop_Dia;

    #endregion

    #region HelpPopup

    private Button Btn_Help_Back;
    private Button Btn_Help_Item;
    private Button Btn_Help_Block;
    private Button Btn_Help_Mode;
    private Button Btn_Help_Visit;

    private Button Btn_HelpItem_Back;
    private Button Btn_HelpBlock_Back;
    private Button Btn_HelpMode_Back;
    private Button Btn_HelpVisit_Back;

    private Button Btn_Help_Service;
    private Button Btn_Help_Privacy;

    #endregion

    #region GooglePopup

    private Button Btn_Google_Back;
    private Button Btn_Google_Login;
    private Button Btn_Google_Logout;
    private GameObject Txt_Google_Title_Login;
    private GameObject Txt_Ios_Title_Login;
    private GameObject Txt_Google_Title_Logout;
    #endregion

    #region GiftPopup

    private Image[] Img_Gift;
    private Text[] Txt_Gift_Val;

    private Button Btn_Gift_Ok;
    private Button Btn_Gift_Ads;

    private Animator Anim_Gift;

    #endregion


    #region DailyPopup

    private Button Btn_Daily_Back;
    private List<Button> Btn_Day = new List<Button>();
    private List<Image> Img_Check = new List<Image>();
    private List<Text> Txt_Daily = new List<Text>();


    #endregion

    #region TutoPopup

    private List<GameObject> obj_Tuto = new List<GameObject>();
    private List<List<GameObject>> obj_Tuto_child = new List<List<GameObject>>();
    private List<GameObject> touchs = new List<GameObject>();

    #endregion

    #region TxtPopup

    private GameObject Txt_Saving;
    private GameObject Txt_Save;
    private GameObject Txt_Loading;
    private GameObject Txt_Load;

    #endregion


    #region LanguagePopup

    private Button Btn_Language_Back;
    private Button[] Btn_Languages;

    #endregion

    #region  MultiPopup

    private Button Btn_Multi_Back;
    private Button Btn_Multi_Dia_1;
    private Button Btn_Multi_Dia_5;
    private Button Btn_Multi_Dia_10;
    public Text Txt_Player_Count;


    #endregion

    #region  MatcingPopup

    private Button Btn_Matcing_Back;
    private Image Img_Player;
    private Text Txt_Player_Name;
    private Image Img_Enemy;
    private Text Txt_Enemy_Name;
    private Text Txt_Count;

    #endregion

    #region  MsgPopup

    private Text Txt_Msg_Exit;
    private Text Txt_Msg_ReMatch;
    private Text Txt_Msg_Cencel;

    #endregion

    #region  PrivacyPopup

    private Button Btn_Privacy_Back;
    private Toggle Toggle_Privacy;
    private Toggle Toggle_service;
    private Button Btn_Privacy;
    private Button Btn_service;
    #endregion


    #region PushGiftPopup

    private Image[] Img_Push_Gift;
    private Text[] Txt_Push_Gift_Val;

    private Button Btn_Push_Gift_Ok;
    private Button Btn_Push_Gift_Ads;

    private Animator Anim_Push_Gift;

    #endregion

    #region TutoMsgPopup

    private List<GameObject> Obj_TutoMsg = new List<GameObject>();
    private List<Button> Btn_TutoMsg_Back = new List<Button>();

    #endregion

    #region BestPopup

    private Button Btn_Best_Game_01;
    private Button Btn_Best_Game_02;
    private Button Btn_Best_Game_03;
    private Button Btn_Best_Back;
    private Button Btn_Best_Start;

    #endregion

    #region SelectPopup

    private Button Btn_8x8;
    private Button Btn_9x9;
    private Button Btn_Select_Back;

    #endregion



    [SerializeField] private GameObject Stage_Block;

    public Sprite Sp_None_Star;
    public Sprite Sp_Star;
    public Sprite[] Sp_Blocks;

    List<Stage_Info> stage_Infos = new List<Stage_Info>();

    public Sprite[] Music;
    public Sprite[] Effect;
    public Sprite[] sp_Items;

    private int[] langeuae = new int[] { 23, 10, 22, 40, 41, 15, 21, 34, 14, 20, 28, 39, 37, 36, 30, 99999 };


    bool isBackKey = false;

    int clearScore = 0;

    int myTween = -1;

    public List<GameObject> explosion = new List<GameObject>();

    public GameObject emptyBlockTemplate;

    public ShapeBlockList shapeBlockList;                         //소환 블록 기본 정보

    bool Show_Ads = false;

    private void Awake()
    {
        LeanTween.init(3000);
        Instance = this;
        Find_Obj();
        //LoadingPopup.SetActive(true);

    }

    private void Start()
    {
        UIManager.Instance.Btn_PushGift.gameObject.SetActive(false);

        AudioManager.instance.Play_Music_Sound(Music_Sound.title_bg);

        Application.targetFrameRate = 60;

        AddListener();
        //LanguageManager.Instance.Get_Language();

        Debug.Log(Application.systemLanguage);

        DataManager.Instance.Get_Json_Data();

        LanguageManager.Instance.Get_Language();

        ShopManager.Instance.Shop_Setting(UIManager.Instance.ShopPopup);

        ShopManager.Instance.Set_Shop_Gift_Time();

        SetUi();

        AdsManager.Instance.Set_Ads();

        //개인정보 동의 안함 && 한국일때만
        if (PlayerPrefs.GetInt("service", 0).Equals(0) && Application.systemLanguage.Equals(SystemLanguage.Korean))
        {

            PushPopup(PrivacyPopup);
        }
        //개인정보 동의 함
        else
        {
            PlayerPrefs.SetInt("service", 1);

            SocialManager.Instance.DoAutoLogin();


            Check_Daily();
            //PushPopup(BestPopup);

#if UNITY_EDITOR
            //FireBaseManager.Instance.Get_Editor_Gift();

#else
            FireBaseManager.Instance.Add_Token();
#endif


        }

        Create_Object();
    }

    public void Create_Object()
    {
        foreach (var item in explosion)
        {
            ObjectPool.CreatePool(item, 60);

        }

        foreach (var item in shapeBlockList.ShapeBlocks)
        {
            ObjectPool.CreatePool(item.shapeBlock, 2);

        }


        ObjectPool.CreatePool(emptyBlockTemplate, 64);
    }

    /// <summary>
    /// 오브젝트 찾기
    /// </summary>
    public void Find_Obj()
    {
        Canvas = GameObject.Find("Canvas").transform;

        Main = Canvas.Find("Main").gameObject;
        Game = Canvas.Find("Game").gameObject;
        Popup = Canvas.Find("Popup").gameObject;

        #region Main

        Btn_Classic = Main.transform.Find("Btn_Classic").GetComponent<Button>();
        Btn_Multi = Main.transform.Find("Btn_Multi").GetComponent<Button>();
        Btn_Stage = Main.transform.Find("Btn_Stage").GetComponent<Button>();
        Btn_Timer = Main.transform.Find("Btn_Timer").GetComponent<Button>();
        Btn_Setting = Main.transform.Find("Btn_Setting").GetComponent<Button>();
        Btn_Rank = Main.transform.Find("Btn_Rank").GetComponent<Button>();
        Btn_Shop = Main.transform.Find("Btn_Shop").GetComponent<Button>();
        Btn_Google = Main.transform.Find("Btn_Google").GetComponent<Button>();
        Btn_Gift = Main.transform.Find("Btn_Gift").GetComponent<Button>();
        Txt_GiftTime = Btn_Gift.transform.Find("Txt_GiftTime").GetComponent<Text>();
        Txt_Gift_Get = Btn_Gift.transform.Find("Txt_Gift_Get").GetComponent<Text>();
        Btn_AddDia = Main.transform.Find("Img_DiaBg/Btn_AddDia").GetComponent<Button>();
        Txt_Dia = Main.transform.Find("Img_DiaBg/Txt_Dia").GetComponent<Text>();
        Btn_Review = Main.transform.Find("Btn_Review").GetComponent<Button>();
        Btn_PushGift = Main.transform.Find("Btn_PushGift").GetComponent<Button>();
        Btn_Achievements = Main.transform.Find("Btn_Achievements").GetComponent<Button>();
        Btn_Ios = Main.transform.Find("Btn_Ios").GetComponent<Button>();

        #endregion

        #region Game


        Top_Classic = Game.transform.Find("Top/Top_Classic").gameObject;
        Top_Stage = Game.transform.Find("Top/Top_Stage").gameObject;
        Top_Multi = Game.transform.Find("Top/Top_Multi").gameObject;
        Top_Timer = Game.transform.Find("Top/Top_Timer").gameObject;

        Btn_Puase = Game.transform.Find("Top/Btn_Puase").GetComponent<Button>();

        Top_Mission_Block = Top_Stage.transform.Find("Top_Mission_Block").gameObject;
        Top_Mission_Grid = Top_Mission_Block.transform.Find("Top_Mission_Grid");

        foreach (var item in Top_Mission_Grid.GetComponentsInChildren<Image>())
        {
            if (item.name.Contains("Misssion_Box"))
                Top_Mission_Boxs.Add(item);

        }

        Top_Mission_Time = Game.transform.Find("Top/Top_Mission_Time").gameObject;
        Slider_Top_Mission_Time = Top_Mission_Time.transform.Find("Slider_Top_Mission_Time").GetComponent<Slider>();
        Top_Mission_Clock = Top_Mission_Time.transform.Find("Top_Mission_Clock").GetComponent<RectTransform>();
        Top_Mission_Score = Top_Stage.transform.Find("Top_Mission_Score").gameObject;
        Txt_Top_Mission_Score = Top_Mission_Score.transform.Find("Txt_Top_Mission_Score").GetComponent<Text>();

        Txt_Top_Misison = Top_Stage.transform.Find("Txt_Top_Misison").GetComponent<Text>();

        Btn_Items = Game.transform.Find("Item/Basic").GetComponentsInChildren<Button>().ToList();
        Txt_Item_Val = Game.transform.Find("Item/Basic").GetComponentsInChildren<Text>().ToList();

        Btn_Tuto_Items = Game.transform.Find("Item/Tuto").GetComponentsInChildren<Button>().ToList();

        Btn_Item_Multi = Game.transform.Find("Item/Btn_Item_Multi").GetComponent<Button>();
        Txt_Multi_Item_Free = Btn_Item_Multi.transform.Find("Txt_Multi_Item_Free").gameObject;
        Img_Multi_Dia = Btn_Item_Multi.transform.Find("Img_Multi_Dia").gameObject;
        Txt_Multi_Start_Time = Game.transform.Find("Txt_Multi_Start_Time").GetComponent<Text>();
        Txt_Multi_Sec = Btn_Item_Multi.transform.Find("Txt_Multi_Sec").GetComponent<Text>();
        Img_Multi_Item_Time = Btn_Item_Multi.transform.Find("Img_Multi_Item_Time").GetComponent<Image>();

        Txt_Multi_Title_Player = Top_Multi.transform.Find("Obj_Player/Txt_Multi_Title_Player").GetComponent<Text>();
        Txt_Multi_Player = Top_Multi.transform.Find("Obj_Player/Txt_Multi_Player").GetComponent<Text>();
        Txt_Multi_Title_Enemy = Top_Multi.transform.Find("Obj_Enemy/Txt_Multi_Title_Enemy").GetComponent<Text>();
        Txt_Multi_Enemy = Top_Multi.transform.Find("Obj_Enemy/Txt_Multi_Enemy").GetComponent<Text>();
        Slider_Multi_Time = Top_Multi.transform.Find("Top_Multi_Time/Slider_Multi_Time").GetComponent<Slider>();

        #endregion

        #region Popup

        PausePopup = Popup.transform.Find("PausePopup").gameObject;
        Over_Popup = Popup.transform.Find("Over_Popup").gameObject;
        StagePopup = Popup.transform.Find("StagePopup").gameObject;
        StageMissionPopup = Popup.transform.Find("StageMissionPopup").gameObject;
        SettingPopup = Popup.transform.Find("SettingPopup").gameObject;
        ExitPopup = Popup.transform.Find("ExitPopup").gameObject;
        ReviewPopup = Popup.transform.Find("ReviewPopup").gameObject;
        ShopPopup = Popup.transform.Find("ShopPopup").gameObject;
        HelpPopup = Popup.transform.Find("HelpPopup").gameObject;
        HelpItemPopup = HelpPopup.transform.Find("HelpItemPopup").gameObject;
        HelpBlockPopup = HelpPopup.transform.Find("HelpBlockPopup").gameObject;
        HelpModePopup = HelpPopup.transform.Find("HelpModePopup").gameObject;
        HelpVisitPopup = HelpPopup.transform.Find("HelpVisitPopup").gameObject;
        GooglePopup = Popup.transform.Find("GooglePopup").gameObject;
        GiftPopup = Popup.transform.Find("GiftPopup").gameObject;
        DailyPopup = Popup.transform.Find("DailyPopup").gameObject;
        TutoPopup = Popup.transform.Find("TutoPopup").gameObject;
        ColudPopup = Popup.transform.Find("ColudPopup").gameObject;
        LanguagePopup = Popup.transform.Find("LanguagePopup").gameObject;
        MultiPopup = Popup.transform.Find("MultiPopup").gameObject;
        MatcingPopup = Popup.transform.Find("MatcingPopup").gameObject;
        MsgPopup = Popup.transform.Find("MsgPopup").gameObject;
        PrivacyPopup = Popup.transform.Find("PrivacyPopup").gameObject;
        NetworkPopup = Popup.transform.Find("NetworkPopup").gameObject;
        PushGiftPopup = Popup.transform.Find("PushGiftPopup").gameObject;
        TutoMsgPopup = Popup.transform.Find("TutoMsgPopup").gameObject;
        BestPopup = Popup.transform.Find("BestPopup").gameObject;
        SelectPopup = Popup.transform.Find("SelectPopup").gameObject;

        #endregion


        #region PausePopup

        Btn_Pause_Music = PausePopup.transform.Find("Btn_Pause_Music").GetComponent<Button>();
        Btn_Pause_Effect = PausePopup.transform.Find("Btn_Pause_Effect").GetComponent<Button>();
        Btn_Pause_Main = PausePopup.transform.Find("Btn_Pause_Main").GetComponent<Button>();
        Btn_Pause_Play = PausePopup.transform.Find("Btn_Pause_Play").GetComponent<Button>();
        Btn_Pause_Share = PausePopup.transform.Find("Btn_Pause_Share").GetComponent<Button>();
        Btn_Pause_Replay = PausePopup.transform.Find("Btn_Pause_Replay").GetComponent<Button>();
        Btn_Pause_Review = PausePopup.transform.Find("Btn_Pause_Review").GetComponent<Button>();

        #endregion

        #region Over_Popup

        Pop = Over_Popup.transform.Find("Pop").GetComponent<Animator>();

        Over_Classic_0 = Pop.transform.Find("Over_Classic_0").gameObject;
        Over_Classic_1 = Pop.transform.Find("Over_Classic_1").gameObject;
        Over_Stage = Pop.transform.Find("Over_Stage").gameObject;
        Over_Multi = Pop.transform.Find("Over_Multi").gameObject;
        Over_Timer_0 = Pop.transform.Find("Over_Timer_0").gameObject;
        Over_Timer_1 = Pop.transform.Find("Over_Timer_1").gameObject;

        Btn_Over_Classic_Rank = Over_Classic_1.transform.Find("Btn_Over_Classic_Rank").GetComponent<Button>();
        Btn_Over_Classic_Main = Over_Classic_1.transform.Find("Btn_Over_Classic_Main").GetComponent<Button>();
        Btn_Over_Classic_Replay = Over_Classic_1.transform.Find("Btn_Over_Classic_Replay").GetComponent<Button>();
        Btn_Over_Classic_Review = Over_Classic_1.transform.Find("Btn_Over_Classic_Review").GetComponent<Button>();
        Txt_Over_Classic_Score = Over_Classic_1.transform.Find("Txt_Over_Classic_Score").GetComponent<Text>();
        Txt_Over_Classic_Best = Over_Classic_1.transform.Find("Txt_Over_Classic_Best").GetComponent<Text>();

        Btn_Over_Classic_0 = Over_Classic_0.transform.Find("Btn_Over_Classic_0").GetComponent<Button>();
        Txt_Over_Classic_0_Price = Btn_Over_Classic_0.transform.Find("Txt_Over_Classic_0_Price").GetComponent<Text>();
        Btn_Over_Classic_0_Back = Over_Classic_0.transform.Find("Btn_Over_Classic_0_Back").GetComponent<Button>();

        Btn_Over_Stage_List = Over_Stage.transform.Find("Btn_Over_Stage_List").GetComponent<Button>();
        Btn_Over_Stage_Replay = Over_Stage.transform.Find("Btn_Over_Stage_Replay").GetComponent<Button>();
        Btn_Over_Stage_Next = Over_Stage.transform.Find("Btn_Over_Stage_Next").GetComponent<Button>();
        Btn_Over_Stage_Rank = Over_Stage.transform.Find("Btn_Over_Stage_Rank").GetComponent<Button>();
        Btn_Over_Stage_Review = Over_Stage.transform.Find("Btn_Over_Stage_Review").GetComponent<Button>();
        Txt_Over_Stage_Succece = Over_Stage.transform.Find("Txt_Over_Stage_Succece").GetComponent<Text>();
        Txt_Over_Stage_Fail = Over_Stage.transform.Find("Txt_Over_Stage_Fail").GetComponent<Text>();
        Img_Over_Stage_Succece = Over_Stage.transform.Find("Img_Over_Stage_Succece").GetComponent<Image>();
        Img_Over_Stage_Fail = Over_Stage.transform.Find("Img_Over_Stage_Fail").GetComponent<Image>();

        Img_Over_Stage_Item = Over_Stage.transform.Find("Img_Over_Stage_Item").GetComponent<Image>();
        Txt_Over_Stage_Item_Val = Over_Stage.transform.Find("Txt_Over_Stage_Item_Val").GetComponent<Text>();
        Img_Stars = Over_Stage.GetComponentsInChildren<Animation>(true).ToList();


        Btn_Over_Up_Popup = Over_Popup.transform.Find("Btn_Over_Up_Popup").GetComponent<Button>();
        Btn_Over_Down_Popup = Over_Popup.transform.Find("Btn_Over_Down_Popup").GetComponent<Button>();

        Btn_Over_Multi_Back = Over_Multi.transform.Find("Btn_Over_Multi_Back").GetComponent<Button>();
        Img_Over_Multi_Player = Over_Multi.transform.Find("Img_Over_Multi_Player").GetComponent<Image>();
        Txt_Over_Multi_Player_Name = Img_Over_Multi_Player.transform.Find("Txt_Over_Multi_Player_Name").GetComponent<Text>();
        Txt_Over_Multi_Player_Rank = Over_Multi.transform.Find("Txt_Over_Multi_Player_Rank").GetComponent<Text>();
        Txt_Over_Multi_Player_Up = Txt_Over_Multi_Player_Rank.transform.Find("Txt_Over_Multi_Player_Up").GetComponent<Text>();

        Img_Over_Multi_Enemy = Over_Multi.transform.Find("Img_Over_Multi_Enemy").GetComponent<Image>();
        Txt_Over_Multi_Enemy_Name = Img_Over_Multi_Enemy.transform.Find("Txt_Over_Multi_Enemy_Name").GetComponent<Text>();

        Txt_Multi_Win = Over_Multi.transform.Find("Txt_Multi_Win").GetComponent<Text>();
        Txt_Multi_Lose = Over_Multi.transform.Find("Txt_Multi_Lose").GetComponent<Text>();
        Img_Multi_Win = Over_Multi.transform.Find("Img_Multi_Win").GetComponent<Image>();
        Img_Multi_Lose = Over_Multi.transform.Find("Img_Multi_Lose").GetComponent<Image>();
        Img_Multi_Draw = Over_Multi.transform.Find("Img_Multi_Draw").GetComponent<Image>();

        Img_Multi_Stars = Over_Multi.GetComponentsInChildren<Animation>(true).ToList();

        foreach (var item in Over_Multi.GetComponentsInChildren<Image>())
        {
            if (item.name.Contains("Img_None_Star"))
                Img_None_Star.Add(item.gameObject);

        }

        Btn_Multi_ReStart = Over_Multi.transform.Find("Btn_Multi_ReStart").GetComponent<Button>();

        Btn_Over_Timer_0 = Over_Timer_0.transform.Find("Btn_Over_Timer_0").GetComponent<Button>();
        Txt_Over_Timer_0_Price = Btn_Over_Timer_0.transform.Find("Txt_Over_Timer_0_Price").GetComponent<Text>();
        Btn_Over_Timer_0_Back = Over_Timer_0.transform.Find("Btn_Over_Timer_0_Back").GetComponent<Button>();

        Btn_Over_Timer_1_Rank = Over_Timer_1.transform.Find("Btn_Over_Timer_1_Rank").GetComponent<Button>();
        Btn_Over_Timer_1_Main = Over_Timer_1.transform.Find("Btn_Over_Timer_1_Main").GetComponent<Button>();
        Btn_Over_Timer_1_Replay = Over_Timer_1.transform.Find("Btn_Over_Timer_1_Replay").GetComponent<Button>();
        Btn_Over_Timer_1_Review = Over_Timer_1.transform.Find("Btn_Over_Timer_1_Review").GetComponent<Button>();
        Txt_Over_Timer_1_Score = Over_Timer_1.transform.Find("Txt_Over_Timer_1_Score").GetComponent<Text>();
        Txt_Over_Timer_1_Best = Over_Timer_1.transform.Find("Txt_Over_Timer_1_Best").GetComponent<Text>();


        #endregion

        #region StagePopup

        Btn_Stage_Back = StagePopup.transform.Find("Btn_Stage_Back").GetComponent<Button>();
        Tr_Stage_Content = StagePopup.transform.Find("Scroll View/Viewport/Tr_Stage_Content");
        Txt_Star_Val = StagePopup.transform.Find("Img_Star_Bg/Txt_Star_Val").GetComponent<Text>();
        Btn_Star_Get = StagePopup.transform.Find("Img_Star_Bg/Btn_Star_Get").GetComponent<Button>();

        #endregion

        #region StageMissionPopup

        foreach (var item in StageMissionPopup.GetComponentsInChildren<Transform>(true))
        {
            if (item.name.Equals("Btn_Mission_Back"))
                Btn_Mission_Back.Add(item.GetComponent<Button>());

            if (item.name.Equals("Txt_Stage_Level_Val"))
                Txt_Stage_Level_Val.Add(item.GetComponent<Text>());

            if (item.name.Equals("Btn_Stage_Start"))
                Btn_Stage_Start.Add(item.GetComponent<Button>());

            if (item.name.Equals("1"))
                Mission_Bg.Add(item.gameObject);

            if (item.name.Equals("2"))
                Mission_Bg.Add(item.gameObject);

            if (item.name.Equals("3"))
                Mission_Bg.Add(item.gameObject);

            if (item.name.Equals("Mission_Box"))
                Mission_Boxs.Add(item.GetComponent<Image>());

        }

        Mission_Block = StageMissionPopup.transform.Find("Mission_List/Mission_Block");

        Mission_Time = StageMissionPopup.transform.Find("Mission_List/Mission_Time");
        Txt_Mission_Time = Mission_Time.transform.Find("Txt_Mission_Time").GetComponent<Text>();

        Mission_Score = StageMissionPopup.transform.Find("Mission_List/Mission_Score");
        Txt_Mission_Score = Mission_Score.transform.Find("Txt_Mission_Score").GetComponent<Text>();


        #endregion

        #region SettingPopup

        Btn_Setting_Back = SettingPopup.transform.Find("Btn_Setting_Back").GetComponent<Button>();
        Btn_Setting_Music = SettingPopup.transform.Find("Setting_Btns/Btn_Setting_Music").GetComponent<Button>();
        Btn_Setting_Effect = SettingPopup.transform.Find("Setting_Btns/Btn_Setting_Effect").GetComponent<Button>();
        Btn_Setting_Save = SettingPopup.transform.Find("Setting_Btns/Btn_Setting_Save").GetComponent<Button>();
        Btn_Setting_Load = SettingPopup.transform.Find("Setting_Btns/Btn_Setting_Load").GetComponent<Button>();
        Btn_Setting_Share = SettingPopup.transform.Find("Setting_Btns/Btn_Setting_Share").GetComponent<Button>();
        Btn_Setting_Review = SettingPopup.transform.Find("Setting_Btns/Btn_Setting_Review").GetComponent<Button>();
        Btn_Setting_Help = SettingPopup.transform.Find("Setting_Btns/Btn_Setting_Help").GetComponent<Button>();
        Btn_Setting_Exit = SettingPopup.transform.Find("Setting_Btns/Btn_Setting_Exit").GetComponent<Button>();
        Btn_Setting_Google_Login = SettingPopup.transform.Find("Btn_Setting_Google_Login").GetComponent<Button>();
        Btn_Setting_Google_Logout = SettingPopup.transform.Find("Btn_Setting_Google_Logout").GetComponent<Button>();
        Btn_Setting_Language = SettingPopup.transform.Find("Setting_Btns/Btn_Setting_Language").GetComponent<Button>();
        Txt_Setting_Ver = SettingPopup.transform.Find("Txt_Setting_Ver").GetComponent<Text>();

        #endregion

        #region ExitPopup

        Btn_Exit_Ok = ExitPopup.transform.Find("Btn_Exit_Ok").GetComponent<Button>();
        Btn_Exit_No = ExitPopup.transform.Find("Btn_Exit_No").GetComponent<Button>();

        #endregion

        #region ReviewPopup

        Btn_Review_Ok = ReviewPopup.transform.Find("Btn_Review_Ok").GetComponent<Button>();
        Btn_Review_Back = ReviewPopup.transform.Find("Btn_Review_Back").GetComponent<Button>();

        #endregion

        #region ShopPopup
        Btn_Shop_Back = ShopPopup.transform.Find("Safe_Area/Btn_Shop_Back").GetComponent<Button>();
        Txt_Shop_Dia = ShopPopup.transform.Find("Safe_Area/Img_Shop_DiaBg/Txt_Shop_Dia").GetComponent<Text>();
        #endregion


        #region HelpPopup

        Btn_Help_Back = HelpPopup.transform.Find("Btn_Help_Back").GetComponent<Button>();
        Btn_Help_Item = HelpPopup.transform.Find("Btn_Help_Item").GetComponent<Button>();
        Btn_Help_Block = HelpPopup.transform.Find("Btn_Help_Block").GetComponent<Button>();
        Btn_Help_Mode = HelpPopup.transform.Find("Btn_Help_Mode").GetComponent<Button>();
        Btn_Help_Visit = HelpPopup.transform.Find("Btn_Help_Visit").GetComponent<Button>();

        Btn_HelpItem_Back = HelpItemPopup.transform.Find("Btn_HelpItem_Back").GetComponent<Button>();
        Btn_HelpBlock_Back = HelpBlockPopup.transform.Find("Btn_HelpBlock_Back").GetComponent<Button>();
        Btn_HelpMode_Back = HelpModePopup.transform.Find("Btn_HelpMode_Back").GetComponent<Button>();
        Btn_HelpVisit_Back = HelpVisitPopup.transform.Find("Btn_HelpVisit_Back").GetComponent<Button>();

        Btn_Help_Service = HelpPopup.transform.Find("Btn_Help_Service").GetComponent<Button>();
        Btn_Help_Privacy = HelpPopup.transform.Find("Btn_Help_Privacy").GetComponent<Button>();

        #endregion

        #region GooglePopup

        Btn_Google_Back = GooglePopup.transform.Find("Btn_Google_Back").GetComponent<Button>();

        Btn_Google_Login = GooglePopup.transform.Find("Btn_Google_Login").GetComponent<Button>();
        Btn_Google_Logout = GooglePopup.transform.Find("Btn_Google_Logout").GetComponent<Button>();

        Txt_Google_Title_Login = GooglePopup.transform.Find("Txt_Google_Title_Login").gameObject;
        Txt_Ios_Title_Login = GooglePopup.transform.Find("Txt_Ios_Title_Login").gameObject;
        Txt_Google_Title_Logout = GooglePopup.transform.Find("Txt_Google_Title_Logout").gameObject;
        #endregion

        #region GiftPopup

        Anim_Gift = GiftPopup.transform.Find("BonusReward/Box_main").GetComponent<Animator>();

        Img_Gift = Anim_Gift.transform.GetComponentsInChildren<Image>(true);
        Txt_Gift_Val = Anim_Gift.transform.GetComponentsInChildren<Text>(true);

        Btn_Gift_Ok = GiftPopup.transform.Find("Btn_Gift_Ok").GetComponent<Button>();
        Btn_Gift_Ads = GiftPopup.transform.Find("Btn_Gift_Ads").GetComponent<Button>();

        #endregion

        #region DailyPopup

        Btn_Daily_Back = DailyPopup.transform.Find("Btn_Daily_Back").GetComponent<Button>();
        Btn_Day = DailyPopup.transform.Find("Day").GetComponentsInChildren<Button>().ToList();

        foreach (var item in Btn_Day)
        {
            Img_Check.Add(item.transform.Find("Img_Check").GetComponent<Image>());
            Txt_Daily.Add(item.transform.Find("Txt_daily_1").GetComponent<Text>());
        }

        #endregion

        #region TutoPopup

        List<Transform> obj = TutoPopup.GetComponentsInChildren<Transform>(true).ToList();


        for (int i = 1; i < 8; i++)
        {
            obj_Tuto.Add(obj.Find(x => x.name.Equals(i.ToString())).gameObject);
            List<GameObject> obj_child = new List<GameObject>();

            List<Transform> obj_ch = obj_Tuto[i - 1].GetComponentsInChildren<Transform>(true).ToList();

            for (int j = 1; ; j++)
            {
                Transform co = obj_ch.Find(x => x.name.Equals(i + "-" + j));

                if (co != null)
                    obj_child.Add(co.gameObject);
                else
                    break;
            }
            obj_Tuto_child.Add(obj_child);
        }

        List<Transform> tan = obj.FindAll(x => x.name.Equals("touch"));

        foreach (var item in tan)
        {
            touchs.Add(item.gameObject);

        }


        #endregion

        #region ColudPopup

        Txt_Saving = ColudPopup.transform.Find("Txt_Saving").gameObject;
        Txt_Save = ColudPopup.transform.Find("Txt_Save").gameObject;
        Txt_Loading = ColudPopup.transform.Find("Txt_Loading").gameObject;
        Txt_Load = ColudPopup.transform.Find("Txt_Load").gameObject;

        #endregion

        #region LanguagePopup

        Btn_Language_Back = LanguagePopup.transform.Find("Btn_Language_Back").GetComponent<Button>();
        Btn_Languages = LanguagePopup.transform.Find("Btn_Languages").GetComponentsInChildren<Button>(true);

        #endregion


        #region  MultiPopup

        Btn_Multi_Back = MultiPopup.transform.Find("Btn_Multi_Back").GetComponent<Button>();
        Btn_Multi_Dia_1 = MultiPopup.transform.Find("Btn_Multi_Dia_1").GetComponent<Button>();
        Btn_Multi_Dia_5 = MultiPopup.transform.Find("Btn_Multi_Dia_5").GetComponent<Button>();
        Btn_Multi_Dia_10 = MultiPopup.transform.Find("Btn_Multi_Dia_10").GetComponent<Button>();
        Txt_Player_Count = MultiPopup.transform.Find("Txt_Player_Count").GetComponent<Text>();

        #endregion

        #region  MatcingPopup

        Btn_Matcing_Back = MatcingPopup.transform.Find("Btn_Matcing_Back").GetComponent<Button>();
        Img_Player = MatcingPopup.transform.Find("Img_Player").GetComponent<Image>();
        Txt_Player_Name = Img_Player.transform.Find("Txt_Player_Name").GetComponent<Text>();
        Img_Enemy = MatcingPopup.transform.Find("Img_Enemy").GetComponent<Image>();
        Txt_Enemy_Name = Img_Enemy.transform.Find("Txt_Enemy_Name").GetComponent<Text>();
        Txt_Count = MatcingPopup.transform.Find("Txt_Count").GetComponent<Text>();

        #endregion

        #region  MsgPopup

        Txt_Msg_Exit = MsgPopup.transform.Find("Txt_Msg_Exit").GetComponent<Text>();
        Txt_Msg_ReMatch = MsgPopup.transform.Find("Txt_Msg_ReMatch").GetComponent<Text>();
        Txt_Msg_Cencel = MsgPopup.transform.Find("Txt_Msg_Cencel").GetComponent<Text>();

        #endregion

        #region  PrivacyPopup

        Btn_Privacy_Back = PrivacyPopup.transform.Find("Btn_Privacy_Back").GetComponent<Button>();
        Toggle_Privacy = PrivacyPopup.transform.Find("Privacy_Bg/Toggle_Privacy").GetComponent<Toggle>();
        Toggle_service = PrivacyPopup.transform.Find("Service_Bg/Toggle_service").GetComponent<Toggle>();
        Btn_Privacy = PrivacyPopup.transform.Find("Btn_Privacy").GetComponent<Button>();
        Btn_service = PrivacyPopup.transform.Find("Btn_service").GetComponent<Button>();
        #endregion

        #region PushGiftPopup

        Anim_Push_Gift = PushGiftPopup.transform.Find("BonusReward/Box_main").GetComponent<Animator>();
        Img_Push_Gift = Anim_Push_Gift.transform.GetComponentsInChildren<Image>(true);
        Txt_Push_Gift_Val = Anim_Push_Gift.transform.GetComponentsInChildren<Text>(true);

        Btn_Push_Gift_Ok = PushGiftPopup.transform.Find("Btn_Gift_Ok").GetComponent<Button>();
        Btn_Push_Gift_Ads = PushGiftPopup.transform.Find("Btn_Gift_Ads").GetComponent<Button>();

        #endregion


        #region TutoMsgPopup

        foreach (var item in TutoMsgPopup.GetComponentsInChildren<Transform>(true))
        {
            if (item.name.Equals("3"))
                Obj_TutoMsg.Add(item.gameObject);

            if (item.name.Equals("4"))
                Obj_TutoMsg.Add(item.gameObject);

            if (item.name.Equals("5"))
                Obj_TutoMsg.Add(item.gameObject);

            if (item.name.Equals("6"))
                Obj_TutoMsg.Add(item.gameObject);

            if (item.name.Equals("7"))
                Obj_TutoMsg.Add(item.gameObject);

            if (item.name.Equals("14"))
                Obj_TutoMsg.Add(item.gameObject);

            if (item.name.Equals("17"))
                Obj_TutoMsg.Add(item.gameObject);

            if (item.name.Equals("25"))
                Obj_TutoMsg.Add(item.gameObject);
        }

        Btn_TutoMsg_Back = TutoMsgPopup.GetComponentsInChildren<Button>(true).ToList();

        #endregion


        #region BestPopup

        Btn_Best_Game_01 = BestPopup.transform.Find("Btn_Game_01").GetComponent<Button>();
        Btn_Best_Game_02 = BestPopup.transform.Find("Btn_Game_02").GetComponent<Button>();
        Btn_Best_Game_03 = BestPopup.transform.Find("Btn_Game_03").GetComponent<Button>();
        Btn_Best_Back = BestPopup.transform.Find("Btn_Best_Back").GetComponent<Button>();
        Btn_Best_Start = BestPopup.transform.Find("Btn_Best_Start").GetComponent<Button>();

        #endregion

        #region SelectPopup

        Btn_8x8 = SelectPopup.transform.Find("Btn_8x8").GetComponent<Button>();
        Btn_9x9 = SelectPopup.transform.Find("Btn_9x9").GetComponent<Button>();
        Btn_Select_Back = SelectPopup.transform.Find("Btn_Select_Back").GetComponent<Button>();

        #endregion

    }

    /// <summary>
    /// 버튼 이벤트 등록
    /// </summary>
    public void AddListener()
    {
        #region Main

        Btn_Classic.onClick.AddListener(() => {
            gameMode = GameMode.Classic;
            PushPopup(SelectPopup);
        });
        Btn_Classic.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Classic"));
        //Btn_Classic.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_rectangle));


        Btn_Multi.onClick.AddListener(() => Multi_Join());
        Btn_Multi.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Multi"));
        Btn_Multi.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_rectangle));

        Btn_Stage.onClick.AddListener(() => Scroll_Control(Tr_Stage_Content, Vector3.zero));
        Btn_Stage.onClick.AddListener(() => PushPopup(StagePopup));
        Btn_Stage.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Stage"));
        Btn_Stage.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_rectangle));

        Btn_Timer.onClick.AddListener(() => {
            gameMode = GameMode.Timer;
            PushPopup(SelectPopup);
        });
        Btn_Timer.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Timer"));
        Btn_Timer.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_rectangle));


        Btn_Setting.onClick.AddListener(() => Set_Google_Txt());
        Btn_Setting.onClick.AddListener(() => PushPopup(SettingPopup));
        Btn_Setting.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Setting"));
        Btn_Setting.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Rank.onClick.AddListener(() => Set_Google_Txt());
        Btn_Rank.onClick.AddListener(() => CloudOnceManager.Instance.Show_Leaderboards());
        Btn_Rank.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Rank"));
        Btn_Rank.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Shop.onClick.AddListener(() => Scroll_Control(ShopManager.Instance.Content, Vector3.zero));
        Btn_Shop.onClick.AddListener(() => PushPopup(ShopPopup));
        Btn_Shop.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Shop"));
        Btn_Shop.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));


        Btn_Google.onClick.AddListener(() => Set_Google_Txt());
        Btn_Google.onClick.AddListener(() => PushPopup(GooglePopup));
        Btn_Google.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Google_Login_Btn"));
        Btn_Google.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Ios.onClick.AddListener(() => Set_Google_Txt());
        Btn_Ios.onClick.AddListener(() => PushPopup(GooglePopup));
        Btn_Ios.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Google_Login_Btn"));
        Btn_Ios.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));



        
        Btn_Gift.onClick.AddListener(() => Get_Gift_Item());


        Btn_AddDia.onClick.AddListener(() => Scroll_Control(ShopManager.Instance.Content, Vector3.zero));
        Btn_AddDia.onClick.AddListener(() => PushPopup(ShopPopup));
        Btn_AddDia.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Crystal"));
        Btn_AddDia.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        Btn_Review.onClick.AddListener(() => Application.OpenURL("https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlegame.multiplayer.tetris"));
        Btn_Review.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));
        Btn_Review.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Review"));

        Btn_Achievements.onClick.AddListener(() => CloudOnceManager.Instance.Show_Achievements());
        //Btn_PushGift.onClick.AddListener(() => Push_Gift());

        #endregion

        #region Game

        Btn_Puase.onClick.AddListener(() => PushPopup(PausePopup));
        Btn_Puase.onClick.AddListener(() => GamePlay.instance.Pause());
        Btn_Puase.onClick.AddListener(() => AdsManager.Instance.BannerShow(false));
        Btn_Puase.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        for (int i = 0; i < Btn_Items.Count; i++)
        {
            Item item = (Item)(i + 1);

            Btn_Items[i].onClick.AddListener(() => GamePlay.instance.Use_Item(item));
            Btn_Tuto_Items[i].onClick.AddListener(() => GamePlay.instance.Use_Item(item));
            Btn_Items[i].onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));
            Btn_Tuto_Items[i].onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        }

        Btn_Item_Multi.onClick.AddListener(() => GamePlay.instance.Use_Item(Item.multi_double));
        Btn_Item_Multi.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        #endregion

        #region PausePopup

        Btn_Pause_Main.onClick.AddListener(() => PopPopup());
        Btn_Pause_Main.onClick.AddListener(() => Go_Main());
        Btn_Pause_Main.onClick.AddListener(() => AdsManager.Instance.ShowInterstitial());
        Btn_Pause_Main.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Pause_Play.onClick.AddListener(() => PopPopup());
        Btn_Pause_Play.onClick.AddListener(() => GamePlay.instance.Pause());
        Btn_Pause_Play.onClick.AddListener(() => AdsManager.Instance.BannerShow(true));
        Btn_Pause_Play.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Pause_Share.onClick.AddListener(() => SocialManager.Instance.Share());
        Btn_Pause_Share.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Pause_Share"));
        Btn_Pause_Share.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Pause_Replay.onClick.AddListener(() => GamePlay.instance.Restart());
        Btn_Pause_Replay.onClick.AddListener(() => PopPopup());
        Btn_Pause_Replay.onClick.AddListener(() => AdsManager.Instance.BannerShow(true));
        Btn_Pause_Replay.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));
        Btn_Pause_Replay.onClick.AddListener(() => Show_Ads = true);
        Btn_Pause_Replay.onClick.AddListener(() => AdsManager.Instance.ShowInterstitial());

        Btn_Pause_Review.onClick.AddListener(() => Application.OpenURL("https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlegame.multiplayer.tetris"));
        Btn_Pause_Review.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Pause_Review"));

        Btn_Pause_Music.onClick.AddListener(() => AudioManager.instance.ToggleMusicStatus());
        Btn_Pause_Music.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Pause_Effect.onClick.AddListener(() => AudioManager.instance.ToggleEffectStatus());
        Btn_Pause_Effect.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        #endregion

        #region Classic_Over_Popup

        Btn_Over_Classic_Rank.onClick.AddListener(() => CloudOnceManager.Instance.Show_Leaderboards());
        Btn_Over_Classic_Rank.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Classic_Result_Rank"));
        Btn_Over_Classic_Rank.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));

        Btn_Over_Classic_Main.onClick.AddListener(() => PopPopup());
        Btn_Over_Classic_Main.onClick.AddListener(() => Go_Main());
        Btn_Over_Classic_Main.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Classic_Result_Exit"));
        Btn_Over_Classic_Main.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));
        Btn_Over_Classic_Main.onClick.AddListener(() => Show_Ads = true);
        Btn_Over_Classic_Main.onClick.AddListener(() => AdsManager.Instance.ShowInterstitial());

        Btn_Over_Classic_Replay.onClick.AddListener(() => PopPopup());
        Btn_Over_Classic_Replay.onClick.AddListener(() => GamePlay.instance.Restart());
        Btn_Over_Classic_Replay.onClick.AddListener(() => AdsManager.Instance.BannerShow(true));
        Btn_Over_Classic_Replay.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Classic_Result_Restart"));
        Btn_Over_Classic_Replay.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));
        Btn_Over_Classic_Replay.onClick.AddListener(() => Show_Ads = true);
        Btn_Over_Classic_Replay.onClick.AddListener(() => AdsManager.Instance.ShowInterstitial());

        Btn_Over_Classic_Review.onClick.AddListener(() => Application.OpenURL("https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlegame.multiplayer.tetris"));
        Btn_Over_Classic_Review.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));
        Btn_Over_Classic_Review.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Classic_Result_Review"));

        Btn_Over_Classic_0.onClick.AddListener(() => Game_continue());

        Btn_Over_Classic_0_Back.onClick.AddListener(() => Classic_End());

        Btn_Over_Stage_List.onClick.AddListener(() => PopPopup());
        Btn_Over_Stage_List.onClick.AddListener(() => Go_Main());
        Btn_Over_Stage_List.onClick.AddListener(() => PushPopup(StagePopup));
        Btn_Over_Stage_List.onClick.AddListener(() => Show_Ads = true);
        Btn_Over_Stage_List.onClick.AddListener(() => AdsManager.Instance.ShowInterstitial());
        Btn_Over_Stage_List.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Stage_Result_List"));
        Btn_Over_Stage_List.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));

        Btn_Over_Stage_Replay.onClick.AddListener(() => PopPopup());
        Btn_Over_Stage_Replay.onClick.AddListener(() => GamePlay.instance.Restart());
        Btn_Over_Stage_Replay.onClick.AddListener(() => AdsManager.Instance.BannerShow(true));
        Btn_Over_Stage_Replay.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Stage_Result_Restart"));
        Btn_Over_Stage_Replay.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));

        Btn_Over_Stage_Next.onClick.AddListener(() => PopPopup());
        Btn_Over_Stage_Next.onClick.AddListener(() => GamePlay.instance.Next_Stage());
        Btn_Over_Stage_Next.onClick.AddListener(() => Set_Stage_Mission_Popop());
        Btn_Over_Stage_Next.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Stage_Result_Next"));
        Btn_Over_Stage_Next.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));

        Btn_Over_Stage_Rank.onClick.AddListener(() => CloudOnceManager.Instance.Show_Leaderboards());
        Btn_Over_Stage_Rank.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Stage_Result_Rank"));
        Btn_Over_Stage_Rank.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));

        Btn_Over_Stage_Review.onClick.AddListener(() => Application.OpenURL("https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlegame.multiplayer.tetris"));
        Btn_Over_Stage_Review.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));
        Btn_Over_Stage_Review.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Stage_Result_Review"));

        Btn_Over_Up_Popup.onClick.AddListener(() => PopupAnimation(true));
        Btn_Over_Up_Popup.onClick.AddListener(() => AdsManager.Instance.BannerShow(true));
        Btn_Over_Up_Popup.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));

        Btn_Over_Down_Popup.onClick.AddListener(() => PopupAnimation(false));
        Btn_Over_Down_Popup.onClick.AddListener(() => AdsManager.Instance.BannerShow(false));
        Btn_Over_Down_Popup.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));

        Btn_Over_Multi_Back.onClick.AddListener(() => PopPopup());
        Btn_Over_Multi_Back.onClick.AddListener(() => Go_Main());
        Btn_Over_Multi_Back.onClick.AddListener(() => PhotonNetwork.Disconnect());
        Btn_Over_Multi_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        Btn_Over_Timer_0.onClick.AddListener(() => Game_continue());

        Btn_Over_Timer_0_Back.onClick.AddListener(() => Timer_End());

        Btn_Over_Timer_1_Rank.onClick.AddListener(() => CloudOnceManager.Instance.Show_Leaderboards());
        Btn_Over_Timer_1_Rank.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Classic_Result_Rank"));
        Btn_Over_Timer_1_Rank.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));

        Btn_Over_Timer_1_Main.onClick.AddListener(() => PopPopup());
        Btn_Over_Timer_1_Main.onClick.AddListener(() => Go_Main());
        Btn_Over_Timer_1_Main.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Classic_Result_Exit"));
        Btn_Over_Timer_1_Main.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));

        Btn_Over_Timer_1_Replay.onClick.AddListener(() => PopPopup());
        Btn_Over_Timer_1_Replay.onClick.AddListener(() => GamePlay.instance.Restart());
        Btn_Over_Timer_1_Replay.onClick.AddListener(() => AdsManager.Instance.BannerShow(true));
        Btn_Over_Timer_1_Replay.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Classic_Result_Restart"));
        Btn_Over_Timer_1_Replay.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));

        Btn_Over_Timer_1_Review.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_octagon));
        Btn_Over_Timer_1_Review.onClick.AddListener(() => Application.OpenURL("https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlegame.multiplayer.tetris"));

        #endregion


        #region StagePopup

        Btn_Stage_Back.onClick.AddListener(() => PopPopup());
        Btn_Stage_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        Btn_Star_Get.onClick.AddListener(() => Get_Star_Item());
        Btn_Star_Get.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Stage_Popup_Star_Get"));
        Btn_Review.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        #endregion

        #region StageMissionPopup

        foreach (var item in Btn_Mission_Back)
        {
            item.onClick.AddListener(() => PopPopup());
            item.onClick.AddListener(() => Check_Back());
            item.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));


        }

        foreach (var item in Btn_Stage_Start)
        {
            item.onClick.AddListener(() => Play_Game(GameMode.Stage));
            //item.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        }

        #endregion

        #region SettingPopup

        Btn_Setting_Back.onClick.AddListener(() => PopPopup());
        Btn_Setting_Back.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Setting_Back"));
        Btn_Setting_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        Btn_Setting_Music.onClick.AddListener(() => AudioManager.instance.ToggleMusicStatus());
        Btn_Setting_Music.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Setting_Effect.onClick.AddListener(() => AudioManager.instance.ToggleEffectStatus());
        Btn_Setting_Effect.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Setting_Save.onClick.AddListener(() => SocialManager.Instance.ShowSaveSelectUI());
        Btn_Setting_Save.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Setting_Google_Save"));
        Btn_Setting_Save.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Setting_Load.onClick.AddListener(() => SocialManager.Instance.ShowLoadSelectUI());
        Btn_Setting_Load.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Setting_Google_Load"));
        Btn_Setting_Load.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Setting_Share.onClick.AddListener(() => SocialManager.Instance.Share());
        Btn_Setting_Share.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Setting_Share"));
        Btn_Setting_Share.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Setting_Review.onClick.AddListener(() => PushPopup(ReviewPopup));
        Btn_Setting_Review.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Setting_Review"));
        Btn_Setting_Review.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Setting_Help.onClick.AddListener(() => PushPopup(HelpPopup));
        Btn_Setting_Help.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Setting_Help"));
        Btn_Setting_Help.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Setting_Exit.onClick.AddListener(() => PushPopup(ExitPopup));
        Btn_Setting_Exit.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Setting_Exit"));
        Btn_Setting_Exit.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Setting_Google_Login.onClick.AddListener(() => Set_Google_Txt());
        Btn_Setting_Google_Login.onClick.AddListener(() => PushPopup(GooglePopup));
        Btn_Setting_Google_Login.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Setting_Google_Login"));
        Btn_Setting_Google_Login.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Setting_Google_Logout.onClick.AddListener(() => Set_Google_Txt());
        Btn_Setting_Google_Logout.onClick.AddListener(() => PushPopup(GooglePopup));
        Btn_Setting_Google_Logout.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Setting_Google_Logout"));
        Btn_Setting_Google_Logout.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        Btn_Setting_Language.onClick.AddListener(() => PushPopup(LanguagePopup));
        Btn_Setting_Language.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Setting_Language"));
        Btn_Setting_Language.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_circle));

        #endregion

        #region ExitPopup

        Btn_Exit_Ok.onClick.AddListener(() => App_Quit());
        Btn_Exit_Ok.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        Btn_Exit_No.onClick.AddListener(() => PopPopup());
        Btn_Exit_No.onClick.AddListener(() => AdsManager.Instance.BannerShow(true));

        Btn_Exit_No.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        #endregion

        #region ReviewPopup

        Btn_Review_Ok.onClick.AddListener(() => Application.OpenURL("https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlegame.multiplayer.tetris"));
        Btn_Review_Ok.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));
        Btn_Review_Ok.onClick.AddListener(() => PlayerPrefs.SetInt("REVIEW", 1));
        Btn_Review_Ok.onClick.AddListener(() => PopPopup());
        Btn_Review_Ok.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("ReviewPopup_Review"));
        Btn_Review_Ok.onClick.AddListener(() => DataManager.Instance.state_Player.Check_Review = true);
        Btn_Review_Ok.onClick.AddListener(() => DataManager.Instance.Save_Player_Data());


        Btn_Review_Back.onClick.AddListener(() => PopPopup());
        Btn_Review_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        #endregion

        #region ShopPopup

        Btn_Shop_Back.onClick.AddListener(() => PopPopup());
        Btn_Shop_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));
        Btn_Shop_Back.onClick.AddListener(() => Shop_Banner());


        #endregion

        #region HelpPopup

        Btn_Help_Back.onClick.AddListener(() => PopPopup());
        Btn_Help_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        Btn_Help_Item.onClick.AddListener(() => PushPopup(HelpItemPopup));
        Btn_Help_Item.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        Btn_Help_Block.onClick.AddListener(() => PushPopup(HelpBlockPopup));
        Btn_Help_Block.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        Btn_Help_Mode.onClick.AddListener(() => PushPopup(HelpModePopup));
        Btn_Help_Mode.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        Btn_Help_Visit.onClick.AddListener(() => PushPopup(HelpVisitPopup));
        Btn_Help_Visit.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        Btn_HelpItem_Back.onClick.AddListener(() => PopPopup());
        Btn_HelpItem_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        Btn_HelpBlock_Back.onClick.AddListener(() => PopPopup());
        Btn_HelpBlock_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        Btn_HelpMode_Back.onClick.AddListener(() => PopPopup());
        Btn_HelpMode_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        Btn_HelpVisit_Back.onClick.AddListener(() => PopPopup());
        Btn_HelpVisit_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        Btn_Help_Service.onClick.AddListener(() => Application.OpenURL("https://sites.google.com/site/breaktieme/terms-of-service"));
        Btn_Help_Service.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_soft));

        if (Application.systemLanguage.Equals(SystemLanguage.Korean))
        {
            Btn_Help_Privacy.onClick.AddListener(() => Application.OpenURL("https://sites.google.com/site/breaktieme/privacy-policy_kr"));
            Btn_Help_Privacy.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_soft));

        }
        else
        {
            Btn_Help_Privacy.onClick.AddListener(() => Application.OpenURL("https://sites.google.com/site/breaktieme/privacy-policy"));
            Btn_Help_Privacy.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_soft));

        }

        #endregion

        #region GooglePopup

        Btn_Google_Back.onClick.AddListener(() => PopPopup());
        Btn_Google_Back.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Google_Close"));
        Btn_Google_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        Btn_Google_Login.onClick.AddListener(() => CloudOnceManager.Instance.Login());
        Btn_Google_Login.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Google_Login"));
        Btn_Google_Login.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        Btn_Google_Logout.onClick.AddListener(() => CloudOnceManager.Instance.Logout());
        Btn_Google_Logout.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Google_Logout"));
        Btn_Google_Logout.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));


        #endregion

        #region GiftPopup

        Btn_Gift_Ok.onClick.AddListener(() => Get_Gift(false));
        Btn_Gift_Ok.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Gift_Get"));
        Btn_Gift_Ok.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

#if UNITY_EDITOR

        Btn_Gift_Ads.onClick.AddListener(() => Get_Gift(true));

#else
        Btn_Gift_Ads.onClick.AddListener(() => AdsManager.Instance.ShowRewardedAd());

#endif

        Btn_Gift_Ads.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Main_Gift_Ads"));
        Btn_Gift_Ads.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        #endregion

        #region DailyPopup

        Btn_Daily_Back.onClick.AddListener(() => PopPopup());
        Btn_Daily_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        foreach (var item in Btn_Day)
        {
            item.onClick.AddListener(() => PopPopup());

        }

        #endregion

        #region LanguagePopup

        Btn_Language_Back.onClick.AddListener(() => PopPopup());
        Btn_Language_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        for (int i = 0; i < Btn_Languages.Length; i++)
        {
            SystemLanguage lng = (SystemLanguage)langeuae[i];
            Btn_Languages[i].onClick.AddListener(() => PopPopup());
            Btn_Languages[i].onClick.AddListener(() => LanguageManager.Instance.Change_language(lng));
            Btn_Languages[i].onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_soft));

        }

        #endregion



        #region  MultiPopup

        Btn_Multi_Back.onClick.AddListener(() => PopPopup());
        Btn_Multi_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));

        Btn_Multi_Dia_1.onClick.AddListener(() => PhotonManager.Instance.Multi_Start(Multi_Room.Dia_1));
        Btn_Multi_Dia_1.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        Btn_Multi_Dia_5.onClick.AddListener(() => PhotonManager.Instance.Multi_Start(Multi_Room.Dia_5));
        Btn_Multi_Dia_5.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        Btn_Multi_Dia_10.onClick.AddListener(() => PhotonManager.Instance.Multi_Start(Multi_Room.Dia_10));
        Btn_Multi_Dia_10.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        #endregion

        #region  MatcingPopup

        Btn_Matcing_Back.onClick.AddListener(() => PopPopup());
        Btn_Matcing_Back.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_close));


        #endregion

        #region  PrivacyPopup

        Btn_Privacy_Back.onClick.AddListener(() => App_Quit());
        Toggle_Privacy.onValueChanged.AddListener(delegate
        {
            Toggle_Check(Toggle_Privacy);
        });

        Toggle_service.onValueChanged.AddListener(delegate
        {
            Toggle_Check(Toggle_service);
        });

        Btn_Privacy.onClick.AddListener(() => Application.OpenURL("https://sites.google.com/site/breaktieme/privacy-policy_kr"));
        Btn_Privacy.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_soft));

        Btn_service.onClick.AddListener(() => Application.OpenURL("https://sites.google.com/site/breaktieme/terms-of-service"));
        Btn_service.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_soft));

        #endregion

        #region PushGiftPopup

        Anim_Push_Gift = PushGiftPopup.transform.Find("BonusReward/Box_main").GetComponent<Animator>();

        Btn_Push_Gift_Ok.onClick.AddListener(() => Get_Push_Gift(false));
        Btn_Push_Gift_Ok.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Push_Gift_Get"));
        Btn_Push_Gift_Ok.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

#if UNITY_EDITOR

        Btn_Push_Gift_Ads.onClick.AddListener(() => Get_Push_Gift(true));

#else
        Btn_Push_Gift_Ads.onClick.AddListener(() => AdsManager.Instance.ShowRewardedAd());

#endif

        Btn_Push_Gift_Ads.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Push_Gift_Ads"));
        Btn_Push_Gift_Ads.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_square));

        #endregion

        #region TutoMsgPopup
        Debug.Log("Btn_TutoMsg_Back " + Btn_TutoMsg_Back.Count);
        foreach (var item in Btn_TutoMsg_Back)
        {
            item.onClick.AddListener(() => PopPopup());
            if (item.transform.parent.name.Equals("14"))
            {
                item.onClick.AddListener(() => StartCoroutine("Co_Misstion_Timer"));

            }
        }

        #endregion

        #region BestPopup

        Btn_Best_Game_01.onClick.AddListener(() => Application.OpenURL("https://play.google.com/store/apps/details?id=com.block.puzzle.puzzlego.number.puzzledom.Kingdom"));
        Btn_Best_Game_02.onClick.AddListener(() => Application.OpenURL("https://play.google.com/store/apps/details?id=com.jumpball.colorballjump.bouncingball"));
        Btn_Best_Game_03.onClick.AddListener(() => Application.OpenURL("https://play.google.com/store/apps/details?id=com.balls.glow.bricks.breaker.brounce.swipebrick.puzzle"));

        Btn_Best_Back.onClick.AddListener(() =>
        {
            PopPopup();
            Check_Daily();
        });

        Btn_Best_Start.onClick.AddListener(() =>
        {
            PopPopup();
            Check_Daily();
        });

        #endregion


        #region SelectPopup

        Btn_8x8.onClick.AddListener(() => Select_Board(8));
        Btn_9x9.onClick.AddListener(() => Select_Board(9));
        Btn_Select_Back.onClick.AddListener(() => PopPopup());

        #endregion

    }

    public void Shop_Banner()
    {
        if (game_Stat.Equals(Game_Stat.End))
        {
            AdsManager.Instance.BannerShow(false);
        }
        else
        {
            AdsManager.Instance.BannerShow(true);
        }
    }

    /// <summary>
    /// 초반 UI 세팅
    /// </summary>
    public void SetUi()
    {
        if (stage_Infos.Count == 0)
        {
            Set_Stage_Block();
        }
        else
        {
            Set_Stage_Info();
        }

        Btn_Help_Service.gameObject.SetActive(Application.systemLanguage.Equals(SystemLanguage.Korean));

        Set_Total_Star_Ui();
        Set_All_Txt();
        Set_Gift_Time();
        Txt_Setting_Ver.text = "Ver " + Application.version;

        Btn_Google.gameObject.SetActive(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor);
        Btn_Ios.gameObject.SetActive(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor);
    }

    /// <summary>
    /// 종료
    /// </summary>
    public void App_Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// 메인으로
    /// </summary>
    public void Go_Main()
    {
        PlayerPrefs.SetInt("size", 8);

        AudioManager.instance.Play_Music_Sound(Music_Sound.title_bg);

        TutoPopup.SetActive(false);

        string Mode = "";

        switch (GamePlay.instance.gameMode)
        {
            case GameMode.Classic:
                Mode = "Classic";
                break;
            case GameMode.Stage:
                Mode = "Stage";
                break;
            case GameMode.Multi:
                Mode = "Multi";

                break;
            case GameMode.Timer:
                Mode = "Timer";

                break;
            default:
                break;
        }

        FireBaseManager.Instance.LogEvent(Mode + "_Pause_Exit");

        AdsManager.Instance.BannerShow(true);

        game_Stat = Game_Stat.Main;

        GamePlay.instance.GameReset();

        Main.SetActive(true);
        Game.SetActive(false);

        foreach (var item in Obj_TutoMsg)
        {
            if (item.activeSelf)
                PopPopup();

            item.SetActive(false);
        }

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (!isBackKey)
                StartCoroutine("BackKey");
        }
    }

    /// <summary>
    /// back key
    /// </summary>
    /// <returns></returns>
    IEnumerator BackKey()
    {
        isBackKey = true;
        Debug.Log(stack_Popup.Count);

        if (stack_Popup.Count > 0)
        {
            switch (game_Stat)
            {
                case Game_Stat.Main:
                    if (PrivacyPopup.activeSelf)
                    {
                        App_Quit();
                    }
                    else if (GiftPopup.activeSelf)
                    {
                        Get_Gift(false);
                    }
                    else if (PushGiftPopup.activeSelf)
                    {
                        Get_Push_Gift(false);
                    }
                    else
                    {
                        PopPopup();
                        AdsManager.Instance.BannerShow(true);

                    }

                    break;
                case Game_Stat.Game:

                    Debug.Log("Obj_TutoMsg " + Obj_TutoMsg[1].name + "   " + Obj_TutoMsg[1].activeSelf);

                    if (Obj_TutoMsg[1].activeSelf)
                    {
                        StartCoroutine("Co_Misstion_Timer");

                    }
                    else if (ShopPopup.activeSelf)
                    {
                        if (GamePlay.instance.Mission_Block_Num.Contains(10) || GamePlay.instance.gameMode.Equals(GameMode.Timer))
                            StartCoroutine("Co_Misstion_Timer");
                    }
                    else if (!TutoMsgPopup.activeSelf)
                    {

                        GamePlay.instance.Pause();
                        AdsManager.Instance.BannerShow(true);
                    }

                    PopPopup();



                    break;
                case Game_Stat.End:

                    PopPopup();

                    switch (GamePlay.instance.gameMode)
                    {
                        case GameMode.Classic:
                            Go_Main();

                            break;
                        case GameMode.Stage:

                            Go_Main();
                            PushPopup(StagePopup);
                            break;
                        case GameMode.Multi:
                            Go_Main();

                            break;
                        case GameMode.Timer:
                            if (Over_Timer_0.activeSelf)
                            {
                                Timer_End();
                                PushPopup(Over_Popup);
                            }
                            else
                                Go_Main();
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
        else if (stack_Popup.Count == 0)
        {
            PopPopup();

            switch (game_Stat)
            {
                case Game_Stat.Main:
                    //종료창
                    PushPopup(ExitPopup);
                    AdsManager.Instance.BannerShow(false);

                    break;
                case Game_Stat.Game:

                    switch (GamePlay.instance.gameMode)
                    {
                        case GameMode.Classic:
                        case GameMode.Timer:
                        case GameMode.Stage:
                            PushPopup(PausePopup);
                            GamePlay.instance.Pause();
                            AdsManager.Instance.BannerShow(false);
                            break;

                        default:
                            break;
                    }


                    break;

                default:
                    break;
            }

        }


        yield return new WaitForSeconds(0.3f);

        isBackKey = false;
    }


    /// <summary>
    /// 팝업 열기
    /// </summary>
    /// <param name="popup"></param>
    public void PushPopup(GameObject popup)
    {
        if (popup.Equals(GooglePopup) && popup.Equals(Over_Popup) && !Check_Network())
            return;

        if (!popup.activeSelf)
        {
            AudioManager.instance.Play_Effect_Sound(Effect_Sound.popup_open);
            stack_Popup.Push(popup);
            popup.SetActive(true);
        }

    }

    /// <summary>
    /// 팝업 닫기
    /// </summary>
    public void PopPopup()
    {
        Debug.Log(stack_Popup.Count);

        if (stack_Popup.Count >= 1)
        {

            GameObject pop = stack_Popup.Pop();
            pop.SetActive(false);

            Debug.Log(pop.name);

            if (game_Stat.Equals(Game_Stat.Game) && pop.Equals(ShopPopup))
            {
                if (GamePlay.instance.Mission_Block_Num.Contains(10) || GamePlay.instance.gameMode.Equals(GameMode.Timer))
                    StartCoroutine("Co_Misstion_Timer");
            }

        }

    }


    #region Stage_Popup

    /// <summary>
    /// 스테이지 팝업 위치 세팅
    /// </summary>
    /// <param name="content"></param>
    /// <param name="pos"></param>
    public void Scroll_Control(Transform content, Vector3 pos)
    {
        if (content.Equals(Tr_Stage_Content))
        {
            float y = (DataManager.Instance.state_Player.clear_Stage.Count / 4) * (content.GetComponent<GridLayoutGroup>().cellSize.y +
                content.GetComponent<GridLayoutGroup>().spacing.y);

            pos = new Vector3(0, y, 0);

        }

        content.localPosition = pos;
    }

    /// <summary>
    /// 스테이지 블럭 세팅
    /// </summary>
    public void Set_Stage_Block()
    {
        stage_Infos = Tr_Stage_Content.GetComponentsInChildren<Stage_Info>().ToList();

        Debug.Log("    " + stage_Infos.Count);
        for (int i = 0; i < stage_Infos.Count; i++)
        {
            if (i < DataManager.Instance.stage_data.Count)
            {
                stage_Infos[i].Set_Stage(i + 1);
                int stage_Num = i;
                stage_Infos[i].GetComponent<Button>().onClick.AddListener(() => Set_Stage_Mission_Popop(stage_Num));
            }
            else
            {
                stage_Infos[i].gameObject.SetActive(false);
            }

        }
    }


    /// <summary>
    /// 스테이지 정보 세팅
    /// </summary>
    public void Set_Stage_Info()
    {
        foreach (var item in stage_Infos)
        {
            item.Set_Lock();
            item.Set_Star();
        }
    }


    /// <summary>
    /// 총 별 갯수 세팅
    /// </summary>
    public void Set_Total_Star_Ui()
    {
        int Total_Star = DataManager.Instance.state_Player.clear_Stage.Sum(s => s.Stage_Star);
        int Get_Star = DataManager.Instance.state_Player.starGift * 30;
        Txt_Star_Val.text =
        string.Format("<color=#ffffff>{0}</color><color=#FFA930>/{1}</color>", Total_Star, Get_Star);

        Btn_Star_Get.interactable = Total_Star >= Get_Star;
        Btn_Star_Get.targetGraphic.color = new Color(1, 1, 1, Total_Star >= Get_Star ? 1 : 0.5f);

    }


    /// <summary>
    /// 별 보상 받기
    /// </summary>
    public void Get_Star_Item()
    {
        DataManager.Instance.state_Player.starGift += 1;

        giftItem.Clear();
        List<int> star_item = new List<int>();
        star_item.Add(0);
        star_item.Add(10);
        giftItem.Add(star_item);

        Set_Total_Star_Ui();

        Set_Gift_Popup();

        PushPopup(GiftPopup);

    }

    #endregion

    #region Game

    GameMode gameMode = GameMode.Classic;

    public void Select_Board(int size)
    {
        PlayerPrefs.SetInt("size", size);
        PopPopup();
        Play_Game(gameMode);
    }

    /// <summary>
    /// 게임 플레이
    /// </summary>
    /// <param name="gameMode"></param>
    public void Play_Game(GameMode gameMode)
    {
        Conti = 0;

        StagePopup.SetActive(false);
        StageMissionPopup.SetActive(false);

        stack_Popup.Clear();

        game_Stat = Game_Stat.Game;

        Main.SetActive(false);
        Game.SetActive(true);


        Game.GetComponent<GamePlay>().gameMode = gameMode;
        Game.GetComponent<GamePlay>().Start_Game();

        switch (gameMode)
        {
            case GameMode.Classic:

                break;
            case GameMode.Stage:


                foreach (var item in Obj_TutoMsg)
                {
                    item.SetActive(false);
                }

                Clear_Stage_Info Find_Stage_Info = DataManager.Instance.state_Player.clear_Stage.Find(x => x.Stage_Id.Equals(GamePlay.instance.Play_Stage_Num));

                switch (GamePlay.instance.Play_Stage_Num)
                {
                    case 2:
                        if (Find_Stage_Info == null)
                        {
                            Obj_TutoMsg.Find(x => x.name.Equals((GamePlay.instance.Play_Stage_Num + 1).ToString())).SetActive(true);
                            PushPopup(TutoMsgPopup);
                        }

                        break;
                    case 3:
                        if (Find_Stage_Info == null)
                        {
                            Obj_TutoMsg.Find(x => x.name.Equals((GamePlay.instance.Play_Stage_Num + 1).ToString())).SetActive(true);
                            PushPopup(TutoMsgPopup);
                        }

                        break;
                    case 4:
                        if (Find_Stage_Info == null)
                        {
                            Obj_TutoMsg.Find(x => x.name.Equals((GamePlay.instance.Play_Stage_Num + 1).ToString())).SetActive(true);
                            PushPopup(TutoMsgPopup);
                        }

                        break;
                    case 5:
                        if (Find_Stage_Info == null)
                        {
                            Obj_TutoMsg.Find(x => x.name.Equals((GamePlay.instance.Play_Stage_Num + 1).ToString())).SetActive(true);
                            PushPopup(TutoMsgPopup);
                        }

                        break;
                    case 6:
                        if (Find_Stage_Info == null)
                        {
                            Obj_TutoMsg.Find(x => x.name.Equals((GamePlay.instance.Play_Stage_Num + 1).ToString())).SetActive(true);
                            PushPopup(TutoMsgPopup);
                        }

                        break;
                    case 13:
                        if (Find_Stage_Info == null)
                        {
                            StopCoroutine("Co_Misstion_Timer");
                            Obj_TutoMsg.Find(x => x.name.Equals((GamePlay.instance.Play_Stage_Num + 1).ToString())).SetActive(true);
                            PushPopup(TutoMsgPopup);
                        }
                        break;
                    case 16:
                        if (Find_Stage_Info == null)
                        {
                            Obj_TutoMsg.Find(x => x.name.Equals((GamePlay.instance.Play_Stage_Num + 1).ToString())).SetActive(true);
                            PushPopup(TutoMsgPopup);
                        }
                        break;
                    case 24:
                        if (Find_Stage_Info == null)
                        {
                            Obj_TutoMsg.Find(x => x.name.Equals((GamePlay.instance.Play_Stage_Num + 1).ToString())).SetActive(true);
                            PushPopup(TutoMsgPopup);
                        }

                        break;
                    default:
                        break;
                }

                break;
            case GameMode.Multi:

                break;
            case GameMode.Timer:

                break;
            default:
                break;
        }


    }

    public void Spawn_Anim()
    {

        int Start_Block = GameBoardGenerator.instance.TotalColumns - 1;

        List<int> Spawn_block = new List<int>();


        for (int i = 0; i <= Start_Block * 2; i++)
        {
            List<int> Spawn = new List<int>();

            foreach (var item in Spawn_block)
            {
                Spawn.Add(item);
            }

            Spawn_block.Clear();

            int Miu = i <= 7 ? 1 : -1;

            for (int j = 0; j < Spawn.Count + Miu; j++)
            {
                if (i == 0)
                {
                    Spawn_block.Add(Start_Block);
                }
                else if (i <= 7)
                {
                    if (j >= Spawn.Count)
                        Spawn_block.Add(Spawn[Spawn.Count - 1] + 8);
                    else
                        Spawn_block.Add(Spawn[j] - 1);
                }
                else
                {
                    Spawn_block.Add(Spawn[j] + 8);
                }
            }

            foreach (var item in Spawn_block)
            {
                Debug.Log("item = " + item);

            }

            Debug.Log("-----------------------------------------");

        }
    }
    /// <summary>
    /// 게임 플레이 UI 세팅
    /// </summary>
    public void Set_PlayUi()
    {

        GameMode gameMode = GamePlay.instance.gameMode;

        Top_Classic.SetActive(gameMode.Equals(GameMode.Classic));
        Top_Stage.SetActive(gameMode.Equals(GameMode.Stage));
        Top_Multi.SetActive(gameMode.Equals(GameMode.Multi));
        Top_Timer.SetActive(gameMode.Equals(GameMode.Timer));


        clearScore = 0;
        ScoreManager.Instance.Set_Score();

        foreach (var item in Img_Stars)
        {
            item.gameObject.SetActive(false);
        }


        Btn_Items[0].transform.parent.gameObject.SetActive(false);
        Btn_Tuto_Items[0].transform.parent.gameObject.SetActive(false);
        Btn_Item_Multi.transform.gameObject.SetActive(false);
        Txt_Multi_Item_Free.SetActive(false);
        Img_Multi_Dia.SetActive(false);
        Txt_Multi_Start_Time.gameObject.SetActive(false);
        Btn_Multi_ReStart.gameObject.SetActive(false);
        Btn_Puase.gameObject.SetActive(false);
        Txt_Multi_Sec.gameObject.SetActive(false);
        Btn_Pause_Replay.gameObject.SetActive(false);
        Top_Mission_Time.SetActive(false);

        StopCoroutine("Co_Misstion_Timer");

        switch (gameMode)
        {
            case GameMode.Classic:

                Btn_Items[0].transform.parent.gameObject.SetActive(true);
                Btn_Puase.gameObject.SetActive(true);
                Btn_Pause_Replay.gameObject.SetActive(true);

                break;
            case GameMode.Stage:
                Btn_Puase.gameObject.SetActive(true);
                Btn_Pause_Replay.gameObject.SetActive(true);

                int playNum = GamePlay.instance.Play_Stage_Num;

                if (playNum >= 3 && playNum < 7)
                {
                    Btn_Tuto_Items[0].transform.parent.gameObject.SetActive(true);

                    for (int i = 0; i < Btn_Tuto_Items.Count; i++)
                    {
                        Btn_Tuto_Items[i].gameObject.SetActive(i == playNum - 3);
                    }

                }
                else
                    Btn_Items[0].transform.parent.gameObject.SetActive(true);

                //Txt_Top_Misison.text = (playNum + 1).ToString();

                Dictionary<string, object> stage_Info = DataManager.Instance.stage_data[GamePlay.instance.Play_Stage_Num];

                Top_Mission_Score.SetActive(true);
                Top_Mission_Block.gameObject.SetActive(false);


                Clear_Stage_Info Find_Stage_Info = DataManager.Instance.state_Player.clear_Stage.Find(x => x.Stage_Id.Equals(GamePlay.instance.Play_Stage_Num));

                int stage_Star = Find_Stage_Info != null ? Find_Stage_Info.Stage_Star : 0;

                for (int i = 0; i < Top_Mission_Boxs.Count; i++)
                {
                    int missionNum = stage_Info["mission_" + (i + 1)].ToString().TryParseInt();
                    int clearNum = (int)stage_Info["clear_" + (i + 1)];

                    if (GamePlay.instance.Play_Stage_Num >= 7)
                    {
                        switch (stage_Star)
                        {
                            case 0:
                                break;
                            case 1:
                                clearNum += Mathf.RoundToInt(clearNum * 0.3f);
                                break;
                            case 2:
                            case 3:

                                clearNum += Mathf.RoundToInt(clearNum * 0.7f);

                                break;
                            default:
                                break;
                        }
                    }

                    switch (missionNum)
                    {
                        case 0:
                            Top_Mission_Boxs[i].gameObject.SetActive(false);
                            if (clearScore == 0)
                            {
                                Txt_Top_Mission_Score.text = ScoreManager.Instance.GetScore().ToString();

                            }
                            break;
                        case 1:
                            Debug.Log(missionNum + "    " + clearNum);
                            Txt_Top_Mission_Score.text = string.Format("{0} / {1}", ScoreManager.Instance.GetScore(), clearNum);
                            clearScore = clearNum;
                            break;
                        case 10:
                            StopCoroutine("Co_Misstion_Timer");

                            Top_Mission_Time.gameObject.SetActive(true);
                            Slider_Top_Mission_Time.maxValue = clearNum;
                            Slider_Top_Mission_Time.value = clearNum;
                            StartCoroutine("Co_Misstion_Timer");
                            break;
                        default:
                            Top_Mission_Block.gameObject.SetActive(true);

                            Top_Mission_Boxs[i].GetComponent<Image>().sprite = Sp_Blocks[missionNum - 2];
                            Top_Mission_Boxs[i].transform.Find("Mission_Val").GetComponent<Text>().text = clearNum.ToString();
                            Top_Mission_Boxs[i].gameObject.SetActive(true);
                            break;

                    }

                }

                break;

            case GameMode.Multi:
                Txt_Multi_Start_Time.text = "3";

                Txt_Multi_Start_Time.gameObject.SetActive(true);
                Btn_Puase.gameObject.SetActive(false);

                Btn_Item_Multi.gameObject.SetActive(true);
                Txt_Multi_Item_Free.SetActive(true);

                break;
            case GameMode.Timer:

                Btn_Items[0].transform.parent.gameObject.SetActive(true);
                Btn_Puase.gameObject.SetActive(true);
                Btn_Pause_Replay.gameObject.SetActive(true);

                StopCoroutine("Co_Misstion_Timer");

                Slider_Top_Mission_Time.maxValue = 60;
                Slider_Top_Mission_Time.value = 60;
                Top_Mission_Time.gameObject.SetActive(true);

                StartCoroutine("Co_Misstion_Timer");

                break;
            default:
                break;
        }
    }



    /// <summary>
    /// 스테이지 미션 타이머
    /// </summary>
    /// <returns></returns>
    IEnumerator Co_Misstion_Timer()
    {
        bool isclock = false;

        LeanTween.cancel(myTween, true);
        Top_Mission_Clock.rotation = Quaternion.Euler(0, 0, 0);

        while (Slider_Top_Mission_Time.value > 0)
        {
            if (!GamePlay.instance.isPause)
            {
                Slider_Top_Mission_Time.value -= 0.1f;

            }

            if (!isclock && Slider_Top_Mission_Time.value <= Slider_Top_Mission_Time.maxValue / 2.0f)
            {
                isclock = true;

                LeanTween.rotateZ(Top_Mission_Clock.gameObject, -50.0f, 0.2f).setOnComplete(() =>
                {
                    myTween = LeanTween.rotateZ(Top_Mission_Clock.gameObject, 50.0f, 0.2f).setLoopPingPong().id;

                });
            }

            yield return new WaitForSeconds(0.1f);

        }

        if (game_Stat != Game_Stat.End)
        {
            GamePlay.instance.end_Stat = End_Stat.timer;

            Set_GameOver_UI();

        }

        Debug.Log("타이머 게임오버");


    }

    public void AddSlider(int val)
    {
        StartCoroutine("Co_AddSlider", val);
    }

    IEnumerator Co_AddSlider(int val)
    {
        StopCoroutine("Co_Misstion_Timer");

        for (int i = 0; i < val; i++)
        {
            if (Slider_Top_Mission_Time.value < 60)
            {
                Slider_Top_Mission_Time.value += 1f;

                yield return new WaitForSeconds(0.001f);
            }
            else
                break;

        }

        StartCoroutine("Co_Misstion_Timer");
        game_Stat = Game_Stat.Game;

    }


    public void Game_continue()
    {

        Debug.Log(GamePlay.instance.blockGrid.Count);

        if (DataManager.Instance.Check_Crystal(Conti_price[Conti]))
        {
            string Mode = "";

            switch (GamePlay.instance.gameMode)
            {
                case GameMode.Classic:
                    Mode = "Classic";
                    break;
                case GameMode.Stage:
                    Mode = "Stage";
                    break;
                case GameMode.Multi:
                    Mode = "Multi";

                    break;
                case GameMode.Timer:
                    Mode = "Timer";

                    break;
                default:
                    break;
            }


            FireBaseManager.Instance.LogEvent(Mode + "_Continue");

            AdsManager.Instance.BannerShow(true);

            AudioManager.instance.musicSource.UnPause();

            DataManager.Instance.state_Player.crystal -= Conti_price[Conti];

            DataManager.Instance.Save_Player_Data();

            UIManager.Instance.Set_Item_Txt(Item.crystal);

            foreach (var item in GamePlay.instance.blockGrid)
            {
                if (item.block_Type.Equals(Block_Type.boom))
                {
                    item.Init();
                }
                else
                {
                    item.Timer_continue();

                }
            }
            int row = PlayerPrefs.GetInt("size", 8);

            int st = row * 2 + 2;
            //8 *2 +2
            //row
            Debug.Log(Math.Ceiling(row /2.0f));
            //반올림 4개 5개
            for (int i = 0; i < Math.Ceiling(row / 2.0f); i++)
            {
                for (int j = 0; j < Math.Ceiling(row / 2.0f); j++)
                {

                    GamePlay.instance.blockGrid[st + j].ClearBlock(GamePlay.instance.blockGrid[st + j].colorID);

                }
                st += row;

            }

            PopPopup();

            switch (GamePlay.instance.gameMode)
            {
                case GameMode.Classic:
                    UIManager.Instance.game_Stat = Game_Stat.Game;
                    break;
                case GameMode.Stage:
                    break;
                case GameMode.Multi:
                    break;
                case GameMode.Timer:
                    AddSlider(30);

                    break;
                default:
                    break;
            }

            Conti = Conti + 1 < Conti_price.Length ? Conti += 1 : Conti;
            BlockShapeSpawner.Instance.CheckOnBoardShapeStatus();

        }

    }

    /// <summary>
    /// 게임 오버 UI 세팅
    /// </summary>
    /// <param name="isClear"></param>
    public void Set_GameOver_UI(bool isClear = false)
    {

        AudioManager.instance.musicSource.Pause();

        game_Stat = Game_Stat.End;


        //AdsManager.Instance.interstitialShow();
        /////////////

        StopCoroutine("Co_Misstion_Timer");

        GameMode gameMode = GamePlay.instance.gameMode;

        Over_Classic_1.SetActive(gameMode.Equals(GameMode.Classic));
        Over_Stage.SetActive(gameMode.Equals(GameMode.Stage));
        Over_Multi.SetActive(gameMode.Equals(GameMode.Multi));
        Over_Timer_0.SetActive(false);
        Over_Timer_1.SetActive(false);

        CloudOnceManager.Instance.Report_Achievements();

        switch (gameMode)
        {
            case GameMode.Classic:
                AdsManager.Instance.BannerShow(false);

                Txt_Over_Classic_0_Price.text = Conti_price[Conti].ToString();

                Over_Classic_0.SetActive(true);
                Over_Classic_1.SetActive(false);

                PushPopup(Over_Popup);
                CloudOnceManager.Instance.Report_Leaderboard(gameMode, ScoreManager.Instance.GetScore());


                Txt_Over_Classic_Score.text = ScoreManager.Instance.GetScore().ToString();
                Txt_Over_Classic_Best.text = DataManager.Instance.state_Player.Classic.ToString();


                //if (!DataManager.Instance.state_Player.Check_Review)
                //{
                //    if (PlayerPrefs.GetInt("Classic_Review", 0) >= 3)
                //    {
                //        PlayerPrefs.SetInt("Classic_Review", 0);
                //        UIManager.Instance.PushPopup(ReviewPopup);
                //    }
                //    else
                //    {
                //        PlayerPrefs.SetInt("Classic_Review", PlayerPrefs.GetInt("Classic_Review", 0) + 1);
                //    }
                //}


                break;
            case GameMode.Stage:
                AdsManager.Instance.BannerShow(false);

                Debug.Log(isClear);
                Btn_Over_Stage_Next.gameObject.SetActive(GamePlay.instance.Play_Stage_Num < (DataManager.Instance.stage_data.Count - 1) && isClear);
                Btn_Over_Stage_Review.gameObject.SetActive(!isClear);
                Txt_Over_Stage_Succece.gameObject.SetActive(false);
                Txt_Over_Stage_Fail.gameObject.SetActive(false);
                Img_Over_Stage_Succece.gameObject.SetActive(isClear);
                Img_Over_Stage_Fail.gameObject.SetActive(!isClear);

                PushPopup(Over_Popup);
                PopupAnimation(false);

                int Total_Star = DataManager.Instance.state_Player.clear_Stage.Sum(s => s.Stage_Star);

                if (isClear)
                {
                    int Star = DataManager.Instance.state_Player.clear_Stage.Find(x => x.Stage_Id.Equals(GamePlay.instance.Play_Stage_Num)).Stage_Star;

                    CloudOnceManager.Instance.Report_Leaderboard(gameMode, Total_Star);

                    StartCoroutine("Co_Over_Star", Star);
                    FireBaseManager.Instance.LogEvent("Stage_Clar", "Stage", GamePlay.instance.Play_Stage_Num);

                }
                else
                {
                    FireBaseManager.Instance.LogEvent("Stage_Fail", "Stage", GamePlay.instance.Play_Stage_Num);

                }

                if (PlayerPrefs.GetInt("REVIEW", 0).Equals(0) && (GamePlay.instance.Play_Stage_Num == 14 || GamePlay.instance.Play_Stage_Num == 29))
                    PushPopup(ReviewPopup);

                break;
            case GameMode.Multi:

                AdsManager.Instance.BannerShow(true);

                Btn_Multi_ReStart.onClick.RemoveAllListeners();

                int win = 0;
                int lose = 0;

                switch ((Multi_Room)PhotonNetwork.CurrentRoom.CustomProperties["Map"])
                {
                    case Multi_Room.Dia_1:
                        win = 2;
                        lose = -1;
                        break;
                    case Multi_Room.Dia_5:
                        win = 4;
                        lose = -2;
                        break;
                    case Multi_Room.Dia_10:
                        win = 10;
                        lose = -5;
                        break;
                    default:
                        break;
                }

                if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                {

                    Btn_Multi_ReStart.onClick.AddListener(() =>
                    PhotonManager.Instance.Rpc_Check_Restart(PhotonNetwork.LocalPlayer));

                    Btn_Multi_ReStart.gameObject.SetActive(DataManager.Instance.Check_Crystal((int)PhotonNetwork.CurrentRoom.CustomProperties["Map"] * 2));

                    int plyaer_Score = (int)PhotonNetwork.LocalPlayer.CustomProperties["Score"];
                    int Enemy_Score = (int)PhotonNetwork.CurrentRoom.GetPlayer(PhotonManager.Instance.Enemy_id).CustomProperties["Score"];

                    //승리시 +2 패배 -1
                    //승리시 +4 패배 -2
                    //승리시 +10 패배 -5

                    Hashtable customRoomProperties = new Hashtable() { { "Ready", 0 } };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(customRoomProperties);

                    if (plyaer_Score > Enemy_Score)
                    {
                        AudioManager.instance.Play_Effect_Sound(Effect_Sound.popup_result_clear);

                        DataManager.Instance.state_Player.Rank += win;

                        Txt_Multi_Win.gameObject.SetActive(true);
                        Txt_Multi_Lose.gameObject.SetActive(false);
                        Img_Multi_Win.gameObject.SetActive(true);
                        Img_Multi_Lose.gameObject.SetActive(false);
                        Img_Multi_Draw.gameObject.SetActive(false);
                        Txt_Over_Multi_Player_Rank.text = DataManager.Instance.state_Player.Rank.ToString();
                        Txt_Over_Multi_Player_Up.text = string.Format("(▲{0})", win);
                        Txt_Over_Multi_Player_Up.color = Color.red;

                        DataManager.Instance.state_Player.crystal += ((int)PhotonNetwork.CurrentRoom.CustomProperties["Map"] * 2);
                        DataManager.Instance.Save_Player_Data();
                        UIManager.Instance.Set_Item_Txt(Item.crystal);

                        foreach (var item in Img_None_Star)
                        {
                            item.SetActive(true);
                        }

                        foreach (var item in Img_Multi_Stars)
                        {
                            item.gameObject.SetActive(true);
                        }



                    }
                    else if (plyaer_Score < Enemy_Score)
                    {
                        AudioManager.instance.Play_Effect_Sound(Effect_Sound.popup_result_fail);

                        DataManager.Instance.state_Player.Rank = DataManager.Instance.state_Player.Rank + lose <= 0 ? 0 : DataManager.Instance.state_Player.Rank + lose;

                        Txt_Multi_Win.gameObject.SetActive(false);
                        Txt_Multi_Lose.gameObject.SetActive(true);
                        Img_Multi_Win.gameObject.SetActive(false);
                        Img_Multi_Lose.gameObject.SetActive(true);
                        Img_Multi_Draw.gameObject.SetActive(false);
                        Txt_Over_Multi_Player_Rank.text = DataManager.Instance.state_Player.Rank.ToString();
                        Txt_Over_Multi_Player_Up.text = string.Format("(▼{0})", lose);
                        Txt_Over_Multi_Player_Up.color = Color.blue;

                        foreach (var item in Img_None_Star)
                        {
                            item.SetActive(true);
                        }

                        foreach (var item in Img_Multi_Stars)
                        {
                            item.gameObject.SetActive(false);
                        }

                    }
                    else
                    {
                        AudioManager.instance.Play_Effect_Sound(Effect_Sound.popup_result_draw);

                        Txt_Multi_Win.gameObject.SetActive(false);
                        Txt_Multi_Lose.gameObject.SetActive(false);
                        Img_Multi_Win.gameObject.SetActive(false);
                        Img_Multi_Lose.gameObject.SetActive(false);
                        Img_Multi_Draw.gameObject.SetActive(true);
                        Txt_Over_Multi_Player_Rank.text = DataManager.Instance.state_Player.Rank.ToString();
                        Txt_Over_Multi_Player_Up.text = "(- )";
                        Txt_Over_Multi_Player_Up.color = Color.gray;

                        foreach (var item in Img_None_Star)
                        {
                            item.SetActive(false);
                        }

                        foreach (var item in Img_Multi_Stars)
                        {
                            item.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    AudioManager.instance.Play_Effect_Sound(Effect_Sound.popup_result_clear);

                    DataManager.Instance.state_Player.Rank += win;

                    Txt_Multi_Win.gameObject.SetActive(true);
                    Txt_Multi_Lose.gameObject.SetActive(false);
                    Img_Multi_Win.gameObject.SetActive(true);
                    Img_Multi_Lose.gameObject.SetActive(false);
                    Img_Multi_Draw.gameObject.SetActive(false);
                    Txt_Over_Multi_Player_Rank.text = DataManager.Instance.state_Player.Rank.ToString();
                    Txt_Over_Multi_Player_Up.text = string.Format("(▲{0})", win);
                    Txt_Over_Multi_Player_Up.color = Color.red;

                    DataManager.Instance.state_Player.crystal += ((int)PhotonNetwork.CurrentRoom.CustomProperties["Map"] * 2);
                    DataManager.Instance.Save_Player_Data();
                    UIManager.Instance.Set_Item_Txt(Item.crystal);

                    foreach (var item in Img_None_Star)
                    {
                        item.SetActive(true);
                    }

                    foreach (var item in Img_Multi_Stars)
                    {
                        item.gameObject.SetActive(true);
                    }
                }

                DataManager.Instance.Save_Player_Data();

                PushPopup(Over_Popup);
                PopupAnimation(false);

                CloudOnceManager.Instance.Report_Leaderboard(gameMode, DataManager.Instance.state_Player.Rank);

                break;

            case GameMode.Timer:

                AdsManager.Instance.BannerShow(false);

                Txt_Over_Timer_0_Price.text = Conti_price[Conti].ToString();

                Over_Timer_0.SetActive(true);
                Over_Timer_1.SetActive(false);

                PushPopup(Over_Popup);
                CloudOnceManager.Instance.Report_Leaderboard(gameMode, ScoreManager.Instance.GetScore());

                break;
            default:
                break;
        }


    }

    public void Classic_End()
    {

        Txt_Over_Classic_Score.text = ScoreManager.Instance.GetScore().ToString();
        Txt_Over_Classic_Best.text = DataManager.Instance.state_Player.Classic.ToString();

        //GoogleManager.Instance.Repart_LeaderBoard(ScoreManager.Instance.GetScore());
        Conti++;

        Over_Classic_0.SetActive(false);
        Over_Classic_1.SetActive(true);
    }


    public void Timer_End()
    {

        Txt_Over_Timer_1_Score.text = ScoreManager.Instance.GetScore().ToString();
        Txt_Over_Timer_1_Best.text = DataManager.Instance.state_Player.Classic.ToString();

        int dia_val = ScoreManager.Instance.GetScore() / 2000;
        if (dia_val >= 1)
        {
            List<int> star_item = new List<int>();
            star_item.Add(0);
            star_item.Add(dia_val);
            giftItem.Add(star_item);
            Get_Gift_Item(true);
        }

        //GoogleManager.Instance.Repart_LeaderBoard(ScoreManager.Instance.GetScore());
        Conti++;

        Over_Timer_0.SetActive(false);
        Over_Timer_1.SetActive(true);
    }

    int[] Conti_price = new int[] { 5, 10, 20, 30, 50, 100, 150, 200 };
    int Conti = 0;

    /// <summary>
    /// 게임오버 팝업 애니메이션
    /// </summary>
    /// <param name="Up"></param>
    public void PopupAnimation(bool Up)
    {
        if (Up)
        {
            Pop.Play("Up");
            Btn_Over_Down_Popup.gameObject.SetActive(true);
            Btn_Over_Up_Popup.gameObject.SetActive(false);
        }
        else
        {
            Pop.Play("Down");
            Btn_Over_Down_Popup.gameObject.SetActive(false);
            Btn_Over_Up_Popup.gameObject.SetActive(true);
        }
    }

    #endregion


    #region stage_mission_popup

    /// <summary>
    /// 스테이지 미션 초기 세팅
    /// </summary>
    /// <param name="stage_num"></param>
    public void Set_Stage_Mission_Popop(int stage_num = -1)
    {
        AdsManager.Instance.BannerShow(true);

        if (stage_num.Equals(-1))
            stage_num = GamePlay.instance.Play_Stage_Num;

        if (!stage_Infos[stage_num].isLock)
        {
            Set_Mission_Popup(stage_num);
            PushPopup(StageMissionPopup);
        }

    }

    /// <summary>
    /// 스테이지 미션 정보 세팅
    /// </summary>
    /// <param name="stage_num"></param>
    public void Set_Mission_Popup(int stage_num)
    {

        Game.GetComponent<GamePlay>().Play_Stage_Num = stage_num;

        Dictionary<string, object> stage_Info = DataManager.Instance.stage_data[stage_num];

        foreach (var item in Txt_Stage_Level_Val)
            item.text = (stage_num + 1).ToString();


        foreach (var item in Mission_Bg)
            item.gameObject.SetActive(false);

        Mission_Block.gameObject.SetActive(false);
        Mission_Time.gameObject.SetActive(false);
        Mission_Score.gameObject.SetActive(false);

        Debug.Log(stage_num);

        Clear_Stage_Info Find_Stage_Info = DataManager.Instance.state_Player.clear_Stage.Find(x => x.Stage_Id.Equals(stage_num));

        int stage_Star = Find_Stage_Info != null ? Find_Stage_Info.Stage_Star : 0;

        int Mission_val = 0;
        int Block_val = 0;

        for (int i = 0; i < 3; i++)
        {
            int missionNum = stage_Info["mission_" + (i + 1)].ToString().TryParseInt();
            int clearNum = (int)stage_Info["clear_" + (i + 1)];

            if (stage_num >= 7)
            {
                switch (stage_Star)
                {
                    case 0:
                        break;
                    case 1:
                        clearNum += Mathf.RoundToInt(clearNum * 0.3f);
                        break;
                    case 2:
                    case 3:

                        clearNum += Mathf.RoundToInt(clearNum * 0.7f);

                        break;
                    default:
                        break;
                }
            }

            Debug.Log("MISSION " + missionNum);
            switch (missionNum)
            {

                case 0:
                    Mission_Boxs[i].gameObject.SetActive(false);

                    break;
                case 1:
                    Mission_Boxs[i].gameObject.SetActive(false);

                    Mission_Score.gameObject.SetActive(true);
                    Txt_Mission_Score.text = clearNum.ToString();
                    Mission_val += 1;

                    break;
                case 10:
                    Mission_Boxs[i].gameObject.SetActive(false);

                    Mission_Time.gameObject.SetActive(true);
                    Txt_Mission_Time.text = clearNum.ToString();
                    Mission_val += 1;
                    break;
                default:
                    Mission_Block.gameObject.SetActive(true);

                    Mission_Boxs[i].GetComponent<Image>().sprite = Sp_Blocks[missionNum - 2];
                    Mission_Boxs[i].transform.Find("Mission_Box_Bg/Mission_Val").GetComponent<Text>().text = clearNum.ToString();
                    Mission_Boxs[i].gameObject.SetActive(true);
                    if (Block_val == 0) Block_val += 1;
                    break;
            }

        }

        Mission_Bg[(Mission_val + Block_val) - 1].SetActive(true);

    }

    /// <summary>
    /// 스테이지 블럭 정보 세팅
    /// </summary>
    public void Set_Mission_Block_Num()
    {
        if (GamePlay.instance.gameMode != GameMode.Stage)
            return;

        if (clearScore == 0)
        {
            Txt_Top_Mission_Score.text = ScoreManager.Instance.GetScore().ToString();

        }

        for (int i = 0; i < Top_Mission_Boxs.Count; i++)
        {
            int val = GamePlay.instance.Mission_Block_Val[i] >= 0 ? GamePlay.instance.Mission_Block_Val[i] : 0;

            switch (GamePlay.instance.Mission_Block_Num[i])
            {

                case 0:
                    Top_Mission_Boxs[i].gameObject.SetActive(false);

                    if (clearScore == 0)
                    {
                        Txt_Top_Mission_Score.text = ScoreManager.Instance.GetScore().ToString();

                    }
                    break;
                case 1:
                    Top_Mission_Boxs[i].gameObject.SetActive(false);

                    Txt_Top_Mission_Score.text = string.Format("{0} / {1}", ScoreManager.Instance.GetScore(), clearScore);
                    break;
                case 10:
                    //Txt_Mission_Time.text = val.ToString();
                    Top_Mission_Boxs[i].gameObject.SetActive(false);

                    break;
                default:
                    Top_Mission_Boxs[i].transform.Find("Mission_Val").GetComponent<Text>().text = val.ToString();
                    //Txt_Top_Mission_Score.text = ScoreManager.Instance.GetScore().ToString();

                    break;
            }
        }
    }

    #endregion


    public void Set_Google_Txt()
    {
        Btn_Setting_Google_Login.gameObject.SetActive(!Social.localUser.authenticated);
        Btn_Setting_Google_Logout.gameObject.SetActive(Social.localUser.authenticated);
        Btn_Google_Login.gameObject.SetActive(!Social.localUser.authenticated);
        Btn_Google_Logout.gameObject.SetActive(Social.localUser.authenticated);

#if UNITY_ANDROID

        Txt_Google_Title_Login.gameObject.SetActive(!Social.localUser.authenticated);
        Txt_Google_Title_Logout.gameObject.SetActive(Social.localUser.authenticated);
        Txt_Ios_Title_Login.gameObject.SetActive(false);

#else
        Txt_Google_Title_Login.gameObject.SetActive(false);
        Txt_Google_Title_Logout.gameObject.SetActive(false);
        Txt_IOS_Title_Login.gameObject.SetActive(!Social.localUser.authenticated);
#endif

    }

    public void Set_Music_Sprite()
    {
        Btn_Pause_Music.GetComponentsInChildren<Image>()[1].sprite = Music[AudioManager.instance.isMusicEnabled ? 1 : 0];
        Btn_Setting_Music.GetComponentsInChildren<Image>()[1].sprite = Music[AudioManager.instance.isMusicEnabled ? 1 : 0];

        Btn_Pause_Effect.GetComponentsInChildren<Image>()[1].sprite = Effect[AudioManager.instance.isEffectEnabled ? 1 : 0];
        Btn_Setting_Effect.GetComponentsInChildren<Image>()[1].sprite = Effect[AudioManager.instance.isEffectEnabled ? 1 : 0];

    }

    public void Set_Item_Txt(Item _item)
    {
        switch (_item)
        {
            case Item.crystal:
                Txt_Dia.text = DataManager.Instance.state_Player.crystal + "";
                Txt_Shop_Dia.text = DataManager.Instance.state_Player.crystal + "";
                break;
            case Item.rotation:
            case Item.Recover:
            case Item.change:
            case Item.boom:

                Txt_Item_Val[(int)_item - 1].text = DataManager.Instance.state_Player.item_info.Items[(int)_item - 1] == 0 ?
                    "+" : DataManager.Instance.state_Player.item_info.Items[(int)_item - 1] + "";
                break;
            case Item.multi_double:

                break;

            default:
                break;
        }

    }

    DateTime GiftTime;

    public void Set_Gift_Time()
    {
        if (DataManager.Instance.state_Player.giftTime == "")
        {
            Btn_Gift.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            Btn_Gift.interactable = true;
            Txt_GiftTime.gameObject.SetActive(false);
            Txt_Gift_Get.gameObject.SetActive(true);
        }
        else
        {
            GiftTime = DateTime.Parse(DataManager.Instance.state_Player.giftTime);

            TimeSpan LateTime = GiftTime - DateTime.Now;

            if (LateTime.TotalSeconds <= 0)
            {
                Btn_Gift.GetComponent<Image>().color = new Color(1, 1, 1, 1);

                Btn_Gift.interactable = true;
                Txt_GiftTime.gameObject.SetActive(false);
                Txt_Gift_Get.gameObject.SetActive(true);
            }
            else
            {
                Btn_Gift.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

                Btn_Gift.interactable = false;
                StartCoroutine("Co_Gift_Timer");
                Txt_GiftTime.gameObject.SetActive(true);
                Txt_Gift_Get.gameObject.SetActive(false);

            }

        }
    }


    List<List<int>> giftItem = new List<List<int>>();
    public List<List<int>> PushgiftItem = new List<List<int>>();

    public void Get_Shop_Gift_Item()
    {
        AudioManager.instance.Play_Effect_Sound(Effect_Sound.gift_oppen);

        //메인 기프트

        giftItem.Clear();

        GiftTime = DateTime.Now.AddMinutes(5);
        DataManager.Instance.state_Player.shopgiftTime = GiftTime.ToString();
        DataManager.Instance.Save_Player_Data();
        Set_Gift_Popup();

        ShopManager.Instance.Set_Shop_Gift_Time();
        PushPopup(GiftPopup);


    }

    public void Get_Gift_Item(bool isDaily = false)
    {
        AudioManager.instance.Play_Effect_Sound(Effect_Sound.gift_oppen);

        //메인 기프트
        if (!isDaily)
        {
            giftItem.Clear();

            GiftTime = DateTime.Now.AddMinutes(10);
            DataManager.Instance.state_Player.giftTime = GiftTime.ToString();
            DataManager.Instance.Save_Player_Data();
            Set_Gift_Popup();


            Set_Gift_Time();
            PushPopup(GiftPopup);
        }
        else
        {
            Set_Gift_Popup();
            Set_Gift_Time();
        }


    }


    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {

        }
        else
        {

            switch (game_Stat)
            {
                case Game_Stat.Main:
                    //종료창

                    break;
                case Game_Stat.Game:
                    switch (GamePlay.instance.gameMode)
                    {
                        case GameMode.Classic:
                        case GameMode.Stage:
                        case GameMode.Timer:

                            if (!ShopPopup.activeSelf && !Show_Ads)
                            {
                                PushPopup(PausePopup);
                                AdsManager.Instance.BannerShow(false);
                                GamePlay.instance.Pause();

                                if (Show_Ads)
                                    Show_Ads = false;

                            }

                            GamePlay.instance.StopHighlighting();

                            foreach (var item in BlockShapeSpawner.Instance.spawn_Blocks)
                            {
                                item.transform.LeanMoveLocal(Vector3.zero, 0.2F);
                                item.transform.localScale = Vector3.one;

                            }


                            break;
                        case GameMode.Multi:
                            break;
                        default:
                            break;
                    }
                    break;
                case Game_Stat.End:
                    Debug.Log("에잇...??????????????????????????");
                    switch (GamePlay.instance.gameMode)
                    {
                        case GameMode.Classic:
                        case GameMode.Stage:
                            Debug.Log("에잇...");
                            PopupAnimation(false);
                            break;
                        case GameMode.Multi:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }


        }
    }


    public void Push_Gift()
    {
        StartCoroutine("Co_Push_Gift");
    }

    IEnumerator Co_Push_Gift()
    {

        while (enum_Gift != Enum_Gift.Stay_Gift || game_Stat != Game_Stat.Main)
        {
            yield return null;
        }

        enum_Gift = Enum_Gift.Push_Gift;


        AudioManager.instance.Play_Effect_Sound(Effect_Sound.gift_oppen);

        for (int i = 0; i < 3; i++)
        {
            Img_Push_Gift[i].gameObject.SetActive(i < PushgiftItem.Count);

            if (i < PushgiftItem.Count)
            {
                Img_Push_Gift[i].sprite = sp_Items[PushgiftItem[i][0]];
                Txt_Push_Gift_Val[i].text = "X" + PushgiftItem[i][1];

                Color color;

                if (PushgiftItem[i][0] == 0)
                {
                    ColorUtility.TryParseHtmlString("#CA4EF9", out color);

                }
                else
                {
                    ColorUtility.TryParseHtmlString("#50FC43", out color);

                }

                Txt_Push_Gift_Val[i].color = color;

            }
        }

        Btn_PushGift.gameObject.SetActive(false);

        PushPopup(PushGiftPopup);
    }

    public void Set_Gift_Popup()
    {
        enum_Gift = Enum_Gift.Main_Gift;

        if (giftItem.Count == 0)
        {
            int num = Random.Range(0, 5);

            List<int> star_item = new List<int>();
            star_item.Add(num);
            star_item.Add(1);
            giftItem.Add(star_item);

        }

        for (int i = 0; i < 3; i++)
        {
            Img_Gift[i].gameObject.SetActive(i < giftItem.Count);

            if (i < giftItem.Count)
            {
                Img_Gift[i].sprite = sp_Items[giftItem[i][0]];
                Txt_Gift_Val[i].text = "X" + giftItem[i][1];

                Color color;

                if (giftItem[i][0] == 0)
                {
                    ColorUtility.TryParseHtmlString("#CA4EF9", out color);

                }
                else
                {
                    ColorUtility.TryParseHtmlString("#50FC43", out color);

                }

                Txt_Gift_Val[i].color = color;

            }
        }



    }

    public void Get_Gift(bool isads)
    {

        foreach (var item in giftItem)
        {
            DataManager.Instance.Get_Item((Item)item[0], item[1] * (isads ? 2 : 1));

        }

        StartCoroutine("Co_Close_Gift");


    }

    public void Get_Push_Gift(bool isads)
    {

        foreach (var item in PushgiftItem)
        {
            DataManager.Instance.Get_Item((Item)item[0], item[1] * (isads ? 2 : 1));

        }

        StartCoroutine("Co_Close_Push_Gift");

    }

    IEnumerator Co_Close_Gift()
    {
        Anim_Gift.Play("BonusReward_close");

        yield return new WaitForSeconds(0.1f);

        enum_Gift = Enum_Gift.Stay_Gift;

        PopPopup();

    }


    IEnumerator Co_Close_Push_Gift()
    {
        Anim_Push_Gift.Play("BonusReward_close");

        yield return new WaitForSeconds(0.1f);

        enum_Gift = Enum_Gift.Stay_Gift;

        PopPopup();

    }

#region 데일리 기프트

    IEnumerator Co_Gift_Timer()
    {
        while (true)
        {
            TimeSpan LateTime = GiftTime - DateTime.Now;

            if (LateTime.TotalSeconds <= 0)
            {
                Btn_Gift.interactable = true;
                Txt_GiftTime.gameObject.SetActive(false);
                Txt_Gift_Get.gameObject.SetActive(true);
                Btn_Gift.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                StopCoroutine("Co_Gift_Timer");
            }
            else
            {
                int diffMiniute = LateTime.Minutes; //30
                int diffSecond = LateTime.Seconds; //0

                Txt_GiftTime.text = string.Format("{0:00}:{1:00}", diffMiniute, diffSecond);

            }

            yield return new WaitForSeconds(1.0f);


        }
    }



    public void Check_Daily()
    {
        DateTime daily;
        Debug.Log(DataManager.Instance.state_Player.daily);

        //처음 접속
        if (DataManager.Instance.state_Player.daily == "")
        {
            Set_Daily();

            Dictionary<string, object> dailygift_data = DataManager.Instance.dailygift_data[DataManager.Instance.state_Player.daily_Num];
            giftItem.Clear();

            for (int i = 0; i < 3; i++)
            {
                Debug.Log(i + "  " + dailygift_data["item_num_" + i]);
                string str_dailygift = (string)dailygift_data["item_num_" + i];

                if (str_dailygift != "")
                {
                    string[] str_block = str_dailygift.Split('/');

                    List<int> star_item = new List<int>();
                    star_item.Add(int.Parse(str_block[0]));
                    star_item.Add(int.Parse(str_block[1]));

                    giftItem.Add(star_item);
                }

            }

            Get_Gift_Item(true);

            PushPopup(DailyPopup);
            PushPopup(GiftPopup);

            daily = DateTime.Now;

            DataManager.Instance.state_Player.daily = daily.ToString();
            DataManager.Instance.state_Player.isDaily = true;
            DataManager.Instance.state_Player.daily_Num += 1;

            DataManager.Instance.Save_Player_Data();


        }
        else
        {

            //오늘 선물 안 받음
            if (!DataManager.Instance.state_Player.isDaily)
            {
                Set_Daily();

                Dictionary<string, object> dailygift_data = DataManager.Instance.dailygift_data[DataManager.Instance.state_Player.daily_Num];
                giftItem.Clear();

                for (int i = 0; i < 3; i++)
                {
                    Debug.Log(i + "  " + dailygift_data["item_num_" + i]);
                    string str_dailygift = (string)dailygift_data["item_num_" + i];

                    if (str_dailygift != "")
                    {
                        string[] str_block = str_dailygift.Split('/');

                        List<int> star_item = new List<int>();
                        star_item.Add(int.Parse(str_block[0]));
                        star_item.Add(int.Parse(str_block[1]));

                        giftItem.Add(star_item);
                    }

                }


                Get_Gift_Item(true);

                PushPopup(DailyPopup);
                PushPopup(GiftPopup);

                daily = DateTime.Now;

                DataManager.Instance.state_Player.daily = daily.ToString();
                DataManager.Instance.state_Player.isDaily = true;
                DataManager.Instance.state_Player.daily_Num += 1;

                DataManager.Instance.Save_Player_Data();





            }
            else
            {
                Debug.Log(DataManager.Instance.state_Player.daily);

                //오늘 선물 받음

                daily = Convert.ToDateTime(DataManager.Instance.state_Player.daily);
                Debug.Log(daily.Day + "   " + DateTime.Now.Day);
                if (daily.Day != DateTime.Now.Day)
                {
                    DataManager.Instance.state_Player.isDaily = false;
                    DataManager.Instance.Save_Player_Data();

                    Check_Daily();
                }

            }

        }
    }

    public void Set_Daily()
    {
        if (DataManager.Instance.state_Player.daily_Num == 7)
        {
            DataManager.Instance.state_Player.daily_Num = 0;
        }

        for (int i = 0; i < Btn_Day.Count; i++)
        {
            Color color;


            if (DataManager.Instance.state_Player.daily_Num >= i)
            {
                ColorUtility.TryParseHtmlString("#712408", out color);
                Btn_Day[i].GetComponent<Image>().color = color;

                ColorUtility.TryParseHtmlString("#FDA528", out color);
                Txt_Daily[i].color = color;

                Img_Check[i].gameObject.SetActive(true);

            }
            else
            {
                ColorUtility.TryParseHtmlString("#3f2524", out color);
                Btn_Day[i].GetComponent<Image>().color = color;

                ColorUtility.TryParseHtmlString("#ffffff", out color);
                Txt_Daily[i].color = color;

                Img_Check[i].gameObject.SetActive(false);


            }
        }
    }

    public void Set_All_Txt()
    {

        Txt_Dia.text = DataManager.Instance.state_Player.crystal + "";
        Txt_Shop_Dia.text = DataManager.Instance.state_Player.crystal + "";

        for (int i = 0; i < DataManager.Instance.state_Player.item_info.Items.Length - 1; i++)
        {
            Txt_Item_Val[i].text = DataManager.Instance.state_Player.item_info.Items[i] == 0 ? "+" : DataManager.Instance.state_Player.item_info.Items[i] + "";
        }
    }

#endregion

#region 튜토리얼

    public int tuto_Step = 0;

    public void Check_Tuto()
    {
        Dictionary<string, object> tuto_info = DataManager.Instance.tutorial_data[GamePlay.instance.Play_Stage_Num];

        tuto_Step = 0;

        TutoPopup.SetActive(true);

        foreach (var item in obj_Tuto)
        {
            item.SetActive(false);

        }

        obj_Tuto[GamePlay.instance.Play_Stage_Num].SetActive(true);
        if ((int)tuto_info["step"] > 1)
        {
            Check_Tuto_Over();
        }
    }

    public void Check_Tuto_Over()
    {
        Dictionary<string, object> tuto_info = DataManager.Instance.tutorial_data[GamePlay.instance.Play_Stage_Num];

        foreach (var item in obj_Tuto_child[GamePlay.instance.Play_Stage_Num])
        {
            item.SetActive(false);
        }

        if (GamePlay.instance.Play_Stage_Num <= 7)
        {
            if ((int)tuto_info["step"] > 1 && (int)tuto_info["step"] > tuto_Step)
            {
                obj_Tuto_child[GamePlay.instance.Play_Stage_Num][tuto_Step].gameObject.SetActive(true);
            }
            else
            {
                obj_Tuto[GamePlay.instance.Play_Stage_Num].SetActive(false);

            }
        }
    }

#endregion

    /// <summary>
    /// 게임 중 아이템이 없을때
    /// </summary>
    public void No_Item()
    {
        Scroll_Control(ShopManager.Instance.Content, Vector3.zero);
        PushPopup(ShopPopup);
        StopCoroutine("Co_Misstion_Timer");

    }

    public void Start_TxtStat(bool isSave)
    {
        Txt_Saving.SetActive(isSave);
        Txt_Save.SetActive(false);
        Txt_Loading.SetActive(!isSave);
        Txt_Load.SetActive(false);

        PushPopup(ColudPopup);
    }

    public void End_TxtStat(bool isSave)
    {

        Txt_Saving.SetActive(false);
        Txt_Save.SetActive(isSave);
        Txt_Loading.SetActive(false);
        Txt_Load.SetActive(!isSave);

        StartCoroutine("Co_End_TxtStat");
    }

    IEnumerator Co_End_TxtStat()
    {

        yield return new WaitForSeconds(1.0f);

        PopPopup();
    }

#region 멀티 플레이

    public void Set_Multi_Player(bool isPlayer, string str_player = "unKnow")
    {
        if (isPlayer)
        {
            //Img_Player = MatcingPopup.transform.Find("Img_Player").GetComponent<Image>();
            Searching();
            PopPopup();
            PushPopup(MatcingPopup);

        }
        else
        {
            //Img_Enemy = MatcingPopup.transform.Find("Img_Enemy").GetComponent<Image>();
            Txt_Enemy_Name.text = str_player;

        }

        //Txt_Count = MatcingPopup.transform.Find("Txt_Count").GetComponent<Text>();

    }

    public void Searching()
    {
        StopCoroutine("Co_Macth");
        StopCoroutine("Co_Searching");

        StartCoroutine("Co_Searching");

    }

    string[] str_search = new string[] { ".", "..", "..." };

    int sec = 0;

    IEnumerator Co_Searching()
    {

        Btn_Matcing_Back.gameObject.SetActive(true);

        while (true)
        {

            Txt_Count.text = str_search[sec];

            yield return new WaitForSeconds(0.5f);
            if (sec == 2)
                sec = 0;
            else
                sec += 1;

        }
    }

    public void ToRoom(bool restart = false)
    {
        StopCoroutine("Co_Searching");
        StopCoroutine("Co_Macth");

        Txt_Count.text = "매칭 완료";

        StartCoroutine("Co_Macth", restart);
    }

    IEnumerator Co_Macth(bool restart)
    {

        AdsManager.Instance.BannerShow(true);

        Slider_Multi_Time.maxValue = 60;
        Slider_Multi_Time.value = 60;

        Btn_Matcing_Back.gameObject.SetActive(false);

        Hashtable customRoomProperties = new Hashtable() { { "Ready", 1 } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(customRoomProperties);

        bool isReady = false;

        //로딩 화면 띄우기~~~

        DataManager.Instance.state_Player.crystal -= (int)PhotonNetwork.CurrentRoom.CustomProperties["Map"];
        DataManager.Instance.Save_Player_Data();
        Instance.Set_Item_Txt(Item.crystal);

        if (stack_Popup.Count >= 1)
            PopPopup();
        if (stack_Popup.Count >= 1)
            PopPopup();

        //Txt_Over_Multi_Player_Name.text = PhotonNetwork.NickName;
        //Txt_Multi_Title_Player.text = PhotonNetwork.NickName;
        //Img_Player.sprite = SendTextures(GoogleManager.Instance.Img_Byte);
        //Img_Over_Multi_Player.sprite = SendTextures(GoogleManager.Instance.Img_Byte);
        Txt_Multi_Player.text = "0";

        Txt_Multi_Title_Enemy.text = PhotonNetwork.CurrentRoom.GetPlayer(PhotonManager.Instance.Enemy_id).NickName;
        Txt_Over_Multi_Enemy_Name.text = PhotonNetwork.CurrentRoom.GetPlayer(PhotonManager.Instance.Enemy_id).NickName;
        //Img_Enemy.sprite = Texture_Sprite((byte[])PhotonNetwork.CurrentRoom.GetPlayer(PhotonManager.Instance.Enemy_id).CustomProperties["Img"]);
        //Img_Over_Multi_Enemy.sprite = Texture_Sprite((byte[])PhotonNetwork.CurrentRoom.GetPlayer(PhotonManager.Instance.Enemy_id).CustomProperties["Img"]);
        Txt_Multi_Enemy.text = "0";

        Play_Game(GameMode.Multi);
        game_Stat = Game_Stat.Wait;

        while (!isReady)
        {
            isReady = (int)PhotonNetwork.LocalPlayer.CustomProperties["Ready"] == 1
            && (int)PhotonNetwork.CurrentRoom.GetPlayer(PhotonManager.Instance.Enemy_id).CustomProperties["Ready"] == 1;

            yield return new WaitForSeconds(0.1f);
        }


        int time = 3;

        Txt_Multi_Start_Time.text = time.ToString();
        Txt_Multi_Start_Time.gameObject.SetActive(true);

        while (time > 0)
        {
            Txt_Multi_Start_Time.text = time.ToString();

            yield return new WaitForSeconds(1.0f);
            time -= 1;

        }

        game_Stat = Game_Stat.Game;

        Txt_Multi_Start_Time.text = "Start";


        customRoomProperties = new Hashtable() { { "Score", 0 }, { "Rank", DataManager.Instance.state_Player.Rank } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(customRoomProperties);

        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable cp = PhotonNetwork.CurrentRoom.CustomProperties;
            cp["StartTime"] = PhotonNetwork.Time + 60;
            PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
            PhotonManager.Instance.Rpc_Start_Game();
        }

    }

    public void Set_Multi_Score(int Player, int Enemy)
    {
        Txt_Multi_Player.text = Player.ToString();
        Txt_Multi_Enemy.text = Enemy.ToString();
    }

    public void Start_Game()
    {
        Txt_Multi_Start_Time.gameObject.SetActive(false);
        StartCoroutine("MultiTime");
    }

    IEnumerator MultiTime()
    {

        Double StartTime = (Double)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];

        while (Slider_Multi_Time.value > 0)
        {

            yield return new WaitForSeconds(0.1f);
            Slider_Multi_Time.value = (float)(StartTime - PhotonNetwork.Time);

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                StopCoroutine("MultiTime");
            }


        }

        Set_GameOver_UI();
    }

    public void Change_Multi_Item()
    {
        Txt_Multi_Item_Free.SetActive(false);
        Img_Multi_Dia.SetActive(true);
        Txt_Multi_Sec.gameObject.SetActive(false);

    }

    public void Start_Multi_Item()
    {
        Txt_Multi_Item_Free.SetActive(false);
        Img_Multi_Dia.SetActive(false);
        Txt_Multi_Sec.gameObject.SetActive(true);
    }


    Queue<enum_Msg> str_msg = new Queue<enum_Msg>();

    public void Start_Message_Popup(enum_Msg str)
    {
        str_msg.Enqueue(str);

        if (str_msg.Count == 1)
            StartCoroutine("Co_Msg");

    }

    IEnumerator Co_Msg()
    {
        while (str_msg.Count != 0)
        {
            switch (str_msg.Dequeue())
            {
                case enum_Msg.Exit:
                    Txt_Msg_Exit.gameObject.SetActive(true);
                    Txt_Msg_ReMatch.gameObject.SetActive(false);
                    Txt_Msg_Cencel.gameObject.SetActive(false);
                    break;
                case enum_Msg.Rematch:
                    Txt_Msg_Exit.gameObject.SetActive(false);
                    Txt_Msg_ReMatch.gameObject.SetActive(true);
                    Txt_Msg_Cencel.gameObject.SetActive(false);
                    break;
                case enum_Msg.Cencel:
                    Txt_Msg_Exit.gameObject.SetActive(false);
                    Txt_Msg_ReMatch.gameObject.SetActive(false);
                    Txt_Msg_Cencel.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }

            MsgPopup.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            MsgPopup.SetActive(false);
        }
    }

    public void Multi_Join()
    {
        if (!Check_Network())
            return;

        Debug.Log("멀티 조인");
        // #Critical, we must first and foremost connect to Photon Online Server.
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();

#if UNITY_EDITOR
        PhotonManager.Instance.Player_Num();
        PushPopup(MultiPopup);

#else
        if (!Social.localUser.authenticated)
        {
            Set_Google_Txt();
            PushPopup(GooglePopup);


        }
        else
        {      
            PhotonManager.Instance.Player_Num();

            PushPopup(MultiPopup);

        }
#endif

    }

#endregion


#region 버튼 이벤트

    public void Btn_Pointer_Down(Transform obj)
    {
        obj.localScale = Vector3.one * 0.9f;
    }

    public void Btn_Pointer_Up(Transform obj)
    {
        obj.localScale = Vector3.one;
    }

#endregion

    public void Toggle_Check(Toggle toggle)
    {
        if (Toggle_Privacy.isOn && Toggle_service.isOn)
        {
            PlayerPrefs.SetInt("service", 1);

            PopPopup();

            SocialManager.Instance.DoAutoLogin();

            DataManager.Instance.Get_Json_Data();

            LanguageManager.Instance.Get_Language();

            SetUi();

            Check_Daily();
            //PushPopup(BestPopup);

#if UNITY_EDITOR
            FireBaseManager.Instance.Get_Editor_Gift();

#else
            FireBaseManager.Instance.Add_Token();
#endif

        }
    }

#region 튜토리얼 

    public void Touch_active(bool active)
    {
        foreach (var item in touchs)
        {
            item.SetActive(active);
        }

    }

#endregion

#region 스테이지 별 애니메이션

    IEnumerator Co_Over_Star(int total)
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log(Img_Stars.Count + "   " + total);

        for (int i = 0; i < total; i++)
        {
            AudioManager.instance.Play_Effect_Sound(Effect_Sound.stage_clear_star);
            Img_Stars[2 - i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

    }

#endregion

    public bool Check_Network()
    {

        bool check = Application.internetReachability != NetworkReachability.NotReachable;

        if (!check) StartCoroutine(Co_Check_Network());

        return check;
    }

    IEnumerator Co_Check_Network()
    {

        NetworkPopup.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        NetworkPopup.SetActive(false);

    }

    public void Check_Back()
    {
        if (game_Stat.Equals(Game_Stat.End))
        {
            Go_Main();
            PushPopup(StagePopup);
        }

    }

    public void Open_Best()
    {

    }
}


