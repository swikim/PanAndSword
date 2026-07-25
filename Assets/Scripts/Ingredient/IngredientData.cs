using UnityEngine;
 
public enum IngredientType { Meat, Vegetable, Spice }
 
[CreateAssetMenu(menuName = "PanAndSword/IngredientData")]
public class IngredientData : ScriptableObject
{
    public string ingredientName;
    public Sprite sprite;
    [Range(0f, 1f)] public float dropRate = 0.5f;
    public IngredientType type;
 
    [Header("드롭 개수 (Boss 등 다중 드롭 몬스터 전용, 일반 몬스터는 미사용)")]
    public int minDropCount = 1;
    public int maxDropCount = 1;
}