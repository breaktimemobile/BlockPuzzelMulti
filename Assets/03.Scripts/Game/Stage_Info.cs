using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage_Info : MonoBehaviour
{
    public int stage_Id;

    [SerializeField] private Image Img_Stage_Bg;

    [SerializeField] private Image[] stars;

    [SerializeField] private Text txt_stage_Num;

    [SerializeField] private GameObject img_Lock;

    public bool isLock;

    [SerializeField] private Sprite[] sp_Stage_Bg;

    /// <summary>
    /// 스테이지 정보 세팅
    /// </summary>
    /// <param name="stage_Id"></param>
    public void Set_Stage(int stage_Id)
    {
        this.stage_Id = stage_Id;
        txt_stage_Num.text = stage_Id.ToString();
        Set_Star();
        Set_Lock();

    }

    /// <summary>
    /// 스테이지 락 정보 세팅
    /// </summary>
    public void Set_Lock()
    {
        isLock = DataManager.Instance.state_Player.clear_Stage.Count < stage_Id - 1;
        stars[0].transform.parent.gameObject.SetActive(!isLock);
        txt_stage_Num.gameObject.SetActive(!isLock);
        img_Lock.SetActive(isLock);

    }

    /// <summary>
    /// 스테이지 별 정보 세팅
    /// </summary>
    public void Set_Star()
    {
        foreach (var item in stars)
        {
            item.sprite = UIManager.Instance.Sp_None_Star;
        }

        Clear_Stage_Info clear_Stage_Info = DataManager.Instance.state_Player.clear_Stage.Find(x => x.Stage_Id.Equals(stage_Id - 1));

        Img_Stage_Bg.sprite = sp_Stage_Bg[0];

        Color color;

        ColorUtility.TryParseHtmlString("#1A3868", out color);

        txt_stage_Num.color = color;

        if (clear_Stage_Info != null)
        {
            switch (clear_Stage_Info.Stage_Star)
            {
                case 0:
                case 1:

                    Img_Stage_Bg.sprite = sp_Stage_Bg[0];

                    ColorUtility.TryParseHtmlString("#1A3868", out color);

                    txt_stage_Num.color = color;

                    break;
                case 2:
                    Img_Stage_Bg.sprite = sp_Stage_Bg[1];

                    ColorUtility.TryParseHtmlString("#633D17", out color);

                    txt_stage_Num.color = color;

                    break;
                case 3:
                    Img_Stage_Bg.sprite = sp_Stage_Bg[2];
                    
                    ColorUtility.TryParseHtmlString("#561610", out color);

                    txt_stage_Num.color = color;

                    break;
                default:
                    break;
            }


            for (int i = 0; i < clear_Stage_Info.Stage_Star; i++)
            {
                stars[i].sprite = UIManager.Instance.Sp_Star;
            }
        }



    }
}
