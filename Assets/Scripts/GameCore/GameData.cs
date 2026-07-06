using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatus
{
    public float attackDamage = 10f;
    public float maxHp = 100f;
    public float skillCooldown = 8f;
}
[System.Serializable]
public class IngredientSaveData
{
    public int meatCount;
    public int vegetableCount;
    public int spiceCount;
}
[System.Serializable]
public class CookedFoodData
{
    public int bulgogi;    // 불고기
    public int salad;      // 샐러드
    public int steak;      // 스테이크
    public int bibimbap;   // 비빔밥
    public int spicySoup;  // 매운탕
}
[System.Serializable]
public class SaveData
{
    public PlayerStatus playerStatus;
    public IngredientSaveData ingredientData;
    public CookedFoodData cookData;
}
public static class GameData 
{
    public static PlayerStatus playerStatus = new PlayerStatus();
    public static IngredientSaveData ingredientData = new IngredientSaveData();
    public static CookedFoodData cookData = new CookedFoodData();
    
}