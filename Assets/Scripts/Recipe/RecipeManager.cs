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
       RoomManager.Instance.OnRoomCleared += OnRoomCleared;
    }
    public void Cook(RecipeData data)
    {
        Dictionary<IngredientType, int> count = IngredientManager.Instance.GetCounts();
        if (!CanCraft(data, count))
        {
            Debug.Log("재료 부족");
            return;
        }
    }
    bool CanCraft(RecipeData data, Dictionary<IngredientType, int> count)
    {
        foreach(var ingredient in data.requirements)
        {
            int have = count[ingredient.type];
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
        RoomManager.Instance.OnRoomCleared -= OnRoomCleared;
    }
}
