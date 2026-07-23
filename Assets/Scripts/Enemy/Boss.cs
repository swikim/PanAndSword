using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private GameObject minotosVisual; 
    [SerializeField] private GameObject extinguisherObject;
    private List<IAttackPattern> attackPatterns;
    private float bossLastAttackTime;
    private float currentAttackCooldown;
    public GameObject aoeWarningPrefab;
    private bool isPhaseTwo = false;

    protected override void Start()
    {
        base.Start(); 
        OnHpChanged += CheckPhaseTransition;
        
        currentAttackCooldown = enemyData.bossAttackCooldown;
        attackPatterns = new List<IAttackPattern>
        {
            new MeleeAttackPattern(enemyData.dashSpeed, enemyData.dashDuration, enemyData.dashDamage),
            new AoeAttackPattern(enemyData.aoeWarningDuration, enemyData.aoeExplosionRadius, enemyData.aoeDamage, aoeWarningPrefab)
        };
    }
    
    void Update()
    {
        if(attackPatterns.Any(pattern => pattern.IsRunning)) return;
        float distance = Vector3.Distance(transform.position, player.position);
        if(Time.time - bossLastAttackTime >= currentAttackCooldown)
        {
            if(distance > enemyData.judgeRange)
            {
                foreach(IAttackPattern attackPattern in attackPatterns)
                {
                    if(attackPattern is AoeAttackPattern)
                    {
                        attackPattern.Execute(player,this);
                        bossLastAttackTime = Time.time;
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
                        bossLastAttackTime = Time.time;
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
            currentAttackCooldown *= 0.8f; 

            attackPatterns = new List<IAttackPattern>
            {
                new MeleeAttackPattern(enemyData.dashSpeed * 1.15f, enemyData.dashDuration, enemyData.dashDamage * 1.2f),
                new AoeAttackPattern(enemyData.aoeWarningDuration * 0.85f, enemyData.aoeExplosionRadius * 1.15f, enemyData.aoeDamage * 1.1f, aoeWarningPrefab)
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