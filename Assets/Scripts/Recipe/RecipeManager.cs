using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    
    public static RecipeManager Instance { get; private set;}   
    [SerializeField] private List<RecipeData> recipeDatas;


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
       if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnRoomCleared += OnRoomCleared;
        }
    }
    public void Cook(RecipeData data)
    {
        Dictionary<IngredientType, int> count = IngredientManager.Instance.GetCounts(); //현재 재료 개수
        if (!CanCraft(data, count))
        {
            Debug.Log("재료 부족");
            return;
        }
        IngredientManager.Instance.ConsumeIngredients(data.requirements);

        switch (data.skillEffect)
        {
            case SkillEffect.FlameOnAttack :
                GameData.cookData.bulgogi++;
                break;
            case SkillEffect.HealOnMove :
                GameData.cookData.salad++;
                break;
            case SkillEffect.Revive :
                GameData.cookData.steak++;
                break;
            case SkillEffect.AllStatBoost :
                GameData.cookData.bibimbap++;
                break;
            case SkillEffect.CooldownReduction :
                GameData.cookData.spicySoup++;
                break;
        }
        SaveManager.Instance.SaveGameData();
    }
    
   
    public bool CanCraft(RecipeData data, Dictionary<IngredientType, int> count)
    {
        foreach(var ingredient in data.requirements)
        {
            int have = count[ingredient.ingredientData.type];
            if(have < ingredient.count)
            {
                return false;
            }
        }
        return true;
    }
    void OnRoomCleared(RoomController clearedRoom)
    {
       
    }
    void OnDestroy()
    {
        if (RoomManager.Instance != null)
        {
            RoomManager.Instance.OnRoomCleared -= OnRoomCleared;
        }
    }
}
