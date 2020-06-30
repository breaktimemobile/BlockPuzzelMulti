using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Shop_Info : MonoBehaviour
{

    public Dictionary<string, object> Shop_data;

    public Text Txt_Shop_Item_Name;

    public Button Btn_Shop_Item_Buy;
    public Text Txt_Shop_Item_Price;
    public Text Txt_Shop_Item_Time;

    private List<Image> Img_Shop_Items = new List<Image>();
    private List<Text> Txt_Shop_Items = new List<Text>();

    /// <summary>
    /// 상점 아이템 정보 세팅
    /// </summary>
    /// <param name="data"></param>
    public void Set_Shop_Item(Dictionary<string, object> data)
    {
        Shop_data = data;


        switch ((int)Shop_data["shop_type"])
        {
            case 0:

                Btn_Shop_Item_Buy = transform.Find("Btn_Shop_Item_Buy").GetComponent<Button>();
                Txt_Shop_Item_Price = Btn_Shop_Item_Buy.transform.Find("Txt_Shop_Item_Price").GetComponent<Text>();

                for (int i = 0; i < 5; i++)
                {
                    Img_Shop_Items.Add(transform.Find("Img_Shop_Item_" + i).GetComponent<Image>());
                    Txt_Shop_Items.Add(Img_Shop_Items[i].transform.Find("Txt_Shop_Item_" + i).GetComponent<Text>());

                }

                for (int i = 0; i < Txt_Shop_Items.Count; i++)
                {
                    string val = Shop_data["item_" + i].ToString();
                    Txt_Shop_Items[i].text = val;
                }


                break;
            case 1:
                Btn_Shop_Item_Buy = transform.Find("Btn_Shop_Item_Buy").GetComponent<Button>();
                Txt_Shop_Item_Price = Btn_Shop_Item_Buy.transform.Find("Txt_Shop_Item_Price").GetComponent<Text>();

                for (int i = 0; i < 1; i++)
                {
                    Img_Shop_Items.Add(transform.Find("Img_Shop_Item_" + i).GetComponent<Image>());
                    Txt_Shop_Items.Add(Img_Shop_Items[i].transform.Find("Txt_Shop_Item_" + i).GetComponent<Text>());

                }

                Txt_Shop_Items[0].text = (string)Shop_data["name"];

                break;
            case 2:

                if(DataManager.Instance.state_Player.noAds)
                    gameObject.SetActive(false);

                Btn_Shop_Item_Buy = GetComponentInChildren<Button>();
                Txt_Shop_Item_Price = Btn_Shop_Item_Buy.transform.Find("Txt_Shop_Item_Price").GetComponent<Text>();

                break;
            case 3:
                Btn_Shop_Item_Buy = GetComponentInChildren<Button>();
                Txt_Shop_Item_Price = Btn_Shop_Item_Buy.transform.Find("Txt_Shop_Item_Get").GetComponent<Text>();
                Txt_Shop_Item_Time = Btn_Shop_Item_Buy.transform.Find("Txt_Shop_Item_Time").GetComponent<Text>();

                break;
            default:
                break;
        }

    
        #region Firebase

        switch ((int)Shop_data["num"])
        {
            case 0:

                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_Free"));

                break;

            case 1:

                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_Ads"));

                break;

            case 2:

                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_beginner_pack_Cancel"));

                break;
            case 3:
                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_best_pack_Cancel"));

                break;
            case 4:
                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_special_pack_Cancel"));

                break;
            case 5:
                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_rich_pack_Cancel"));

                break;
            case 6:
                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_Crystal_10_Cancel"));

                break;
            case 7:
                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_Crystal_30_Cancel"));

                break;
            case 8:
                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_Crystal_50_Cancel"));

                break;
            case 9:
                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_Crystal_100_Cancel"));

                break;
            case 10:
                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_Rotation_Item_Cancel"));

                break;
            case 11:
                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_Undo_Item_Cancel"));

                break;
            case 12:
                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_Change_Item_Cancel"));

                break;
            case 13:
                Btn_Shop_Item_Buy.onClick.AddListener(() => FireBaseManager.Instance.LogEvent("Shop_Boom_Item_Cancel"));

                break;
            default:
                break;
        }
        #endregion

        Shop_price();
    }

    public void Shop_price()
    {
        switch ((int)Shop_data["price_type"])
        {
            case 0:
                Btn_Shop_Item_Buy.onClick.AddListener(() => IAPManager.Instance.OnBtnPurchaseClicked((int)Shop_data["num"] - 1));

                if (Txt_Shop_Item_Price != null)
                {
                    if (IAPManager.Instance.price.Count != 0)
                    {
                        Txt_Shop_Item_Price.text = IAPManager.Instance.price[(int)Shop_data["num"] - 1];

                    }
                    else
                    {
                        Txt_Shop_Item_Price.text = Shop_data["price"].ToString();

                    }

                }


                break;
            case 1:
                Btn_Shop_Item_Buy.onClick.AddListener(() => ShopManager.Instance.Buy((int)Shop_data["num"] - 1));
                Txt_Shop_Item_Price.text = Shop_data["price"].ToString();
                break;
            case 2:
                Btn_Shop_Item_Buy.onClick.AddListener(() => ShopManager.Instance.Buy((int)Shop_data["num"] - 1));
                Txt_Shop_Item_Price.text = Shop_data["price"].ToString();
                gameObject.SetActive(false);

                break;
            case 3:
                Btn_Shop_Item_Buy.onClick.AddListener(() => UIManager.Instance.enum_Gift = Enum_Gift.Shop_Gift);

                Btn_Shop_Item_Buy.onClick.AddListener(() => AdsManager.Instance.ShowRewardedAd());

                break;
            default:
                break;
        }

        Btn_Shop_Item_Buy.onClick.AddListener(() => AudioManager.instance.Play_Effect_Sound(Effect_Sound.button_soft));


    }
}
