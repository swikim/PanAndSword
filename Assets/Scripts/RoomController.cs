using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomController : MonoBehaviour
{
    public GameObject doorObject;
    public Transform enemyContainer;
    private bool isCleared = false;

    public event Action<RoomController> OnRoomCleared;
    void Update()
    {
        if(isCleared) return;
        if(enemyContainer.childCount == 0)
        {
            ClearRoom();
        }
    }
    void ClearRoom()
    {
        isCleared = true;
        doorObject.SetActive(false);
        Debug.Log(gameObject.name + " 클리어! 문이 열렸습니다.");

        OnRoomCleared?.Invoke(this);
    }
}
