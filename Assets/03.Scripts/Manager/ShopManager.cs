using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public Transform Content;

    public GameObject empty_place;

    public Shop_Info pick_Shop_Info;

    public Shop_Info Gift_Shop_Info;

    
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 상점 아이템 세팅
    /// </summary>
    public void Shop_Setting(GameObject shopPopup)
    {

        Shop_Info[] shopinfos = shopPopup.GetComponentsInChildren<Shop_Info>(true);

        Gift_Shop_Info = shopinfos[0];

        for (int i = 0; i < shopinfos.Length; i++)
        {
            shopinfos[i].GetComponent<Shop_Info>().Set_Shop_Item(DataManager.Instance.shop_data[i]);
            shopinfos[i].GetComponent<Shop_Info>().Shop_price();

        }

    }

    public void Shop_price()
    {
        Shop_Info[] shopinfos = UIManager.Instance.ShopPopup.GetComponentsInChildren<Shop_Info>(true);

        for (int i = 0; i < shopinfos.Length; i++)
        {
            shopinfos[i].GetComponent<Shop_Info>().Shop_price();

        }

    }
    bool isTouch = false;

    /// <summary>
    /// 인덱스로 구매한 아이템 선택 구매
    /// </summary>
    /// <param name="index"></param>
    public void Buy(int index)
    {
        if (isTouch)
            return;

        AudioManager.instance.Play_Effect_Sound(Effect_Sound.Product_purchase_complete);

        isTouch = true;

        StartCoroutine(Co_Touch());

        Dictionary<string, object> Shop_data = DataManager.Instance.shop_data.Find(x => ((int)x["num"]).Equals(index +1));

        int val = 0;
        switch ((int)Shop_data["shop_type"])
        {
            case 0:
                for (int i = 0; i < 5; i++)
                {
                    val = int.Parse(Shop_data["item_" + i].ToString());

                    if (val != 0)
                        DataManager.Instance.Get_Item((Item)i, val);
                }
                break;
            case 1:

                if ((int)Shop_data["price_type"] == 0)
                {

                    for (int i = 0; i < 5; i++)
                    {
                        val = int.Parse(Shop_data["item_" + i].ToString());

                        if (val != 0)
                            DataManager.Instance.Get_Item((Item)i, val);
                    }
                }
                else
                {
                    if (DataManager.Instance.Check_Crystal((int)Shop_data["price"]))
                    {
                        Debug.Log(Shop_data["price"]);
                        DataManager.Instance.state_Player.crystal -= (int)Shop_data["price"];

                        for (int i = 0; i < 5; i++)
                        {
                            val = int.Parse(Shop_data["item_" + i].ToString());

                            if (val != 0)
                                DataManager.Instance.Get_Item((Item)i, val);
                        }
                        DataManager.Instance.Save_Player_Data();
                        UIManager.Instance.Set_All_Txt();

                        #region Firebase
                        switch ((int)Shop_data["num"])
                        {

                            case 9:
                                FireBaseManager.Instance.LogEvent("Shop_Rotation_Item_Buy");

                                break;
                            case 10:
                                FireBaseManager.Instance.LogEvent("Shop_Undo_Item_Buy");

                                break;
                            case 11:
                                FireBaseManager.Instance.LogEvent("Shop_Change_Item_Buy");

                                break;
                            case 12:
                                FireBaseManager.Instance.LogEvent("Shop_Boom_Item_Buy");

                                break;
                            default:
                                break;
                        }
                        #endregion
                    }
                }


                break;
            case 2:
                DataManager.Instance.state_Player.noAds = true;
                DataManager.Instance.Save_Player_Data();
                UIManager.Instance.Set_All_Txt();
                Shop_Info[] shopinfos = UIManager.Instance.ShopPopup.GetComponentsInChildren<Shop_Info>(true);

                shopinfos[1].gameObject.SetActive(false);
                break;

        }
    }

    IEnumerator Co_Touch()
    {
        yield return new WaitForSeconds(0.2f);
        isTouch = false;
    }

    DateTime GiftTime;

    public void Set_Shop_Gift_Time()
    {
        if (DataManager.Instance.state_Player.shopgiftTime == "")
        {
            Gift_Shop_Info.Btn_Shop_Item_Buy.GetComponent<Image>().color = new Color(1, 1, 1, 1);

            Gift_Shop_Info.Btn_Shop_Item_Buy.interactable = true;
            Gift_Shop_Info.Txt_Shop_Item_Time.gameObject.SetActive(false);
            Gift_Shop_Info.Txt_Shop_Item_Price.gameObject.SetActive(true);
        }
        else
        {
            GiftTime = DateTime.Parse(DataManager.Instance.state_Player.shopgiftTime);

            TimeSpan LateTime = GiftTime - DateTime.Now;

            if (LateTime.TotalSeconds <= 0)
            {
                Debug.Log("shopsdsdsdsd");
                Gift_Shop_Info.Btn_Shop_Item_Buy.GetComponent<Image>().color = new Color(1, 1, 1, 1);

                Gift_Shop_Info.Btn_Shop_Item_Buy.interactable = true;
                Gift_Shop_Info.Txt_Shop_Item_Time.gameObject.SetActive(false);
                Gift_Shop_Info.Txt_Shop_Item_Price.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("shopstatt");
                Gift_Shop_Info.Btn_Shop_Item_Buy.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);

                Gift_Shop_Info.Btn_Shop_Item_Buy.interactable = false;
                StartCoroutine("Co_Shop_Gift_Timer");
                Gift_Shop_Info.Txt_Shop_Item_Time.gameObject.SetActive(true);
                Gift_Shop_Info.Txt_Shop_Item_Price.gameObject.SetActive(false);

            }

        }
    }


    IEnumerator Co_Shop_Gift_Timer()
    {
        while (true)
        {
            TimeSpan LateTime = GiftTime - DateTime.Now;

            if (LateTime.TotalSeconds <= 0)
            {
                Gift_Shop_Info.Btn_Shop_Item_Buy.interactable = true;
                Gift_Shop_Info.Txt_Shop_Item_Time.gameObject.SetActive(false);
                Gift_Shop_Info.Txt_Shop_Item_Price.gameObject.SetActive(true);
                Gift_Shop_Info.Btn_Shop_Item_Buy.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                StopCoroutine("Co_Shop_Gift_Timer");
            }
            else
            {
                int diffMiniute = LateTime.Minutes; //30
                int diffSecond = LateTime.Seconds; //0

                Gift_Shop_Info.Txt_Shop_Item_Time.text = string.Format("{0:00}:{1:00}", diffMiniute, diffSecond);

            }

            yield return new WaitForSeconds(1.0f);


        }
    }
}
