using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadFile : MonoBehaviour {

    [SerializeField] Flat flatPrefab;
    public enum Columns {
        Id,
        Name,
        Type,
        Year,
        Size,
        TurnsToFinish,
        Turns,
        Status,
        Plot,
        Color,
    }

    private List<List<string>> fileList;
    public static LoadFile Instance {
        private set;
        get;
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {

        List<Plot> plotList = PlotHandler.Instance.GetPlotList();
        List<Plot> avaliablePlotList = PlotHandler.Instance.GetAvailablePlotList();


        fileList = SaveCSV.Instance.GetBuildingFileList();

        foreach (List<string> file in fileList) {

            if (file[(int)Columns.Plot] != "-1") {

                int index = Int32.Parse(file[(int)Columns.Plot]);

                string buildingId = file[(int)Columns.Id];
                Flat flat = Instantiate(flatPrefab, plotList[index].transform);
                Plot plotScript = plotList[index].GetComponent<Plot>();

                int id = Int32.Parse(file[(int)Columns.Id]);
                string name = file[(int)Columns.Name];
                string type = file[(int)Columns.Type];
                int year = Int32.Parse(file[(int)Columns.Year]);
                float area = float.Parse(file[(int)Columns.Size]);
                string status = file[(int)Columns.Status];

                int TurnsTillFinish = Int32.Parse(file[(int)Columns.TurnsToFinish]);
                int turns = Int32.Parse(file[(int)Columns.Turns]);
                string color = file[(int)Columns.Color];

                flat.Initialize(id, name, type, year, area, TurnsTillFinish, turns, status, plotList[index].transform, color);
                FlatHandler.Instance.AddToFlatList(flat);
                plotScript.SetPlot(flat);
                plotScript.isReserved = true;

            }

        }

        PlotHandler.Instance.ReloadLists();
        plotList = PlotHandler.Instance.GetPlotList();
        avaliablePlotList = PlotHandler.Instance.GetAvailablePlotList();


        foreach (List<string> file in fileList) {

            if (file[(int)Columns.Plot] == "-1") {

                int randomPlot = UnityEngine.Random.Range(0, avaliablePlotList.Count);

                string buildingId = file[(int)Columns.Id];
                Flat flat = Instantiate(flatPrefab, avaliablePlotList[randomPlot].transform);
                Plot plotScript = avaliablePlotList[randomPlot].GetComponent<Plot>();

                int id = Int32.Parse(file[(int)Columns.Id]);
                string name = file[(int)Columns.Name];
                string type = file[(int)Columns.Type];
                int year = Int32.Parse(file[(int)Columns.Year]);
                float area = float.Parse(file[(int)Columns.Size]);
                string status = file[(int)Columns.Status];

                int TurnsTillFinish = Int32.Parse(file[(int)Columns.TurnsToFinish]);
                int turns = Int32.Parse(file[(int)Columns.Turns]);
                string color = file[(int)Columns.Color];

                flat.Initialize(id, name, type, year, area, TurnsTillFinish, turns, status, avaliablePlotList[randomPlot].transform, color);
                plotScript.isReserved = true;

                FlatHandler.Instance.AddToFlatList(flat);
                plotScript.SetPlot(flat);

                SaveCSV.Instance.EditOneValueOnLine(Int32.Parse(buildingId), SaveCSV.BuildingColumns.Plot, SaveCSV.Instance.GetBuildingFilePath(), plotList.IndexOf(avaliablePlotList[randomPlot]).ToString());

                avaliablePlotList.RemoveAt(randomPlot);

            }
        }
        PlotHandler.Instance.ReloadLists();
    }
}
