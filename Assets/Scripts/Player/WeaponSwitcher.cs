using UnityEngine;
using System.Collections;

public enum WeaponType
{
    Pan,
    Sword
}

public class WeaponSwitcher : MonoBehaviour
{
    PlayerController playerController;
    private EnemyDetector enemyDetector;
    public GameObject panObject;
    public GameObject swordObject;

    public WeaponType currentWeapon = WeaponType.Pan;
    [SerializeField] private float switchCooldown = 0.5f;
    private float lastSwitchTime = -999f;


    void Start()
    {
        playerController = GetComponent<PlayerController>();
        enemyDetector = GetComponent<EnemyDetector>();
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
        StartCoroutine(SwitchWeaponVisualDelayed());
        if(currentWeapon == WeaponType.Pan)
        {
            enemyDetector.attackAngle = 45f;
            enemyDetector.closeAttackAngle = 65f;
            enemyDetector.closeAttackRange = 1.5f;
            enemyDetector.attackInterval = 0.8f;
        }
        else if(currentWeapon == WeaponType.Sword)
        {
            enemyDetector.attackAngle = 30f;
            enemyDetector.closeAttackAngle = 45f;
            enemyDetector.closeAttackRange = 1.0f;
            enemyDetector.attackInterval = 1f;
        }

        playerController.TriggerAnimation("SwitchWeapon");
        Debug.Log("현재 무기: " + currentWeapon);
    }

    private void UpdateWeaponVisual()
    {
        panObject.SetActive(currentWeapon == WeaponType.Pan);
        swordObject.SetActive(currentWeapon == WeaponType.Sword);
    }
    IEnumerator SwitchWeaponVisualDelayed()
    {
        yield return new WaitForSeconds(1f);
        UpdateWeaponVisual();
    }
}