using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform nextStageSpawnPoint;
    [SerializeField] private int currentStageIndex;
    [SerializeField] private bool isDongeonClear = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("포탈에 들어왔습니다.");
        if (isDongeonClear)
        {
            RoomManager.Instance.ShowResultPannel();
        }
        else
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.TeleportToNextStage(nextStageSpawnPoint.position);
            RoomManager.Instance.ActivateNextStage(currentStageIndex);
        }
    }
}
