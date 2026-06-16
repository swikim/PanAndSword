using UnityEngine;

public enum WeaponType
{
    Pan,
    Sword
}

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject panObject;
    public GameObject swordObject;

    public WeaponType currentWeapon = WeaponType.Pan;

    void Start()
    {
        UpdateWeaponVisual();
    }

    void Update()
    {
        // 임시 입력: Tab 키 (나중에 UI 버튼으로 교체)
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchWeapon();
        }
    }

    void SwitchWeapon()
    {
        currentWeapon = (currentWeapon == WeaponType.Pan) ? WeaponType.Sword : WeaponType.Pan;
        UpdateWeaponVisual();

        Debug.Log("현재 무기: " + currentWeapon);
    }

    void UpdateWeaponVisual()
    {
        panObject.SetActive(currentWeapon == WeaponType.Pan);
        swordObject.SetActive(currentWeapon == WeaponType.Sword);
    }
}