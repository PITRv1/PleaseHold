using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour, IPointerClickHandler {


    Transform Flat;

    public bool isReserved = false;

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right && !isReserved) {
            EventHandlerScript.Instance.SendOnPlotRightClick(this);
        }
    }
}
