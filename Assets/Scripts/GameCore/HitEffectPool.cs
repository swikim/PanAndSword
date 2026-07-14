using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectPool : MonoBehaviour
{
    public static HitEffectPool Instance { get; private set; }
    [SerializeField]private ParticleSystem hitEffectPrefab;
    [SerializeField]private int poolSize = 10;

    private Queue<ParticleSystem> hitEffectPool; 

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        hitEffectPool = new Queue<ParticleSystem>();

        for(int i = 0; i < poolSize; i++)
        {
            ParticleSystem hitEffect = Instantiate(hitEffectPrefab, transform);
            hitEffect.gameObject.SetActive(false);
            hitEffectPool.Enqueue(hitEffect);
        }
    }
    void Start()
    {
        
    } 
    public void PlayHitEffect(Vector3 position)
    {
        if (hitEffectPool.Count == 0)
        {
            Debug.LogWarning("HitEffectPool: 풀이 부족합니다!");
            return;
        }

        ParticleSystem ps = hitEffectPool.Dequeue();
        ps.transform.position = position;
        ps.gameObject.SetActive(true);
        ps.Play();

        StartCoroutine(ReturnToPool(ps));
    }

    private IEnumerator ReturnToPool(ParticleSystem hitEffect)
    {
        yield return new WaitForSeconds(hitEffect.main.duration);
        hitEffect.gameObject.SetActive(false);
        hitEffectPool.Enqueue(hitEffect);
    }

}
