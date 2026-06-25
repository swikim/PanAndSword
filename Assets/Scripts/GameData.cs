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
public static class GameData 
{
    public static PlayerStatus playerStatus = new PlayerStatus();
    public static IngredientSaveData ingredientData = new IngredientSaveData();
    
}