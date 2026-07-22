using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackPattern : IAttackPattern
{
    private float attackRange;
    private float attackDamage;
    public bool IsRunning => false;
    public PlayerController playerController;

    public NormalAttackPattern(float attackRange, float attackDamage, PlayerController playerController)
    {
        this.attackRange = attackRange;
        this.attackDamage = attackDamage;
        this.playerController = playerController;
    }

    public void Execute(Transform player,MonoBehaviour owner)
    {
        if (playerController == null || playerController.IsDead) return;

        float dist = Vector3.Distance(player.position, owner.transform.position);
        if (dist <= attackRange)
        {
            playerController.TakeDamage(attackDamage);
        }
    }
}
