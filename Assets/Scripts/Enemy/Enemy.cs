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
    
    public EnemyData enemyData;
    public event System.Action<float,float> OnHpChanged;
    private IAttackPattern attackPattern;
    private float lastAttackTime;

    protected virtual void Start()
    {
        maxHP = enemyData.maxHP;
        currentHP = maxHP;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        OnHpChanged?.Invoke(currentHP,maxHP);

        PlayerController playerController  = player.GetComponent<PlayerController>();
        attackPattern = new NormalAttackPattern(enemyData.attackRange,enemyData.attackDamage,playerController);
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

        if (Time.time - lastAttackTime >= enemyData.attackCooldown)
        {
            attackPattern.Execute(player, this);
            lastAttackTime = Time.time;
        }
    }
    protected virtual void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if(distance <= enemyData.chaseRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * enemyData.moveSpeed * Time.fixedDeltaTime);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHP -= damage;
        OnHpChanged?.Invoke(currentHP,enemyData.maxHP);

        if(currentHP <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
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
