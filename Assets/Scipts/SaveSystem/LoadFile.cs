using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadFile : MonoBehaviour {

    [SerializeField] Transform flatPrefab;
    public enum Columns {
        Id,
        Name,
        Type,
        Year,
        Size,
        TurnsToFinish,
        Status,
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

        List<Transform> plotList = PlotHandler.Instance.GetPlotList();
        List<Transform> avaliablePlotList = PlotHandler.Instance.GetAvailablePlotList();


        fileList = SaveCSV.Instance.GetBuildingFileList();

        foreach (List<string> file in fileList) {
            switch (file[(int)Columns.Type]){
                case "lakóház":

                    Transform flat = Instantiate(flatPrefab, avaliablePlotList[0]);

                    Flat flatScript = flat.GetComponent<Flat>();
                    Plot plotScript = avaliablePlotList[0].GetComponent<Plot>();


                    int id = Int32.Parse(file[(int)Columns.Id]);
                    string name = file[(int)Columns.Name];
                    string type = file[(int)Columns.Type];
                    int year = Int32.Parse(file[(int)Columns.Year]);
                    float area = float.Parse(file[(int)Columns.Size]);
                    string status = file[(int)Columns.Status];
                    string turnToBuild = "0";

                    flatScript.Initialize(id, name, type, year, area, Int32.Parse(turnToBuild), status, avaliablePlotList[0]);
                    plotScript.isReserved = true;
                    avaliablePlotList.RemoveAt(0);

                    break;
            }
        }
        PlotHandler.Instance.ReloadLists();
    }

}
