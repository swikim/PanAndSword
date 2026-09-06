using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonUI : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private Image oilSplashImage;
    [SerializeField] private Image dashImage;
    [SerializeField] private TMP_Text cooldownText;
    [SerializeField] private Button skillButton;
    [SerializeField] private Button weaponSwitchButton;
    [SerializeField] private Skill skill;
    [SerializeField] private WeaponSwitcher weaponSwitcher;
    
    void Start()
    {
        skillButton.onClick.AddListener(() => skill.TryUseSkill());
        weaponSwitchButton.onClick.AddListener(() => weaponSwitcher.SwitchWeapon());
    }

    void Update()
    {
        float remaining = skill.GetCooldownRemaining();
        float cooldown = skill.skillCooldown;

        cooldownOverlay.fillAmount = remaining / cooldown;
        cooldownOverlay.gameObject.SetActive(remaining > 0f);
        cooldownText.text = remaining > 0 ? remaining.ToString("F1") : "";
    }
    public void SwichButtonImage(WeaponType weaponType)
    {
        if(weaponType == WeaponType.Pan)
        {
            oilSplashImage.gameObject.SetActive(true);
            dashImage.gameObject.SetActive(false);
        }
        else
        {
            oilSplashImage.gameObject.SetActive(false);
            dashImage.gameObject.SetActive(true);
        }
    }

}
