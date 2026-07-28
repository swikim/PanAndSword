using System.Collections.Generic;
using UnityEngine;

public enum AttackType { Melee, Ranged }

[CreateAssetMenu(menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("공통")]
    public string enemyName;
    public float maxHP;
    public float moveSpeed;
    public float chaseRange;
    public List<IngredientData> dropTable;
    public AttackType attackType;

    [Header("일반 몬스터 - 근접 공격")]
    public float attackRange;
    public float attackDamage;
    public float attackCooldown;
    [Header("원거리 몬스터 전용")]
    public float projectileSpeed;
    public GameObject projectilePrefab;

    [Header("보스 전용 - 패턴 전환")]
    public float bossAttackCooldown;
    public float judgeRange;

    [Header("보스 전용 - Melee(Dash) 패턴")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDamage;

    [Header("보스 전용 - AOE 패턴")]
    public float aoeWarningDuration;
    public float aoeExplosionRadius;
    public float aoeDamage;
}