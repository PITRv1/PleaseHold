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
        }

        List<string> buildings = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetBuildingFilePath());

        foreach (string building in buildings) {
            string[] line = building.Split(',');
        }


            foreach (Transform plot in transform) {
            plotList.Add(plot);
            Plot plotScript = plot.GetComponent<Plot>();
            if (plotScript.isReserved) continue;
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
