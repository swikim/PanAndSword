using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackPattern : IAttackPattern
{
    private readonly float attackDamage;
    private readonly float projectileSpeed;
    private readonly GameObject projectilePrefab;
    private readonly PlayerController playerController;
    private  Vector3 spawnOffset = new Vector3(1,1,0f); 

    public bool IsRunning => false;

    public RangedAttackPattern(float attackDamage, float projectileSpeed, GameObject projectilePrefab, PlayerController playerController)
    {
        this.attackDamage = attackDamage;
        this.projectileSpeed = projectileSpeed;
        this.projectilePrefab = projectilePrefab;
        this.playerController = playerController;
    }

    public void Execute(Transform player, MonoBehaviour owner)
    {
        if (player == null || playerController.IsDead) return;

        Vector3 direction = (player.position - owner.transform.position).normalized;
        Vector3 spawnPos = owner.transform.position + spawnOffset; 

        GameObject projectile = GameObject.Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(direction));
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.InitObj(direction, projectileSpeed, attackDamage);
    }
}