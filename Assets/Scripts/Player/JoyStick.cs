using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 centerPosition;
    public Vector2 direction;
    [SerializeField] private RectTransform background;
    [SerializeField]private RectTransform joystickHandle;
    public bool isDragging = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        centerPosition = eventData.position;
        isDragging = true;
        ApearJoyStick();
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        background.gameObject.SetActive(isDragging);
        direction = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 currentPosition = eventData.position;
        Vector2 offset = currentPosition - centerPosition;
        
        Vector2 clamped = Vector2.ClampMagnitude(offset, 200f);
        direction = clamped.normalized;
        joystickHandle.anchoredPosition = clamped;

    }
    
    void ApearJoyStick()
    {
        background.gameObject.SetActive(isDragging);
        background.transform.position = centerPosition;
    }
}
