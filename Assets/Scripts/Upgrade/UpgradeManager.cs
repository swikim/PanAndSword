using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    public void Eat(RecipeData recipe)
    {
        if (!HasFood(recipe)) return;

        switch (recipe.skillEffect)
        {
            case SkillEffect.FlameOnAttack:     GameData.cookData.bulgogi--;   break;
            case SkillEffect.HealOnMove:        GameData.cookData.salad--;     break;
            case SkillEffect.AttackBoostPercent: GameData.cookData.steak--;    break;
            case SkillEffect.AllStatBoost:      GameData.cookData.bibimbap--;  break;
            case SkillEffect.CooldownReduction: GameData.cookData.spicySoup--; break;
        }

        ApplyStat(recipe);
        SaveManager.Instance.SaveGameData();
    }
    public bool HasFood(RecipeData recipe)
    {
        return recipe.skillEffect switch
        {
            SkillEffect.FlameOnAttack     => GameData.cookData.bulgogi >= 1,
            SkillEffect.HealOnMove        => GameData.cookData.salad >= 1,
            SkillEffect.AttackBoostPercent => GameData.cookData.steak >= 1,
            SkillEffect.AllStatBoost      => GameData.cookData.bibimbap >= 1,
            SkillEffect.CooldownReduction  => GameData.cookData.spicySoup >= 1,
            _                             => false
        };
    }
    private void ApplyStat(RecipeData recipe)
    {
        switch (recipe.skillEffect)
        {
            case SkillEffect.FlameOnAttack:
                GameData.playerStatus.attackDamage += 10;
                break;
            case SkillEffect.HealOnMove:
                Debug.Log("재생력 ++ (추후 구현)");
                break;
            case SkillEffect.AttackBoostPercent:
                GameData.playerStatus.attackDamage *= 1.1f;
                break;
            case SkillEffect.AllStatBoost:
                GameData.playerStatus.attackDamage += 10;
                GameData.playerStatus.maxHp += 10;
                GameData.playerStatus.skillCooldown -= 0.1f;
                break;
            case SkillEffect.CooldownReduction:
                GameData.playerStatus.skillCooldown -= 0.1f;
                break;
        }
    }

    
    private void UpdateUI()
    {
        
    }
}
