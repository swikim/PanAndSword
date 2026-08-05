using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    public static IngredientManager Instance { get; private set; }
    private const int UpgradeThreshold = 2;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Skill skill;
    public Dictionary<IngredientData,int> currentRunCollected = new Dictionary<IngredientData, int>();


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
    }
    public void AddIngredient(IngredientData data)
    {
        switch (data.type)
        {
            case IngredientType.Meat:
                GameData.ingredientData.meatCount++;
                break;
            case IngredientType.Vegetable:
                GameData.ingredientData.vegetableCount++;
                break;
            case IngredientType.Spice:
                GameData.ingredientData.spiceCount++;
                break;
        }
        int currentCount = data.type switch
        {
            IngredientType.Meat      => GameData.ingredientData.meatCount,
            IngredientType.Vegetable => GameData.ingredientData.vegetableCount,
            IngredientType.Spice     => GameData.ingredientData.spiceCount,
            _                        => 0
        };
        if(currentRunCollected.ContainsKey(data))
        {
            currentRunCollected[data]++;
        }
        else
        {
            currentRunCollected[data] = 1;
        }

        Debug.Log($"[Manager] {data.type} {currentCount}/{UpgradeThreshold}");
        
        if(GameData.ingredientData.meatCount >= UpgradeThreshold)
        {
            Debug.Log("레시피를 사용할 수 있습니다.");
            //레시피 데이터에 bool 값을 하나 넣어서 사용 가능한지 체크
        }
    }    
    public void ResetRunCollected()
    {
        currentRunCollected.Clear();
    }
    public Dictionary<IngredientData, int> GetCurrentRunCollected()
    {
        return currentRunCollected;
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
    public Dictionary<IngredientType, int> GetCounts()
    {
        return new Dictionary<IngredientType, int>
        {
            { IngredientType.Meat, GameData.ingredientData.meatCount},
            { IngredientType.Vegetable, GameData.ingredientData.vegetableCount},
            { IngredientType.Spice, GameData.ingredientData.spiceCount},
        };
    }

    public void ConsumeIngredients(List<IngredientRequirement> requirements)
    {
        foreach(var req in requirements)
        {
            switch (req.ingredientData.type)
            {
                case IngredientType.Meat:
                    GameData.ingredientData.meatCount -= req.count;
                    break;
                case IngredientType.Vegetable:
                GameData.ingredientData.vegetableCount -= req.count;
                break;
                case IngredientType.Spice:
                    GameData.ingredientData.spiceCount -= req.count;
                    break;
            }
        }
    }
}
