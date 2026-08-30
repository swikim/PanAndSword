using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    public static IAPManager Instance { get; private set; }

    private const string REMOVE_ADS_ID = "remove_ads";

    private StoreController storeController;

    public event Action OnPurchaseCompleted;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitializeIAP();
    }

    async void InitializeIAP()
    {
        storeController = UnityIAPServices.StoreController();

        // 이벤트 핸들러 등록 
        storeController.OnPurchasePending += OnPurchasePending;
        storeController.OnPurchaseFailed += OnPurchaseFailed;
        storeController.OnPurchaseDeferred  += OnPurchaseDeferred;
        storeController.OnProductsFetched += OnProductsFetched;
        storeController.OnPurchasesFetched += OnPurchasesFetched;

        await storeController.Connect(); // 스토어 연결 (비동기)

        var productsToFetch = new List<ProductDefinition>
        {
            new ProductDefinition(REMOVE_ADS_ID, ProductType.NonConsumable)
        };

        storeController.FetchProducts(productsToFetch);
    }

    // 상품 목록을 성공적으로 가져왔을 때
    void OnProductsFetched(List<Product> products)
    {
        Debug.Log("상품 목록 가져오기 완료");
        storeController.FetchPurchases(); // 이어서 기존 구매 내역도 가져옴 (복원 포함)
    }

    // 구매 내역(복원 포함)을 가져왔을 때
    void OnPurchasesFetched(Orders orders)
    {
        foreach (var order in orders.ConfirmedOrders)
        {
            foreach (var item in order.CartOrdered.Items())
            {
                if (item.Product.definition.id == REMOVE_ADS_ID)
                {
                    GameData.adsRemoved = true;
                    SaveManager.Instance.SaveGameData();
                    Debug.Log("광고 제거 구매 확인됨 (복원 포함)");
                }
            }
        }
    }
    void OnPurchaseDeferred(DeferredOrder order)
    {
        Debug.Log("구매가 승인 대기 중입니다");
    }
    
    // 새로운 구매가 진행 중일 때 (구매 버튼 눌렀을 때)
    void OnPurchasePending(PendingOrder order)
    {
        foreach (var item in order.CartOrdered.Items())
        {
            if (item.Product.definition.id == REMOVE_ADS_ID)
            {
                GameData.adsRemoved = true;
                SaveManager.Instance.SaveGameData();
                OnPurchaseCompleted?.Invoke();
                Debug.Log("광고 제거 구매 완료");
            }
        }

        storeController.ConfirmPurchase(order); // 구매 확정 처리
    }

    void OnPurchaseFailed(FailedOrder order)
    {
        Debug.LogError($"구매 실패: {order.FailureReason}");
    }

    // ===== 외부에서 호출할 것들 =====

    public void PurchaseRemoveAds()
    {
        storeController.PurchaseProduct(REMOVE_ADS_ID);
    }

    public bool IsAdsRemoved()
    {
        return GameData.adsRemoved;
    }
}