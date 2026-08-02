using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamageable
{
    public float maxHP;
    protected float currentHP;
    public float CurrentHP => currentHP;
    protected Transform player;
    protected Rigidbody rb;
    private float rotationSpeed = 10f;
    
    public EnemyData enemyData;
    public event System.Action<float,float> OnHpChanged;
    private IAttackPattern attackPattern;
    private float lastAttackTime;
    protected Animator animator;
   
    

    protected virtual void Start()
    {
        maxHP = enemyData.maxHP;
        currentHP = maxHP;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        EnemyHpBarManager.Instance.CreateHPbar(this);
        OnHpChanged?.Invoke(currentHP,maxHP);

        PlayerController playerController  = player.GetComponent<PlayerController>();
        if(enemyData.attackType == AttackType.Melee)
        {
            attackPattern = new NormalAttackPattern(enemyData.attackRange,enemyData.attackDamage,playerController);
        }
        else
        {
            attackPattern = new RangedAttackPattern(enemyData.attackDamage,enemyData.projectileSpeed,enemyData.projectilePrefab,playerController);
        }
        
    }
    void FixedUpdate()
    {
        ChasePlayer();
    }
    void Update()
    {
        TryAttack();
    }
    public void TryAttack()
    {
        if(attackPattern.IsRunning) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (Time.time - lastAttackTime >= enemyData.attackCooldown && distance <= enemyData.attackRange)
        {
            attackPattern.Execute(player, this);
            lastAttackTime = Time.time;
            animator.SetTrigger(AnimParam.Attack.ToString());
        }
    }
    protected virtual void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        bool isMoving = distance <= enemyData.chaseRange;
        if(isMoving)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * enemyData.moveSpeed * Time.fixedDeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);            
        }
        animator.SetBool(AnimParam.IsMoving.ToString(), isMoving);
    }
    

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;
        OnHpChanged?.Invoke(currentHP,enemyData.maxHP);
        animator.SetTrigger(AnimParam.Damaged.ToString());
        if(currentHP <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        animator.SetTrigger(AnimParam.Death.ToString());
        Debug.Log(gameObject.name + " 사망!");
        TryDrop();
        Destroy(gameObject);
    }
    protected virtual void TryDrop()
    {
        foreach(var data in enemyData.dropTable)
        {
            if(Random.value <= data.dropRate)
            {
                IngredientPool.Instance.Get(data,transform.position);
                Debug.Log(data.name);
            }
        }
    }
    
}
