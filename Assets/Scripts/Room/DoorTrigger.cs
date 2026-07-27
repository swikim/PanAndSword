using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private RoomController roomController;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        roomController.NotifyDoorPassed();
    }
}
