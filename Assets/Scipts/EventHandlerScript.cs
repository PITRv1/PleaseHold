using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventHandlerScript : MonoBehaviour {

    public event EventHandler<BuildingEventArgs> OnPlotRightClick;
    public event EventHandler<BuildingEventArgs> OnFlatEnter;
    public event EventHandler<BuildingEventArgs> OnFlatExit;

    public class BuildingEventArgs : EventArgs {

        public Transform gameObject;
        public BuildingEventArgs(Transform givenGameObject) {
            gameObject = givenGameObject;
        }
    }

    public static EventHandlerScript Instance {
        private set;
        get;
    }
    private void Awake() {
        Instance = this;
    }

    public void SendOnPlotRightClick(Transform givenGameObject) {
        OnPlotRightClick?.Invoke(this, new BuildingEventArgs(givenGameObject));
    }

    public void SendOnFlatEnter(Transform givenGameObject) {
        OnFlatEnter?.Invoke(this, new BuildingEventArgs(givenGameObject));
    }

    public void SendOnFlatExit(Transform givenGameObject) {
        OnFlatExit?.Invoke(this, new BuildingEventArgs(givenGameObject));
    }

}
