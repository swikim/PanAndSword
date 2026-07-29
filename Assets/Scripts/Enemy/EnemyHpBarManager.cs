using UnityEngine;

public class EnemyHpBarManager : MonoBehaviour
{
    public static EnemyHpBarManager Instance {get; private set;}

    [SerializeField]private GameObject hpBarPrefab;
    [SerializeField]private Transform hpBarContainer;

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
}
