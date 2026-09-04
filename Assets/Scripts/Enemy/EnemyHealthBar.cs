using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]private Slider hpBar;
    private Enemy targetEnemy;
    private Transform targetTransform;
    private Vector3 worldOffset = new Vector3(0, 2f, 0);

    void LateUpdate()
    {
        if(targetTransform == null) 
        {
            Destroy(gameObject); 
            return;
        }
        transform.position = targetTransform.position + worldOffset;
        transform.rotation = Camera.main.transform.rotation;
    }
    public void Init(Enemy enemy)
    {
        targetEnemy = enemy;
        targetTransform = enemy.transform;
        Debug.Log(targetTransform.position);
        targetEnemy.OnHpChanged += UpdateHpBar;
    }

    void UpdateHpBar(float currentHp, float maxHp)
    {
        hpBar.value = (float)currentHp / maxHp;
    }

    void OnDestroy()
    {
        if (targetEnemy != null)
        {
            targetEnemy.OnHpChanged -= UpdateHpBar;
        }
    }
}
