using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    
    public static RecipeManager Instance { get; private set;}   
    [SerializeField] private List<RecipeData> recipeDatas;


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
       RoomManager.Instance.OnRoomCleared += OnRoomCleared;
    }
    void OnRoomCleared(RoomController clearedRoom)
    {
       
    }
    void OnDestroy()
    {
        RoomManager.Instance.OnRoomCleared -= OnRoomCleared;
    }
}
