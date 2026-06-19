using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientItem : MonoBehaviour
{
    public IngredientData ingredientData;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
            
        Debug.Log($"[Item] {ingredientData.ingredientName} 수집");

        IngredientPool.Instance.Return(gameObject);
    }
}
