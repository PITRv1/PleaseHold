using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour, IPointerClickHandler {

    Transform Flat;
    [SerializeField] private GameObject selectedVisual;


    private bool _isReserved = false; // Default is false

    public bool isReserved
    {
        get { return _isReserved; }
        set
        {
            if (!_isReserved && value) // Only trigger when changing from false to true
            {
                _isReserved = true;
                gotReserved();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right && !isReserved) {
            EventHandlerScript.Instance.SendOnPlotRightClick(this);
        }
    }

    private void gotReserved()
    {
        selectedVisual.SetActive(false);
    }

}
