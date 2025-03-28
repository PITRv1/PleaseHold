using System;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour, IPointerClickHandler {

    [SerializeField] private GameObject selectedVisual;

    private Flat flatOn;


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

    public void SetPlot(Flat flat) {
        flatOn = flat;
    }

    public bool TryGetFlatOnPlot(out Flat flatOnPlot) {
        flatOnPlot = flatOn;
        if (isReserved && flatOn.GetBuildingStatus() != "in construction") {
            return true;
        } else {
            return false;
        }
    }

}
