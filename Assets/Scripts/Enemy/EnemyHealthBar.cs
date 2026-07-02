using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]private Slider hpBar;
    private Enemy enemy;
    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    void Start()
    {
        if (enemy != null)
        {
            enemy.OnHpChanged += UpdateHpBar;
        }
    }

    void UpdateHpBar(float currentHp, float maxHp)
    {
        hpBar.value = (float)currentHp / maxHp;
    }

    void OnDestroy()
    {
        if (enemy != null)
        {
            enemy.OnHpChanged -= UpdateHpBar;
        }
    }
}
