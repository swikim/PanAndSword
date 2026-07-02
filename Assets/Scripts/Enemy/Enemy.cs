using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamageable
{
    public float maxHP = 30;
    private float currentHP;
    
    public float moveSpeed = 2f;
    public float chaseRange = 8f;

    private Transform player;
    private Rigidbody rb;
    [SerializeField] private List <IngredientData> dropTable;
    public event System.Action<float,float> OnHpChanged;

    void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        OnHpChanged?.Invoke(currentHP,maxHP);
    }
    void FixedUpdate()
    {
        ChasePlayer();
    }
    void ChasePlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if(distance <= chaseRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        OnHpChanged?.Invoke(currentHP,maxHP);

        if(currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log(gameObject.name + " 사망!");
        TryDrop();
        Destroy(gameObject);
    }
    void TryDrop()
    {
        foreach(var data in dropTable)
        {
            if(Random.value <= data.dropRate)
            {
                IngredientPool.Instance.Get(data,transform.position);
                Debug.Log(data.name);
            }
        }
    }
}
