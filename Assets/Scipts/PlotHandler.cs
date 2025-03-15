using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlotHandler : MonoBehaviour {

    private List<Transform> plotList = new List<Transform>();
    private List<Transform> availablePlotList = new List<Transform>();
    public static PlotHandler Instance {
        private set;
        get;
    }
    private void Awake() {
        Instance = this;
        ReloadLists();
    }

    public void ReloadLists() {
        plotList = new List<Transform>();
        availablePlotList = new List<Transform>();
        foreach (Transform plot in transform) {
            plotList.Add(plot);
            Plot flatScript = plot.GetComponent<Plot>();
            if (flatScript.isReserved) continue;
            availablePlotList.Add(plot);
        }
    }

    public List<Transform> GetPlotList() {
        return plotList;
    }
    public List<Transform> GetAvailablePlotList() {
        return plotList;
    }

}
