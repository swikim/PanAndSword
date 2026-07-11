using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AoeAttackPattern : IAttackPattern
{
    public bool IsRunning { get; private set; }
    private float warningDuration;
    private float explosionRadius;
    private float aoeDamage;
    private GameObject warningPrefab;
    public AoeAttackPattern(float warningDuration,float explosionRadius,float aoeDamage,GameObject warningPrefab)
    {
        this.warningDuration = warningDuration;
        this.explosionRadius = explosionRadius;
        this.aoeDamage = aoeDamage;
        this.warningPrefab = warningPrefab;
    }
    public void Execute(Transform player,MonoBehaviour owner)
    {
        owner.StartCoroutine(AoeRoutine(player,owner));
    }
    
    private IEnumerator AoeRoutine(Transform target, MonoBehaviour owner)
    {
        IsRunning = true;
        Vector3 warningPos = new Vector3(target.position.x,0.1f,target.position.z);
        GameObject warnigEffect = GameObject.Instantiate(warningPrefab,warningPos,Quaternion.identity);

        yield return new WaitForSeconds(warningDuration);

        GameObject.Destroy(warnigEffect);
        Collider[] hits = Physics.OverlapSphere(warningPos, explosionRadius);
        foreach(Collider hit in hits)
        {
            if(hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(aoeDamage);
            }
        }

        // TODO: 파티클 이펙트 추가
        IsRunning = false;
    }
}
