using UnityEngine;
using UnityEngine.EventSystems;

public class EventClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            Debug.Log("Left Click");
        } else if (eventData.button == PointerEventData.InputButton.Right) {
            Debug.Log("Right Click");
        } else {
            Debug.Log("Middle Click");
        }
        
    }

    public void OnPointerDown(PointerEventData eventData) {
        // Empty
    }

    public void OnPointerEnter(PointerEventData eventData) {
        // Empty
    }

    public void OnPointerExit(PointerEventData eventData) {
        // Empty
    }

    public void OnPointerUp(PointerEventData eventData) {
        // Empty
    }
}
