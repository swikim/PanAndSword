using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public float detectRange = 5f; // 감지 범위
    public float attackInterval = 1f; // 공격 주기

    private Transform currentTarget;
    private WeaponSwitcher weaponSwitcher;
    private PlayerController playerController;

    void Start()
    {
        weaponSwitcher = GetComponent<WeaponSwitcher>();
        playerController = GetComponent<PlayerController>();
        StartCoroutine(AutoAttackRoutine());   
    }
    void Update()
    {
        FindNearestEnemy();
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearest = null;
        float minDistance = detectRange;

        foreach(GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if(distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy.transform;
            }
        }
        
        currentTarget = nearest;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    IEnumerator AutoAttackRoutine()
    {
        while (true)
        {
            if(currentTarget != null)
                Attack(currentTarget);

            yield return new WaitForSeconds(attackInterval);
        }
    }

    void Attack(Transform target)
    {
        Enemy enemy = target.GetComponent<Enemy>();

        if (enemy == null) return;
        float multiplier = (weaponSwitcher.currentWeapon == WeaponType.Pan) ? 1.5f : 0.8f;
        int damage = Mathf.RoundToInt(playerController.attackDamage * multiplier);
        enemy.TakeDamage(damage);
        playerController.PlayAttackAnimation(weaponSwitcher.currentWeapon);
        Debug.Log("Damage : "+damage);
    }
}
