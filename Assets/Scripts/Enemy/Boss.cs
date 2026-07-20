using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private GameObject minotosVisual; 
    [SerializeField] private GameObject extinguisherObject;
    private List<IAttackPattern> attackPatterns;
    private float lastAttackTime;
    [SerializeField] private float bossHp;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float judgeRange = 10f;
    [SerializeField] public float dashSpeed = 10f;
    [SerializeField] public float dashDuration = 0.5f;
    [SerializeField] public float dashDamage = 20f;
    [SerializeField] private float aoeWarningDuration = 1f;
    [SerializeField] private float aoeExplosionRadius = 3f;
    [SerializeField] private float aoeDamage = 15f;
    public GameObject aoeWarningPrefab;
    private bool isPhaseTwo = false;

    protected override void Start()
    {
        base.Start(); 
        OnHpChanged += CheckPhaseTransition;
        moveSpeed = 1f;
        maxHP = bossHp;
        currentHP = maxHP;
        attackPatterns = new List<IAttackPattern>
        {
            new MeleeAttackPattern(dashSpeed, dashDuration, dashDamage),
            new AoeAttackPattern(aoeWarningDuration,aoeExplosionRadius,aoeDamage,aoeWarningPrefab)
        };
    }
    
    void Update()
    {
        if(attackPatterns.Any(pattern => pattern.IsRunning)) return;
        float distance = Vector3.Distance(transform.position, player.position);
        if(Time.time - lastAttackTime >= attackCooldown)
        {
            if(distance > judgeRange)
            {
                foreach(IAttackPattern attackPattern in attackPatterns)
                {
                    if(attackPattern is AoeAttackPattern)
                    {
                        attackPattern.Execute(player,this);
                        lastAttackTime = Time.time;
                        break;
                    }
                }
            }
            else
            {
                foreach(IAttackPattern attackPattern in attackPatterns)
                {
                    if(attackPattern is MeleeAttackPattern)
                    {
                        attackPattern.Execute(player,this);
                        lastAttackTime = Time.time;
                        break;
                    }
                }
            }
        }
    }

    private void CheckPhaseTransition(float currentHp, float maxHp)
    {
        if (!isPhaseTwo && currentHp <= maxHp * 0.5f)
        {
            isPhaseTwo = true;
            attackCooldown *= 0.8f; // 공격 쿨다운 감소
            attackPatterns = new List<IAttackPattern>
            {
                new MeleeAttackPattern(dashSpeed * 1.15f, dashDuration, dashDamage * 1.2f),
                new AoeAttackPattern(aoeWarningDuration * 0.85f, aoeExplosionRadius * 1.15f, aoeDamage * 1.1f, aoeWarningPrefab)
            };
        }
    }
    protected override void Die()
    {
        Debug.Log("Boss 사망!");
        enabled = false;
        TryDrop();
        
        extinguisherObject.SetActive(true);
        extinguisherObject.transform.position = gameObject.transform.position;
        minotosVisual.SetActive(false);
    }
}