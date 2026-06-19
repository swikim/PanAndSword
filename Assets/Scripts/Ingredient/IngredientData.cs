using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PanAndSword/IngredientData")]

public class IngredientData : ScriptableObject
{
   public string ingredientName;
   public Sprite sprite;

   [Range(0f,1f)] public float dropRate = 0.5f;
}
