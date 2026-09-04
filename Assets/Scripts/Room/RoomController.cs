using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public enum RoomType
{
    Normal,
    MiniBoss,
    Boss,
}
public class RoomController : MonoBehaviour
{
    public RoomType roomType;
    public GameObject doorObject;
    public GameObject leftDoorObject;
    public GameObject rightDoorObject;
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
        if (roomType == RoomType.Boss || roomType == RoomType.MiniBoss)
        {
            SoundManager.Instance.PlayBossBGM();
        }
    }

    public void CloseDoor()
    {
        doorObject.SetActive(true);
        rightDoorObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        leftDoorObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
   
    void ClearRoom()
    {
        isCleared = true;
        if(roomType == RoomType.Boss)
        {
            portalPrefab.SetActive(true);
            SoundManager.Instance.PlayGameSceneBGM(); // 추후 승리 BGM 추가 예정
        }
        else if(roomType == RoomType.MiniBoss)
        {
            portalPrefab.SetActive(true);
            SoundManager.Instance.PlayGameSceneBGM(); // 미니보스는 다시 일반 BGM
        }
        else
        {
            doorObject.SetActive(false);
            Debug.Log(gameObject.name + " 클리어! 문이 열렸습니다.");
            rightDoorObject.transform.rotation = Quaternion.Euler(0, 90, 0);
            leftDoorObject.transform.rotation = Quaternion.Euler(0, -90, 0);
        }

        OnRoomCleared?.Invoke(this);
    }
}
