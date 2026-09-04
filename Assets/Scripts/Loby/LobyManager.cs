using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;



public class LobyManager : MonoBehaviour
{
    [Header("재료 텍스트")]
    [SerializeField] private TMP_Text meatCountText;
    [SerializeField] private TMP_Text vegetableCountText;
    [SerializeField] private TMP_Text spiceCountText;

    [Header("스탯 텍스트")]
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text cooldownText;
    [SerializeField] private TMP_Text autoHealText;

    [Header("버튼")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button recipeButton;
    [SerializeField] private Button upgradeBackButton;
    [SerializeField] private Button recipeBackButton;
    [SerializeField] private UpgradePanelUI upgradePanelUI;

    [Header("패널")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject recipePanel;

    [Header("광고제거")]
    [SerializeField] private Button removeAdsButton;

    void Start()
    {
        UpdateUI();

        startButton.onClick.AddListener(OnStartButton);
        upgradeButton.onClick.AddListener(OnUpgradeButton);
        recipeButton.onClick.AddListener(OnRecipeButton);
        upgradeBackButton.onClick.AddListener(() => upgradePanel.SetActive(false));
        recipeBackButton.onClick.AddListener(() => recipePanel.SetActive(false));
        removeAdsButton.onClick.AddListener(OnRemoveAdsButtonClicked);

        IAPManager.Instance.OnPurchaseCompleted += UpdateRemoveAdsButtonVisibility;
        UpdateRemoveAdsButtonVisibility(); 
        attackText.text = GameData.playerStatus.attackDamage.ToString();
        hpText.text = GameData.playerStatus.maxHp.ToString();
        cooldownText.text = GameData.playerStatus.skillCooldown.ToString("F1") + "s";
        autoHealText.text = GameData.playerStatus.autoHeal.ToString() + "hp/s";
    }

    void UpdateUI()
    {
        meatCountText.text = $"x{GameData.ingredientData.meatCount}";
        vegetableCountText.text = $"x{GameData.ingredientData.vegetableCount}";
        spiceCountText.text = $"x{GameData.ingredientData.spiceCount}";

        attackText.text = $"{GameData.playerStatus.attackDamage}";
        hpText.text = $"{GameData.playerStatus.maxHp}";
        cooldownText.text = $"{GameData.playerStatus.skillCooldown:F1}s";
    }
    void OnStartButton()
    {
        SceneManager.LoadScene("GameScene");
    }
    void OnRecipeButton()
    {
        recipePanel.SetActive(true);
    }
    void OnUpgradeButton()
    {
        upgradePanel.SetActive(true);
    }
    void OnRemoveAdsButtonClicked()
    {
        IAPManager.Instance.PurchaseRemoveAds();
    }
    void OnDestroy()
    {
        if (IAPManager.Instance != null)
        {
            IAPManager.Instance.OnPurchaseCompleted -= UpdateRemoveAdsButtonVisibility;
        }
    }
    void UpdateRemoveAdsButtonVisibility()
    {
        if (IAPManager.Instance.IsAdsRemoved())
        {
            removeAdsButton.gameObject.SetActive(false); // 이미 구매했으면 숨기기
        }
        else
        {
            removeAdsButton.gameObject.SetActive(true);
        }
    }
}
