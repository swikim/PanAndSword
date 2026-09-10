using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUI : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay;
    
    [SerializeField] private TMP_Text cooldownText;
    [SerializeField] private Button skillButton;
    [SerializeField] private Image skillButtonIcon;
    [SerializeField] private Button weaponSwitchButton;
    [SerializeField] private Skill skill;
    [SerializeField] private WeaponSwitcher weaponSwitcher;
    
    void Start()
    {
        skillButton.onClick.AddListener(() => skill.TryUseSkill());
        skillButton.image.sprite = skill.GetCurrentSkillIcon();
        weaponSwitchButton.onClick.AddListener(() => weaponSwitcher.SwitchWeapon());
    }

    void Update()
    {
        float remaining = skill.GetCooldownRemaining();
        float progress = remaining / skill.GetMaxCooldown();

        cooldownOverlay.fillAmount = progress;
        cooldownOverlay.gameObject.SetActive(remaining > 0f);
        cooldownText.text = remaining > 0 ? remaining.ToString("F1") : "";

        skillButtonIcon.sprite = skill.GetCurrentSkillIcon();
    }
    

}
