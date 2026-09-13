using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossHealthBar : MonoBehaviour
{
    [SerializeField]private Slider hpBar;
    private Image fillImage;
    private Enemy targetEnemy;
    void Awake()
    {
        fillImage = hpBar.fillRect.GetComponent<Image>(); 
    }
    public void Init(Enemy enemy)
    {
        targetEnemy = enemy;
        targetEnemy.OnHpChanged += UpdateHpBar;
        UpdateHpBar(enemy.CurrentHP, enemy.maxHP);
    }
    public void UpdateHpBar(float currentHp, float maxHp)
    {
        hpBar.value = currentHp / maxHp;
        fillImage.color = currentHp <= maxHp * 0.5f ? Color.red : Color.green;
    }
    void OnDestroy()
    {
        if(targetEnemy != null)
        {
            targetEnemy.OnHpChanged -= UpdateHpBar;
        }
    }
}
