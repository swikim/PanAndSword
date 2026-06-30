using UnityEngine;

public enum WeaponType
{
    Pan,
    Sword
}

public class WeaponSwitcher : MonoBehaviour
{
    PlayerController playerController;
    public GameObject panObject;
    public GameObject swordObject;

    public WeaponType currentWeapon = WeaponType.Pan;
    [SerializeField] private float switchCooldown = 0.5f;
    private float lastSwitchTime = -999f;


    void Start()
    {
        playerController = GetComponent<PlayerController>();
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
        if(Time.time - lastSwitchTime < switchCooldown) return;
        lastSwitchTime = Time.time;

        currentWeapon = (currentWeapon == WeaponType.Pan) ? WeaponType.Sword : WeaponType.Pan;
        UpdateWeaponVisual();

        playerController.TriggerAnimation("SwitchWeapon");
        Debug.Log("현재 무기: " + currentWeapon);
    }

    void UpdateWeaponVisual()
    {
        panObject.SetActive(currentWeapon == WeaponType.Pan);
        swordObject.SetActive(currentWeapon == WeaponType.Sword);
    }
}