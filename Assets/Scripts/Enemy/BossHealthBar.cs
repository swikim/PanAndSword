using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossHealthBar : MonoBehaviour
{
    [SerializeField]private Slider hpBar;
    private Boss boss;
    void Awake()
    {
        boss = GetComponent<Boss>();
    }
    void Start()
    {
        if (boss != null)
        {
            boss.OnHpChanged += UpdateHpBar;
            UpdateHpBar(boss.CurrentHP, boss.maxHP);
        }
    }  
    private void UpdateHpBar(float currentHp, float maxHp)
    {
        hpBar.value = (float)currentHp / maxHp;
    }
    void OnDestroy()
    {
        if(boss != null)
        {
            boss.OnHpChanged -= UpdateHpBar;
        }
    }
}
