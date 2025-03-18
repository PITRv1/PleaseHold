using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Flat : Buildings, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] Flat flat;
    [SerializeField] Image Timer;
    [SerializeField] Transform HeadsUpDisplay;
    [SerializeField] Transform buildingMesh;

    private Transform buildingPlot;

    public enum States {
        InConstruction,
        Built,
    }


    private int buildingid;
    private string buildingName;
    private string buildingType;
    private int buildingYear;
    private float buildingArea;
    private string buildingStatus;
    private int buildingTurnsTillFinish;
    private int turns;

    private void Start() {
        GameHandler.Instance.NewMonthEvent += GameHandler_NewMonthEvent;
        turns = 0;
    }

    private void GameHandler_NewMonthEvent(object sender, System.EventArgs e) {
        List<string> lines = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetBuildingFilePath());
        buildingTurnsTillFinish = Int32.Parse(lines[buildingid].Split(',')[(int)SaveCSV.BuildingColumns.TurnsToFinish]);
        turns = Int32.Parse(lines[buildingid].Split(',')[(int)SaveCSV.BuildingColumns.Turns]);
        if (lines[buildingid].Split(',')[(int)SaveCSV.BuildingColumns.Status] == "in construction") {
            if (turns < buildingTurnsTillFinish) {
                turns += 1;

                if (turns >= buildingTurnsTillFinish) {
                    buildingStatus = "excelent";
                    SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.BuildingColumns.Status, SaveCSV.Instance.GetBuildingFilePath(), buildingStatus);
                }
                Timer.fillAmount = (float)turns / buildingTurnsTillFinish;
                SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.BuildingColumns.TurnsToFinish, SaveCSV.Instance.GetBuildingFilePath(), (buildingTurnsTillFinish).ToString());
                SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.BuildingColumns.Turns, SaveCSV.Instance.GetBuildingFilePath(), (turns).ToString());
                if (buildingTurnsTillFinish - turns > 0) {
                    buildingMesh.gameObject.SetActive(false);
                } else {
                    buildingMesh.gameObject.SetActive(true);
                    HeadsUpDisplay.gameObject.SetActive(false);
                    int newResidents = (int)buildingArea / 30;
                    for (int i = 0; i < newResidents; i++) {
                        GameHandler.Instance.NewResident(buildingid.ToString());
                    }
                }
            }
        } else if(lines[buildingid].Split(',')[(int)SaveCSV.BuildingColumns.Status] != "excelent") {
            if (turns < buildingTurnsTillFinish) {
                HeadsUpDisplay.gameObject.SetActive(true);
                turns += 1;

                if (turns >= buildingTurnsTillFinish) {
                    buildingStatus = "excelent";
                    SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.BuildingColumns.Status, SaveCSV.Instance.GetBuildingFilePath(), buildingStatus);
                }
                Timer.fillAmount = (float)turns / buildingTurnsTillFinish;
                SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.BuildingColumns.TurnsToFinish, SaveCSV.Instance.GetBuildingFilePath(), (buildingTurnsTillFinish).ToString());
                SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.BuildingColumns.Turns, SaveCSV.Instance.GetBuildingFilePath(), (turns).ToString());
                if (buildingTurnsTillFinish - turns <= 0) {
                    HeadsUpDisplay.gameObject.SetActive(false);
                }
            }
        } 
        
    }

    public void Initialize(int id, string name, string type, int year, float area, int turnsTillFinish, int turns, string status, Transform plot) {
        buildingid = id;
        buildingName = name;
        buildingType = type;
        buildingYear = year;
        buildingArea = area;
        buildingPlot = plot;
        buildingStatus = status;
        buildingTurnsTillFinish = turnsTillFinish;
        Timer.fillAmount = 0;
        if (buildingTurnsTillFinish > 0) {
            buildingMesh.gameObject.SetActive(false);
        } else {
            buildingMesh.gameObject.SetActive(true);
            HeadsUpDisplay.gameObject.SetActive(false);
        }
    }
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            EventHandlerScript.Instance.SendOnFlatRightClick(transform);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnPointerEnter(PointerEventData eventData) {
        EventHandlerScript.Instance.SendOnFlatEnter(transform);
    }

    public void OnPointerExit(PointerEventData eventData) {
        EventHandlerScript.Instance.SendOnFlatExit(transform);
    }

    public void OnPointerUp(PointerEventData eventData) {
    }

    public void SetPlot(Transform plot) {
        buildingPlot = plot;
    }

    public int GetBuildingId() { return buildingid; }
    public string GetBuildingName() { return buildingName; }
    public string GetBuildingType() { return buildingType; }
    public int GetBuildingYear() { return buildingYear; }
    public float GetBuildingArea() { return buildingArea; }
    public string GetBuildingStatus() { return buildingStatus; }
}
