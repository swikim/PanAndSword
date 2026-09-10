using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillSlot
{
    public WeaponType weaponType;
    public float cooldown;
    public float lastUsedTime = -999f;
    public Sprite skillIcon;
}
public class Skill : MonoBehaviour
{

    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public int dashDamage = 25;
    public float oilSplashDamage;
    [SerializeField] private List<SkillSlot> skillSlots;

    [SerializeField] private LayerMask enemyLayer;

    private WeaponSwitcher weaponSwitcher;
    [SerializeField] private DashHitBox dashHitBoxPrefab;
    private PlayerController playerController;
    private Animator animator;
    private Rigidbody rb;

    void Start()
    {
        weaponSwitcher = GetComponent<WeaponSwitcher>(); 
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();  
        rb = GetComponent<Rigidbody>();
        dashHitBoxPrefab.Deactivate();

        foreach(SkillSlot slot in skillSlots)
        {
            slot.lastUsedTime = -999f;
            slot.cooldown = GameData.playerStatus.skillCooldown;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryUseSkill();
        }
    }
    SkillSlot GetCurrentSkillSlot()
    {
        return skillSlots.Find(slot => slot.weaponType == weaponSwitcher.currentWeapon);
    }

    public void TryUseSkill()
    {
        if (playerController.IsDead) return;
        SkillSlot currentSlot = GetCurrentSkillSlot();
        if (currentSlot == null) return;

        if(Time.time - currentSlot.lastUsedTime < currentSlot.cooldown)
        {
            float remaining = currentSlot.cooldown - (Time.time - currentSlot.lastUsedTime);
            Debug.Log("스킬 쿨타임 남음: " + remaining.ToString("F1") + "초");
            return;
        }
        UseSkill();
        currentSlot.lastUsedTime = Time.time;
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
        dashHitBoxPrefab.Activate(dashDamage);

        float elapsed = 0f;

        while(elapsed < dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        playerController.isDashing = false;

        dashHitBoxPrefab.Deactivate();
    }
    public float GetCooldownRemaining()
    {
        SkillSlot slot = GetCurrentSkillSlot();
        if (slot == null) return 0f;
        return Mathf.Max(0f, slot.cooldown - (Time.time - slot.lastUsedTime));
    }

    public float GetMaxCooldown()
    {
        return GetCurrentSkillSlot()?.cooldown ?? 0f;
    }
    public Sprite GetCurrentSkillIcon()
    {
        return GetCurrentSkillSlot().skillIcon;
    }
}
