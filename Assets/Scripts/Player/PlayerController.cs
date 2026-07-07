using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;

public class PlayerController : MonoBehaviour,IDamageable
{
    [SerializeField] private Joystick joystick;
    private bool isUsingJoystick = true; 
    public bool isDashing = false;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDirection;

    public float attackDamage;

    [Header("Stats")]
    [SerializeField] private float maxHp = 100f;

    public float CurrentHp { get; private set; }
    public float MaxHp => maxHp;
    public bool IsDead { get; private set; }

    public event Action<float, float> OnHealthChanged; // current, max
    public event Action OnPlayerDied;
    
    
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        attackDamage = GameData.playerStatus.attackDamage;
        maxHp = GameData.playerStatus.maxHp;
        CurrentHp = maxHp;
        IsDead = false;
    }

    void Update()
    {
        if (IsDead) return;
        if(isUsingJoystick)
            MoveWithJoystick();
        else
            Move();
    }
    void FixedUpdate()
    {
        if(isDashing) return;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    void Move()
    {
        // 임시 입력: 키보드 (나중에 조이스틱으로 교체)
        float h = Input.GetAxisRaw("Horizontal"); // A/D, ←/→
        float v = Input.GetAxisRaw("Vertical");   // W/S, ↑/↓

        moveDirection = new Vector3(h, 0f, v).normalized;

        bool isMoving = moveDirection.magnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            // 이동 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    void MoveWithJoystick()
    {
        moveDirection = new Vector3(joystick.direction.x, 0f, joystick.direction.y).normalized;
        Debug.Log(moveDirection);

        bool isMoving = moveDirection.magnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            // 이동 방향으로 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void PlayAttackAnimation(WeaponType type)
    {
        if (IsDead) return;
        if(type == WeaponType.Pan)
        {
            animator.SetTrigger("Attack");
        }
        else
            animator.SetTrigger("SwordAttack");
    }
    public void TriggerAnimation(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
    public void TakeDamage(float damage)
    {
        if(IsDead) return;

        CurrentHp = Mathf.Max(0f, CurrentHp - damage);
        OnHealthChanged?.Invoke(CurrentHp, maxHp);
        Debug.Log(CurrentHp+"  "+ maxHp);

        if(CurrentHp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        IsDead = true;
        TriggerAnimation("Die");
        OnPlayerDied?.Invoke();
    }
}
