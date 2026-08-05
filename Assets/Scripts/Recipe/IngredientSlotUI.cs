using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlotUI : MonoBehaviour
{
    public GameObject slotRoot;
    public TMP_Text countText;
    public Image icon;
    public void SetIngredientData(IngredientData data, int count)
    {
        countText.text = count.ToString();
        icon.sprite = data.sprite;
   }
}