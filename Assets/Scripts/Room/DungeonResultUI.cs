using TMPro;
using UnityEngine;

public class DungeonResultUI : MonoBehaviour
{
    public static DungeonResultUI Instance { get; private set;}
    [SerializeField]private GameObject resultPanel;
    [SerializeField]private GameObject slotPrefab;
    [SerializeField]private Transform slotContainer;
    [SerializeField]private TextMeshProUGUI resultText;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowResultPanel(float totalPlayTime)
    {
        resultPanel.SetActive(true);
        int minutes = Mathf.FloorToInt(totalPlayTime / 60f);
        int seconds = Mathf.FloorToInt(totalPlayTime % 60f);
        string formattedTime = $"{minutes:D2}:{seconds:D2}";
        Debug.Log($"총 플레이 시간: {formattedTime}");
        resultText.text = formattedTime;

        foreach(Transform item in slotContainer)
        {
            Destroy(item.gameObject);
        }
        var ingredients = IngredientManager.Instance.currentRunCollected;
        foreach(var data in ingredients)
        {
            GameObject slot = Instantiate(slotPrefab, slotContainer);
            var slotUI = slot.GetComponent<IngredientSlotUI>();
            slotUI.SetIngredientData(data.Key, data.Value);
        }
    }
}
