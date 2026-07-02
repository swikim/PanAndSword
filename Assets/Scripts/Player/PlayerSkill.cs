using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float skillCooldown = 8f;
    private float lastSkillTime = -999f;

    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public int dashDamage = 25;
    public float oilSplashDamage;

    [SerializeField] private LayerMask enemyLayer;

    private WeaponSwitcher weaponSwitcher;
    private PlayerController playerController;
    private Animator animator;
    private Rigidbody rb;

    void Start()
    {
        weaponSwitcher = GetComponent<WeaponSwitcher>(); 
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();  
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryUseSkill();
        }
    }

    void TryUseSkill()
    {
        if (playerController.IsDead) return;
        if(Time.time - lastSkillTime < skillCooldown)
        {
            float remaining = skillCooldown - (Time.time - lastSkillTime);
            Debug.Log("스킬 쿨타임 남음: " + remaining.ToString("F1") + "초");
            return;
        }
        UseSkill();
        lastSkillTime = Time.time;
    }

    void UseSkill()
    {
        Debug.Log("Useskill");
        if(weaponSwitcher.currentWeapon == WeaponType.Pan)
            OilSplash();
        else
            SwordSlash();
    }

    void OilSplash()
    {
        Debug.Log("🍳 Oil Toss 발동! 주변 적에게 광역 데미지");
        animator.SetTrigger("OilSplash");

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 3f,enemyLayer);

        oilSplashDamage = GameData.playerStatus.attackDamage * 0.8f;
        foreach (Collider col in hitEnemies)
        {
            if(col.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(oilSplashDamage);
            }
        }
    }
    void SwordSlash()
    {
        Debug.Log("⚔️ Dash Slash 발동! 돌진 베기");
        animator.SetTrigger("SwordSlash");
        StartCoroutine(DashRoutine());
    }

    IEnumerator DashRoutine()
    {
        playerController.isDashing = true;

        Vector3 dashDirection = transform.forward;

        float elapsed = 0f;

        while(elapsed < dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        playerController.isDashing = false;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 1.5f, enemyLayer);

        foreach(Collider col in hitEnemies)
        {
            if(col.TryGetComponent<IDamageable>(out var target))
            {
                target.TakeDamage(dashDamage);
            }
        }
    }
}
