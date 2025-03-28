using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlotHandler : MonoBehaviour {

    private List<Plot> plotList = new List<Plot>();
    private List<Plot> availablePlotList = new List<Plot>();
    private List<Plot> reversedPlotList = new List<Plot>();
    public static PlotHandler Instance {
        private set;
        get;
    }
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        ReloadLists();
    }

    public void ReloadLists() {
        plotList = new List<Plot>();
        availablePlotList = new List<Plot>();
        reversedPlotList = new List<Plot>();

    Plot[] plotArray = transform.GetComponentsInChildren<Plot>();

        foreach (Plot plot in plotArray) {
            plotList.Add(plot);
        }

        List<string> buildings = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetBuildingFilePath());
        buildings.RemoveAt(0);

        foreach (string building in buildings) {
            string[] line = building.Split(',');
            if (Int32.Parse(line[(int)SaveCSV.BuildingColumns.Plot]) >= 0) {
                plotList[Int32.Parse(line[(int)SaveCSV.BuildingColumns.Plot])].isReserved = true;
            }
        }

        foreach (Plot plot in plotList) {
            if (plot.isReserved == true) {
                reversedPlotList.Add(plot);
                continue;
            }
            availablePlotList.Add(plot);
        }
    }

    public List<Plot> GetPlotList() {
        ReloadLists();
        return plotList;
    }
    public List<Plot> GetAvailablePlotList() {
        ReloadLists();
        return availablePlotList;
    }

    public List<Plot> GetRevervedPlotList() {
        ReloadLists();
        return reversedPlotList;
    }

}
