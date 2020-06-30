using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager Instance;

    private IStoreController controller = null;
    private IExtensionProvider extensions = null;

    private string[] sProductIds = { "noads_299" ,"beginner_pack_999", "best_pack_1999", "special_pack_2999", "rich_pack_9999",
                                      "crystal_099", "crystal_299", "crystal_499", "crystal_999"   };


    private string[] android_ProductIds = { "noads_299" ,"beginner_pack_999", "best_pack_1999", "special_pack_2999", "rich_pack_9999",
                                      "crystal_099", "crystal_299", "crystal_499", "crystal_999"   };

    private string[] ios_ProductIds = { "noads_299" ,"beginner_pack_999", "best_pack_1999", "special_pack_2999", "rich_pack_9999",
                                      "crystal_099", "crystal_299", "crystal_499", "crystal_999"   };

    public List<string> price = new List<string>();

    private void Awake()

    {
        Instance = this;

    }

    void Start()
    {
        InitStore();

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        string receipt = builder.Configure<IAppleConfiguration>().appReceipt;
    }

    private bool IsInitialized()
    {
        return (controller != null && extensions != null);
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
            return;


        var module = StandardPurchasingModule.Instance();

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(module);

        for (int i = 0; i < sProductIds.Length; i++)
        {
            builder.AddProduct(sProductIds[i], ProductType.Consumable, new IDs
            {
                { ios_ProductIds[i], AppleAppStore.Name },
                { android_ProductIds[i], GooglePlay.Name },
            });
        }

        UnityPurchasing.Initialize(this, builder);


    }

    private void InitStore()
    {
        Debug.Log("결제 인증 시작");

        if (IsInitialized())
            return;



        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

#if UNITY_ANDROID
        Debug.Log("안드로이드 결제 인증 시작");
        foreach (var item in android_ProductIds)
        {
            builder.AddProduct(item, ProductType.Consumable);
        }
#elif UNITY_IOS

        Debug.Log("아이폰 결제 인증 시작");
        foreach (var item in ios_ProductIds)
        {
            builder.AddProduct(item, ProductType.Consumable);
        }
#endif


        UnityPurchasing.Initialize(this, builder);


    }

    void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {

        Debug.Log("아이폰 결제 완료");

        this.controller = controller;
        this.extensions = extensions;

        Debug.Log("결제 초기화 개수 " + controller.products.all.Length);

        foreach (var item in controller.products.all)
        {

            Find_Sing(item.metadata.isoCurrencyCode.ToString());

            price.Add(sign + " " + item.metadata.localizedPrice.ToString());


        }

        ShopManager.Instance.Shop_price();

    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("초기화 실패");

    }

    public void OnBtnPurchaseClicked(int index)
    {
        if (!UIManager.Instance.Check_Network())
            return;

        Debug.Log("결제 클릭   " + index);


#if UNITY_ANDROID


        Debug.Log("안드로이드 시작   ");

        if (controller != null)
        {
            // Fetch the currency Product reference from Unity Purchasing


            Product product = controller.products.WithID(android_ProductIds[index]);
            if (product != null && product.availableToPurchase)
            {
                controller.InitiatePurchase(product);
            }

        }

#elif UNITY_IOS

        
            Product product = controller.products.WithID(ios_ProductIds[index]);
            if (product != null && product.availableToPurchase)
            {
                controller.InitiatePurchase(product);
            }
#endif

#if UNITY_EDITOR

        ShopManager.Instance.Buy(index);

#endif

    }

    PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs e)
    {
        Debug.Log("구매 완료");

#if UNITY_ANDROID
        for (int i = 0; i < android_ProductIds.Length; i++)
        {
            if (e.purchasedProduct.definition.id.Equals(android_ProductIds[i]))
            {
                ShopManager.Instance.Buy(i);

                switch (i)
                {
                    case 0:

                        FireBaseManager.Instance.LogEvent("Main_Ads_Buy");

                        break;
                    case 1:

                        FireBaseManager.Instance.LogEvent("Shop_beginner_pack_Buy");

                        break;
                    case 2:
                        FireBaseManager.Instance.LogEvent("Shop_best_pack_Buy");

                        break;
                    case 3:
                        FireBaseManager.Instance.LogEvent("Shop_special_pack_Buy");

                        break;
                    case 4:
                        FireBaseManager.Instance.LogEvent("Shop_rich_pack_Buy");

                        break;
                    case 5:
                        FireBaseManager.Instance.LogEvent("Shop_Crystal_10_Buy");

                        break;
                    case 6:
                        FireBaseManager.Instance.LogEvent("Shop_Crystal_30_Buy");

                        break;
                    case 7:
                        FireBaseManager.Instance.LogEvent("Shop_Crystal_50_Buy");

                        break;
                    case 8:
                        FireBaseManager.Instance.LogEvent("Shop_Crystal_100_Buy");

                        break;

                    default:
                        break;
                }
            }

        }

#elif UNITY_IOS
#endif

        for (int i = 0; i < ios_ProductIds.Length; i++)
        {
            if (e.purchasedProduct.definition.id.Equals(ios_ProductIds[i]))
            {
                ShopManager.Instance.Buy(i);

                switch (i)
                {
                    case 0:

                        FireBaseManager.Instance.LogEvent("Main_Ads_Buy");

                        break;
                    case 1:

                        FireBaseManager.Instance.LogEvent("Shop_beginner_pack_Buy");

                        break;
                    case 2:
                        FireBaseManager.Instance.LogEvent("Shop_best_pack_Buy");

                        break;
                    case 3:
                        FireBaseManager.Instance.LogEvent("Shop_special_pack_Buy");

                        break;
                    case 4:
                        FireBaseManager.Instance.LogEvent("Shop_rich_pack_Buy");

                        break;
                    case 5:
                        FireBaseManager.Instance.LogEvent("Shop_Crystal_10_Buy");

                        break;
                    case 6:
                        FireBaseManager.Instance.LogEvent("Shop_Crystal_30_Buy");

                        break;
                    case 7:
                        FireBaseManager.Instance.LogEvent("Shop_Crystal_50_Buy");

                        break;
                    case 8:
                        FireBaseManager.Instance.LogEvent("Shop_Crystal_100_Buy");

                        break;

                    default:
                        break;
                }
            }

        }


        return PurchaseProcessingResult.Complete;
    }

    void IStoreListener.OnPurchaseFailed(Product i, PurchaseFailureReason error)
    {
        if (!error.Equals(PurchaseFailureReason.UserCancelled))
        {
            Debug.Log("구매 실패");

        }
    }

    string sign = "";

    /// <summary>
    /// 통화 기호 찾기
    /// </summary>
    public void Find_Sing(string code)
    {
        foreach (var item in DataManager.Instance.global_data)
        {
            if (code.Equals(item["code"]))
            {
                sign = item["sign"].ToString();
                break;
            }

            sign = "$";

        }

    }

    public void ReleaseIAP() {

        foreach (string productId in android_ProductIds)
        {
            Product p = controller.products.WithID(productId);

            //isPurchaseUnderProcess = true;

            if (p != null)
            {
                Debug.Log("구매한 거 " + p.definition.id);
                controller.ConfirmPendingPurchase(p);
            }
          
        }
    }

    public void Restorepurchase()
    {
        
        //if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        //{
            extensions.GetExtension<IAppleExtensions>().RestoreTransactions(result => {
                if (result)
                {
                    // This does not mean anything was restored,
                    // merely that the restoration process succeeded.
                    Debug.Log("구매 복원 성공");
                }
                else
                {
                    // Restoration failed.
                    Debug.Log("구매 복원 실패");
                }
            });
        //}
    }

    public bool HadPurchased(string productId)
    {
        if (!IsInitialized()) return false;

        var product = controller.products.WithID(productId);

        if (product != null)
        {
            return product.hasReceipt;
        }

        return false;

    }
}