using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class RoomController : MonoBehaviour
{
    public GameObject doorObject;
    public GameObject doorTrigger;
    public Transform enemyContainer;
    private bool isCleared = false;

    public event Action<RoomController> OnRoomCleared;
    public event Action<RoomController> OnDoorPassed;

    void Start()
    {
        
    }
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
        doorObject.SetActive(false);
        Debug.Log(gameObject.name + " 클리어! 문이 열렸습니다.");

        OnRoomCleared?.Invoke(this);
    }
}
