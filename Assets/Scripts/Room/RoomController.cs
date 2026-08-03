using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public enum RoomType
{
    Normal,
    Boss,
}
public class RoomController : MonoBehaviour
{
    public RoomType roomType;
    public GameObject doorObject;
    public GameObject doorTrigger;
    public Transform enemyContainer;
    public GameObject portalPrefab;
    private bool isCleared = false;

    public event Action<RoomController> OnRoomCleared;
    public event Action<RoomController> OnDoorPassed;

    void Update()
    {
        if(isCleared) return;
        if(enemyContainer.childCount == 0)
        {
            ClearRoom();
        }
    }
    public void NotifyDoorPassed()
    {
        OnDoorPassed?.Invoke(this);
    }
    public void ActivateRoom()
    {
        enemyContainer.gameObject.SetActive(true);
    }

    public void CloseDoor()
    {
        doorObject.SetActive(true);
    }
   
    void ClearRoom()
    {
        isCleared = true;
        if(roomType == RoomType.Boss)
        {
            portalPrefab.SetActive(true);
        }
        doorObject.SetActive(false);
        Debug.Log(gameObject.name + " 클리어! 문이 열렸습니다.");

        OnRoomCleared?.Invoke(this);
    }
}
