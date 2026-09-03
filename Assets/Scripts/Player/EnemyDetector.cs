using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private ParticleSystem attackEffect;
    [SerializeField]private Vector3 effectPositionOffset = new Vector3(0f, 1f, 0f);
    public float detectRange = 5f; // 감지 범위
    public float attackInterval = 1f; // 공격 주기
    public float closeAttackRange = 1.5f; 
    public float closeAttackAngle = 65f; // 근접 공격 범위 각도 
    public float attackAngle = 45f; // 공격 범위 각도 
    
    

    public Transform currentTarget;
    private WeaponSwitcher weaponSwitcher;
    private PlayerController playerController;
    [SerializeField] private HitEffectPool hitEffectPool;

    void Start()
    {
        weaponSwitcher = GetComponent<WeaponSwitcher>();
        playerController = GetComponent<PlayerController>();
        hitEffectPool = HitEffectPool.Instance;
        StartCoroutine(AutoAttackRoutine());   
    }
    

    void FindNearestEnemy()
    {
        Transform nearest = null;
        float minSqrDistance = detectRange * detectRange;

        foreach (Enemy enemy in EnemyRegistry.Instance.ActiveEnemies)
        {
            float sqrDistance = (enemy.transform.position - transform.position).sqrMagnitude;

            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                nearest = enemy.transform;
            }
        }
        
        currentTarget = nearest;
    }
    public bool HasTarget()
    {
        return currentTarget != null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Vector3 rightBoundary = Quaternion.Euler(0, attackAngle, 0) * transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -attackAngle, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectRange);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, closeAttackRange);
        Vector3 forwardBoundary = Quaternion.Euler(0, closeAttackAngle, 0) * transform.forward;
        Vector3 backwardBoundary = Quaternion.Euler(0, -closeAttackAngle, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + forwardBoundary * closeAttackRange);
        Gizmos.DrawLine(transform.position, transform.position + backwardBoundary * closeAttackRange);
    }
    

    IEnumerator AutoAttackRoutine()
    {
        while (true)
        {
            FindNearestEnemy();

            if(currentTarget != null)
                Attack(currentTarget);

            yield return new WaitForSeconds(attackInterval);
        }
    }

    void Attack(Transform target)
    {
        Vector3 toTarget = target.position - transform.position;
        float distance = toTarget.magnitude;
        Vector3 direction = toTarget.normalized;
        float angle = Vector3.Angle(transform.forward, direction);
        Enemy enemy = target.GetComponent<Enemy>();

        bool isInCloseRange = distance <= closeAttackRange && angle < closeAttackAngle;
        bool isInFanRange = distance <= detectRange && angle < attackAngle;

        if (enemy == null) return;

        if(isInCloseRange || isInFanRange)
        {
            float multiplier = (weaponSwitcher.currentWeapon == WeaponType.Pan) ? 1.5f : 0.8f;
            int damage = Mathf.RoundToInt(playerController.attackDamage * multiplier);
            enemy.TakeDamage(damage);
            playerController.PlayAttackAnimation(weaponSwitcher.currentWeapon);
            
            hitEffectPool.PlayHitEffect(target.transform.position + effectPositionOffset);
            Debug.Log("Damage : " + damage);
        }
        
       
    }
    
    
}
