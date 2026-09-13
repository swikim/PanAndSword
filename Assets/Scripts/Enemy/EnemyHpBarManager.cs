using UnityEngine;

public class EnemyHpBarManager : MonoBehaviour
{
    public static EnemyHpBarManager Instance {get; private set;}

    [SerializeField]private GameObject hpBarPrefab;
    [SerializeField]private GameObject bossHpBarPrefab;
    [SerializeField] private Transform hpBarContainer;      // World Space Canvas
    [SerializeField] private Transform bossHpBarContainer;  // Screen Space Canvas

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public EnemyHealthBar CreateHPbar(Enemy enemy)
    {
        GameObject obj = Instantiate(hpBarPrefab,hpBarContainer);
        EnemyHealthBar hpBar = obj.GetComponent<EnemyHealthBar>();
        hpBar.Init(enemy);
        return hpBar;
    }
    public BossHealthBar CreateBossHPbar(Enemy enemy)
    {
        GameObject obj = Instantiate(bossHpBarPrefab,bossHpBarContainer);
        BossHealthBar hpBar = obj.GetComponent<BossHealthBar>();
        hpBar.Init(enemy);
        return hpBar;
    }
}
