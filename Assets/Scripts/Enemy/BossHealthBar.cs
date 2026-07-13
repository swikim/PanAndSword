using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossHealthBar : MonoBehaviour
{
    [SerializeField]private Slider hpBar;
    private Image fillImage;
    private Boss boss;
    void Awake()
    {
        boss = GetComponent<Boss>();
        fillImage = hpBar.fillRect.GetComponent<Image>(); 
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
        hpBar.value = currentHp / maxHp;
        fillImage.color = currentHp <= maxHp * 0.5f ? Color.red : Color.green;
    }
    void OnDestroy()
    {
        if(boss != null)
        {
            boss.OnHpChanged -= UpdateHpBar;
        }
    }
}
