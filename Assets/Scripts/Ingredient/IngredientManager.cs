using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance { get; private set; }
    private const int UpgradeThreshold = 5;
    public IngredientData ingredientData;
    private PlayerController playerController;
    private Dictionary<IngredientType,int> _counts;


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
        _counts = new Dictionary<IngredientType, int>
        {
            { IngredientType.Meat,      0 },
            { IngredientType.Vegetable, 0 },
            { IngredientType.Spice,     0 }
        };
        playerController = playerController.GetComponent<PlayerController>();
    }
    public void AddIngredient(IngredientData data)
    {
        _counts[data.type]++;
        Debug.Log($"[Manager] {data.type} {_counts[data.type]}/{UpgradeThreshold}");
        
        if(_counts[data.type] >= UpgradeThreshold)
        {
            Debug.Log("레시피를 사용할 수 있습니다.");
            //레시피 데이터에 bool 값을 하나 넣어서 사용 가능한지 체크
        }
    }    
    public void ApplyUpgrade(RecipeData recipeData)
    {
        switch (recipeData.skillEffect)
        {
            case SkillEffect.FlameOnAttack:
                playerController.attackDamage += 3;
                break;
        }
    }
}
