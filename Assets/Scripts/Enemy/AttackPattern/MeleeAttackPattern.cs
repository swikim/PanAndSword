using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackPattern : IAttackPattern
{
    private float dashSpeed;
    private float dashDuration;
    private float dashDamage;
    public bool IsRunning { get; private set; }
    public void Execute(Transform player,MonoBehaviour owner)
    {
        owner.StartCoroutine(DashRoutine(player,owner));
    }
    public MeleeAttackPattern(float dashSpeed, float dashDuration, float dashDamage)
    {
        this.dashSpeed = dashSpeed;
        this.dashDuration = dashDuration;
        this.dashDamage = dashDamage;
    }
    private IEnumerator DashRoutine(Transform target, MonoBehaviour owner)
    {
        IsRunning = true;
        Vector3 dashDirection = (target.position - owner.transform.position).normalized;
        
        float elapsed = 0f;
        while(elapsed < dashDuration)
        {
            owner.transform.position += dashDirection * dashSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
        IsRunning = false;

        if(target.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(dashDamage);
        }
        yield return new WaitForSeconds(0.3f);
    }
}
