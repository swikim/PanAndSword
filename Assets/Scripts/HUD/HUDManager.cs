using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField]private Slider hpBar;
    [SerializeField] private PlayerController player;

    void Start()
    {
        if (player != null)
        {
            player.OnHealthChanged += UpdateHpBar;
            UpdateHpBar(player.CurrentHp, player.MaxHp);
        }
        else
        {
            Debug.LogWarning("player가 인스펙터에 할당 안 됨!");
        }
    }

    void LateUpdate()
    {
        hpBar.transform.rotation = Camera.main.transform.rotation;
    }
    void UpdateHpBar(float currentHp, float maxHp)
    {
        hpBar.value = currentHp/maxHp;
        Debug.Log(currentHp + "  "+ maxHp);
    }

    void OnDestroy()
    {
        if(player != null)
        {
            player.OnHealthChanged -= UpdateHpBar;
        }
    }
}
