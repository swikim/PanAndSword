using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float attackCooldown = 1.5f;

    private PlayerController player;
    private float lastAttackTime;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if(player == null || player.IsDead) return;
        float dist = Vector3.Distance(player.transform.position,transform.position);
        if(dist <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            player.TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
    }

}
