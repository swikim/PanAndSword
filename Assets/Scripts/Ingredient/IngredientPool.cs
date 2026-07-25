using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;


[System.Serializable] 
public class PoolEntry
    {
        public IngredientData ingredientData;
        public GameObject prefab;
        public int initialSize = 5;
    }

public class IngredientPool : MonoBehaviour
{
    public static IngredientPool Instance { get; private set; }

    [SerializeField] private List<PoolEntry> entries;
    private Dictionary<IngredientData, Queue<GameObject>> _pools;
    
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
        _pools = new Dictionary<IngredientData, Queue<GameObject>>();

        foreach(var entry in entries)
        {
            var queue = new Queue<GameObject>();
            for(int i = 0; i < entry.initialSize; i++)
            {
                var obj = Instantiate(entry.prefab, transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            _pools[entry.ingredientData] = queue;
        }
    }

    public GameObject Get(IngredientData data , Vector3 position)
    {
        if(!_pools.TryGetValue(data, out var queue))
        {
            Debug.LogWarning($"[Pool] {data.ingredientName} 풀 없음");
            return null;
        }

        GameObject obj  = queue.Count > 0
            ? queue.Dequeue()
            : Instantiate(GetPrefab(data), transform);

        obj.transform.position = position;
        obj.SetActive(true);
        
        Debug.Log("GET ITEM");
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        var item = obj.GetComponent<IngredientItem>();
        if(item != null && _pools.TryGetValue(item.ingredientData, out var queue))
        {
            queue.Enqueue(obj);
        }
    }

    private GameObject GetPrefab(IngredientData data)
    {
        foreach(var e in entries)
            if(e.ingredientData == data) return e.prefab;
        
        return null;
    }
}
