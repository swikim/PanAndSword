using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class AdManager : MonoBehaviour, IUnityAdsShowListener, IUnityAdsLoadListener, IUnityAdsInitializationListener
{
    public static AdManager Instance { get; private set;}

    #if UNITY_ANDROID
    private string gameId = "800362566";
    #elif UNITY_IOS
        private string gameId = "iOS Game ID";
    #else
        private string gameId = "unexpected_platform";
    #endif

    private string interstitialAdUnitId = "Interstitial_Android"; //광고 단위 ID UnityDashboard에서 만든 이름과 동일해야함

    private Action onAdCompleteCallback; // 광고 끝나면 실행할 작업을 담아둘 변수
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        Advertisement.Initialize(gameId, true, this); // false => 테스트 모드, 개발 중 => true
        //Advertisement.Initialize(게임ID, 테스트모드여부, 콜백받을대상)
    }

    //초기화 완료 콜백
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads 초기화 완료");
        LoadInterstitialAd();
    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads 초기화 실패: {error} - {message}");
    }
    void LoadInterstitialAd()
    {
        Advertisement.Load(interstitialAdUnitId, this);
    }
    public void TryShowInterstitialAd(Action onComplete)
    {
        onAdCompleteCallback = onComplete;
        if(GameData.adsRemoved)
        {
            Debug.Log("광고 제거 구매됨, 광고 표시하지 않음");
            onComplete?.Invoke();
            return;
        }
        Advertisement.Show(interstitialAdUnitId, this);
    }

    //로드 콜백
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"광고 로드 완료: {placementId}");
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"광고 표시 실패: {error} - {message}");
    }
    // 표시 콜백
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"광고 표시 실패: {error} - {message}");
        onAdCompleteCallback?.Invoke(); // 광고 실패 시에도 콜백 실행
        onAdCompleteCallback = null;
    }
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        LoadInterstitialAd(); // 다음 표시를 위해 미리 다시 로드해둠
        onAdCompleteCallback?.Invoke();
        onAdCompleteCallback = null;
    }
}
