using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Stage
{
    public List<RoomController> rooms;
}
public class RoomManager : MonoBehaviour
{
    public static  RoomManager Instance { get; private set;} 
    [SerializeField]private PlayerController playerController;

    public List<Stage> stages;
    public event Action<RoomController> OnRoomCleared;
    private int clearedStageCount = 0;
    private int clearedRoomCount = 0;
    private float gameStartTime;
    

    void Awake()
    {
        gameStartTime = Time.time;
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    void Start()
    {
        IngredientManager.Instance.ResetRunCollected();
        playerController.OnPlayerDied += TriggerGameOver;
         
        foreach(Stage stage in stages)
        {
            foreach(RoomController room in stage.rooms)
            {
                room.OnRoomCleared += HandleRoomCleared;
                room.OnDoorPassed += StageEnemyContainerActive;
            }
        }
    }
    void HandleRoomCleared(RoomController clearedRoom)
    {
        clearedRoomCount++;
        if(stages[clearedStageCount].rooms.Count <= clearedRoomCount)
        {
            clearedStageCount++;
            clearedRoomCount = 0;
        }

        if (stages.Count <= clearedStageCount)
        {
            Debug.Log("모든 스테이지 클리어!");
            //UnsubscribeAll(); 
            return; 
        }

        Debug.Log($"[RoomManager] {clearedRoom.name} 클리어 ({clearedRoomCount}/{stages[clearedStageCount].rooms.Count})");
    }
   
    
    public void StageEnemyContainerActive(RoomController clearedRoom)
    {
        for(int stageIndex = 0; stageIndex < stages.Count; stageIndex++)
        {
            int roomIndex = stages[stageIndex].rooms.IndexOf(clearedRoom);
            if (roomIndex == -1) continue;

            List<RoomController> stageRooms = stages[stageIndex].rooms;

            if(roomIndex + 1 < stageRooms.Count)
            {
                StartCoroutine(OpenRoomRoutine(stageIndex, roomIndex));
                return;
            }
        }
    }
    IEnumerator OpenRoomRoutine(int stageIndex, int RoomIndex)
    {
        yield return new WaitForSeconds(1f);
        stages[stageIndex].rooms[RoomIndex + 1].ActivateRoom();
        stages[stageIndex].rooms[RoomIndex].CloseDoor();
    }
    public void ActivateNextStage(int currentStageIndex)
    {
        int nextStageIndex = currentStageIndex + 1;
        if (nextStageIndex >= stages.Count)
        {
            Debug.Log("모든 스테이지 클리어! 던전 종료");
            return;
        }

        stages[nextStageIndex].rooms[0].ActivateRoom();
    }
    
    void UnsubscribeAll()
    {
        playerController.OnPlayerDied -= TriggerGameOver;
        foreach(Stage stage in stages)
        {
            foreach(RoomController room in stage.rooms)
            {
                if(room != null)
                {
                    room.OnRoomCleared -= HandleRoomCleared;
                    room.OnDoorPassed -= StageEnemyContainerActive;
                }
            }
        }
    }
    public void ShowResultPannel()
    {
        Time.timeScale = 0f;
        float totalTime = Time.time - gameStartTime;
        DungeonResultUI.Instance.ShowResultPanel(totalTime,true);
    }
    public void TriggerGameOver()
    {
        Time.timeScale = 0f;
        float totalTime = Time.time - gameStartTime;
        DungeonResultUI.Instance.ShowResultPanel(totalTime,false);
    }

    void OnDestroy()
    {
        UnsubscribeAll();
    }
}
