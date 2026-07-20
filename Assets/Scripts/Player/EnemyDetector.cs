using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private ParticleSystem attackEffect;
    [SerializeField]private Vector3 effectPositionOffset = new Vector3(0f, 1f, 0f);
    public float detectRange = 5f; // 감지 범위
    public float attackInterval = 1f; // 공격 주기

    private Transform currentTarget;
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
        Vector3 rightBoundary = Quaternion.Euler(0,40f,0) * transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0,-40f,0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectRange);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectRange);
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
        Vector3 toTarget = target.position - transform.position;
        float distance = toTarget.magnitude;
        Vector3 direction = toTarget.normalized;
        float angle = Vector3.Angle(transform.forward, direction);
        Enemy enemy = target.GetComponent<Enemy>();

        if (enemy == null) return;
        if(angle < 40f && toTarget.magnitude <= 50f)
        {
             float multiplier = (weaponSwitcher.currentWeapon == WeaponType.Pan) ? 1.5f : 0.8f;
            int damage = Mathf.RoundToInt(playerController.attackDamage * multiplier);
            enemy.TakeDamage(damage);
            playerController.PlayAttackAnimation(weaponSwitcher.currentWeapon);
            
            hitEffectPool.PlayHitEffect(target.transform.position + effectPositionOffset);
            Debug.Log("Damage : "+damage);
        }
       
    }
    
    
}
