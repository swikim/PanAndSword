using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoomManager : MonoBehaviour
{
    public static  RoomManager Instance { get; private set;} 

    public List<RoomController> rooms;
    public event Action<RoomController> OnRoomCleared;
    private int clearedCount = 0;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    void Start()
    {
        foreach(RoomController room in rooms)
        {
            room.OnRoomCleared += HandleRoomCleared;
        }
    }
    void HandleRoomCleared(RoomController clearedRoom)
    {
        clearedCount++;
        Debug.Log($"[RoomManager] {clearedRoom.name} 클리어 ({clearedCount}/{rooms.Count})");

        if(clearedCount >= rooms.Count)
        {
            Debug.Log("All rooms cleared");
        }
    }

    void OnDestroy()
    {
        if(rooms == null) return;

        foreach(RoomController room in rooms)
        {
            if(room != null) room.OnRoomCleared -= HandleRoomCleared;
        }
    }
}
