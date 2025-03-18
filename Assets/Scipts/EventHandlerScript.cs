using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventHandlerScript : MonoBehaviour {

    public event EventHandler<BuildingEventArgsPlot> OnPlotRightClick;
    public event EventHandler<BuildingEventArgsFlat> OnFlatEnter;
    public event EventHandler<BuildingEventArgsFlat> OnFlatExit;

    public event EventHandler<BuildingEventArgsFlat> OnFlatRightClick;

    public class BuildingEventArgsPlot : EventArgs {

        public Plot plot;
        public BuildingEventArgsPlot(Plot plot) {
            this.plot = plot;
        }
    }

    public class BuildingEventArgsFlat : EventArgs {

        public Flat flat;
        public BuildingEventArgsFlat(Flat flat) {
            this.flat = flat;
        }
    }

    public static EventHandlerScript Instance {
        private set;
        get;
    }
    private void Awake() {
        Instance = this;
    }

    public void SendOnPlotRightClick(Plot plot) {
        OnPlotRightClick?.Invoke(this, new BuildingEventArgsPlot(plot));
    }

    public void SendOnFlatRightClick(Flat flat) {
        OnFlatRightClick?.Invoke(this, new BuildingEventArgsFlat(flat));
    }

    public void SendOnFlatEnter(Flat flat) {
        OnFlatEnter?.Invoke(this, new BuildingEventArgsFlat(flat));
    }

    public void SendOnFlatExit(Flat flat) {
        OnFlatExit?.Invoke(this, new BuildingEventArgsFlat(flat));
    }

}
