using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class FoodCardUI
{
    public RecipeData recipeData;
    public TMP_Text countText;
    public Button eatButton;
    public CanvasGroup canvasGroup;
}
public class UpgradePanelUI : MonoBehaviour
{
    public List<FoodCardUI> foodCardUI = new List<FoodCardUI>();

    void Awake()
    {
        // 버튼 연결은 Awake에서 (SetActive 상태 무관하게 실행)
        foreach (FoodCardUI card in foodCardUI)
        {
            FoodCardUI captured = card;
            card.eatButton.onClick.AddListener(() =>
            {
                UpgradeManager.Instance.Eat(captured.recipeData);
                UpdateUI();
            });
        }
    }

    void OnEnable()
    {
        UpdateUI(); // 패널 열릴 때마다 최신 데이터 반영
    }

    public void UpdateUI()
    {
        Debug.Log("업그레이드창 열기");
        foreach (FoodCardUI fo in foodCardUI)
        {
            int count = GetCount(fo.recipeData);
            bool has = count >= 1;

            fo.countText.text = $"x{count}";
            fo.canvasGroup.alpha = has ? 1f : 0.4f;
            fo.canvasGroup.interactable = has;
        }
    }

    int GetCount(RecipeData recipe)
    {
        return recipe.skillEffect switch
        {
            SkillEffect.FlameOnAttack      => GameData.cookData.bulgogi,
            SkillEffect.HealOnMove         => GameData.cookData.salad,
            SkillEffect.AttackBoostPercent => GameData.cookData.steak,
            SkillEffect.AllStatBoost       => GameData.cookData.bibimbap,
            SkillEffect.CooldownReduction  => GameData.cookData.spicySoup,
            _                              => 0
        };
    }
}