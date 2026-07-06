using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string saveFilePath;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
        LoadGameData();
    }

    public void SaveGameData()
    {
        SaveData saveData = new SaveData();
        saveData.playerStatus = GameData.playerStatus;
        saveData.ingredientData = GameData.ingredientData; 
        saveData.cookData = GameData.cookData;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Game data saved to {saveFilePath}");
    }
    public void LoadGameData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            GameData.playerStatus = saveData.playerStatus;
            GameData.ingredientData = saveData.ingredientData;
            GameData.cookData = saveData.cookData;

            Debug.Log($"Game data loaded from {saveFilePath}");
        }
        else
        {
            Debug.LogWarning($"Save file not found at {saveFilePath}");
        }
    }
}
