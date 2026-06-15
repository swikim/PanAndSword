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

    void Start()
    {
        weaponSwitcher = GetComponent<WeaponSwitcher>();
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
        int damage = (weaponSwitcher.currentWeapon ==WeaponType.Pan)? 15 : 8;
        enemy.TakeDamage(damage);
    }
}
