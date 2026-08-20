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
        
        if(currentRunCollected.ContainsKey(data))
        {
            currentRunCollected[data]++;
        }
        else
        {
            currentRunCollected[data] = 1;
        }
    }    
    public void CommitRunToPermanentData()
    {
        foreach(var pair in currentRunCollected)
        {
            switch (pair.Key.type)
            {
                case IngredientType.Meat:
                    GameData.ingredientData.meatCount += pair.Value;
                    break;
                case IngredientType.Vegetable:
                    GameData.ingredientData.vegetableCount += pair.Value;
                    break;
                case IngredientType.Spice:
                    GameData.ingredientData.spiceCount += pair.Value;
                    break;
            }
        }
        SaveManager.Instance.SaveGameData(); // 파일로도 확정 저장
    }
    public void ResetRunCollected()
    {
        currentRunCollected.Clear();
        Debug.Log("currentRunCollected has been reset.");
    }
    public Dictionary<IngredientData, int> GetCurrentRunCollected()
    {
        if(currentRunCollected == null || currentRunCollected.Count == 0)
        {
            return new Dictionary<IngredientData, int>();
        }
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
        Debug.Log("GameData.ingredientData is null? " + (GameData.ingredientData.meatCount));
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
