using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SkillEffect
{
    FlameOnAttack,       // 불고기 - 공격 시 화염 DoT
    HealOnMove,          // 샐러드 - 이동 중 회복
    AttackBoostPercent,  // 스테이크 - 공격력 % 증가
    AllStatBoost,        // 비빔밥 - 전 스탯 소폭
    CooldownReduction    // 매운탕 - 쿨타임 대폭 감소
}
[System.Serializable]
public class IngredientRequirement
{
    public IngredientType type;
    public int count;
}
[CreateAssetMenu(menuName = "PanAndSword/RecipeData")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    public string description;

    public List<IngredientRequirement> requirements;
    public SkillEffect skillEffect;

    private void OnValidate()
    {
        var seen = new HashSet<IngredientType>();
        foreach (var req in requirements)
        {
            if (!seen.Add(req.type))
            {
                Debug.LogWarning($"[{recipeName}] {req.type} 타입이 중복 등록되어 있습니다!");
            }
        }
    }
}
