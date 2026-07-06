using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class IngredientSlotUI
{
    public GameObject slotRoot;
    public TMP_Text count;
    public Image icon;
}
[System.Serializable]
public class RecipeCardUI
{
    public RecipeData recipeData;
    public List<IngredientSlotUI> slotUIList;
    public Button cookButton;
    public CanvasGroup canvasGroup;
    public TMP_Text nameText;

}
public class RecipePanelUI : MonoBehaviour
{
    public List<RecipeCardUI> recipeCardUIs = new List<RecipeCardUI>();

    void Awake()
    {
        InitialCards();
        AutoLinkSlots();
    }

    void AutoLinkSlots()
    {
        foreach (RecipeCardUI card in recipeCardUIs)
        {
            if (card.nameText == null)
            {
                Transform nameTextTransform = card.canvasGroup.transform.Find("NameText");
                if (nameTextTransform != null)
                {
                    card.nameText = nameTextTransform.GetComponent<TMP_Text>();
                }
                else
                {
                    Debug.LogWarning($"NameText를 못 찾음: {card.canvasGroup.name}");
                }
            }
            foreach (IngredientSlotUI slot in card.slotUIList)
            {
                if (slot.slotRoot == null)
                {
                    Debug.LogWarning($"slotRoot가 비어있음: {card.recipeData?.recipeName}");
                    continue;
                }

                // 검증: icon이 이미 채워져 있어도, 진짜 내 자식이 맞는지 확인
                if (slot.icon != null && slot.icon.transform.parent != slot.slotRoot.transform)
                {
                    Debug.LogWarning($"[{card.recipeData?.recipeName}] icon이 잘못된 슬롯을 참조 중! " +
                        $"기대: {slot.slotRoot.name}(InstanceID:{slot.slotRoot.GetInstanceID()})의 자식, " +
                        $"실제 부모: {slot.icon.transform.parent.name}(InstanceID:{slot.icon.transform.parent.GetInstanceID()})");
                }

                if (slot.icon == null)
                {
                    Transform iconTransform = slot.slotRoot.transform.Find("Icon");
                    if (iconTransform != null)
                        slot.icon = iconTransform.GetComponent<Image>();
                    else
                        Debug.LogWarning($"Icon을 못 찾음: {slot.slotRoot.name}");
                }

                // count도 동일하게 검증 로직 추가 가능
                if (slot.count == null)
                {
                    Transform countTransform = slot.slotRoot.transform.Find("CountText");
                    if (countTransform != null)
                        slot.count = countTransform.GetComponent<TMP_Text>();
                    else
                        Debug.LogWarning($"CountText를 못 찾음: {slot.slotRoot.name}");
                }
            }
        }
    }

    void InitialCards()
    {
        foreach(RecipeCardUI card in recipeCardUIs)
        {
            RecipeCardUI captured = card;
            card.cookButton.onClick.AddListener(() =>
            {
                RecipeManager.Instance.Cook(captured.recipeData);
                UpdateUI();
            });
        }
    }
    void OnEnable()
    {
        UpdateUI(); 
    }
    void UpdateUI()
    {
        foreach(RecipeCardUI card in recipeCardUIs)
        {
            RecipeData RD = card.recipeData;
            card.nameText.text = RD.recipeName;
            for(int i = 0; i < card.slotUIList.Count; i++)
            {
                if (i < RD.requirements.Count)
                {
                    card.slotUIList[i].count.text = RD.requirements[i].count.ToString();
                    card.slotUIList[i].icon.sprite = RD.requirements[i].ingredientData.sprite;
Debug.Log($"{card.recipeData.recipeName} - 슬롯{i}: sprite = {RD.requirements[i].ingredientData.sprite}");
                    card.slotUIList[i].slotRoot.SetActive(true);
                }
                else
                {
                    card.slotUIList[i].slotRoot.SetActive(false);
                }
                
            }
            Dictionary<IngredientType, int> count = IngredientManager.Instance.GetCounts();
            if (RecipeManager.Instance.CanCraft(RD, count))
            {
                card.canvasGroup.alpha = 1f;
                card.canvasGroup.interactable = true;
            }
            else
            {
                card.canvasGroup.alpha = 0.4f;
                card.canvasGroup.interactable = false;
            }
        }
    }
}
