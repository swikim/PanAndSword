using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private float attackCooldown = 3f;
    private List<IAttackPattern> attackPatterns;
    private float lastAttackTime;
    [SerializeField] private float judgeRange = 10f;

    protected override void Start()
    {
        base.Start(); 
        base.attackCooldown = 3f;
        attackPatterns = new List<IAttackPattern>
        {
            new MeleeAttackPattern(),
            new AoeAttackPattern()
        };
    }
    
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if(Time.time - lastAttackTime >= attackCooldown)
        {
            if(distance > judgeRange)
            {
                foreach(IAttackPattern attackPattern in attackPatterns)
                {
                    if(attackPattern is AoeAttackPattern)
                    {
                        attackPattern.Execute(player);
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
                        attackPattern.Execute(player);
                        lastAttackTime = Time.time;
                        break;
                    }
                }
            }
        }
    }
}