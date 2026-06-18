using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP = 30;
    private int currentHP;
    
    public float moveSpeed = 2f;
    public float chaseRange = 8f;

    private Transform player;
    private Rigidbody rb;

    void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        Debug.Log(gameObject.name + "Hit!" + currentHP);

        if(currentHP <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log(gameObject.name + " 사망!");
        Destroy(gameObject);
    }
}
