using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Flat : Buildings, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] Canvas canvas;
    [SerializeField] Flat flat;
    [SerializeField] Image Timer;
    [SerializeField] TextMeshProUGUI IdText;
    [SerializeField] Image Integrity;
    [SerializeField] Transform HeadsUpDisplay;
    [SerializeField] Transform HospitalMesh;
    [SerializeField] Transform BuildingBlueMesh;
    [SerializeField] Transform BuildingBrownMesh;
    [SerializeField] Transform BuildingGreenMesh;
    [SerializeField] Transform BuildingPurpleMesh;
    [SerializeField] Transform BuildingYellowMesh;
    [SerializeField] Transform SchoolMesh;
    Transform buildingMesh;

    private Transform buildingPlot;

    public enum States {
        InConstruction,
        Built,
    }

    private int buildingid = 3;
    private string buildingName;
    private string buildingType;
    private int buildingYear;
    private float buildingArea;
    private string buildingStatus;
    private int buildingTurnsTillFinish;
    private int turns;

    private void Start() {
        GameHandler.Instance.NewMonthEvent += GameHandler_NewMonthEvent;
    }

    private void GameHandler_NewMonthEvent(object sender, System.EventArgs e) {
        List<string> lines = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetBuildingFilePath());
        buildingTurnsTillFinish = Int32.Parse(lines[buildingid].Split(',')[(int)SaveCSV.BuildingColumns.TurnsToFinish]);
        turns = Int32.Parse(lines[buildingid].Split(',')[(int)SaveCSV.BuildingColumns.Turns]);
        if (lines[buildingid].Split(',')[(int)SaveCSV.BuildingColumns.Status] == "in construction") {
            if (turns < buildingTurnsTillFinish) {
                turns += 1;

                if (turns >= buildingTurnsTillFinish) {
                    buildingStatus = "Perfect";
                    GameEventSystem.Instance.AddToOutput("Létrejött egy új épület " + buildingName + " néven");
                    SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.BuildingColumns.Status, SaveCSV.Instance.GetBuildingFilePath(), buildingStatus);
                }
                Timer.fillAmount = (float)turns / buildingTurnsTillFinish;
                Integrity.fillAmount = GetIntegrityFloat(buildingStatus);
                

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
        } else if(lines[buildingid].Split(',')[(int)SaveCSV.BuildingColumns.Status] != "Perfect") {
            if (turns < buildingTurnsTillFinish) {
                HeadsUpDisplay.gameObject.SetActive(true);
                turns += 1;

                if (turns >= buildingTurnsTillFinish) {
                    buildingStatus = "Perfect";
                    GameEventSystem.Instance.AddToOutput("Befejeződött egy karbantartás, a ház neve: " + buildingName);
                    SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.BuildingColumns.Status, SaveCSV.Instance.GetBuildingFilePath(), buildingStatus);
                }
                Timer.fillAmount = (float)turns / buildingTurnsTillFinish;
                Integrity.fillAmount = GetIntegrityFloat(buildingStatus);

                SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.BuildingColumns.TurnsToFinish, SaveCSV.Instance.GetBuildingFilePath(), (buildingTurnsTillFinish).ToString());
                SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.BuildingColumns.Turns, SaveCSV.Instance.GetBuildingFilePath(), (turns).ToString());
                if (buildingTurnsTillFinish - turns <= 0) {
                    HeadsUpDisplay.gameObject.SetActive(false);
                }
            }
        } 
        
    }

    public void Initialize(int id, string name, string type, int year, float area, int turnsTillFinish, int turns, string status, Transform plot, string color) {
        buildingid = id;
        buildingName = name;
        buildingType = type;
        buildingYear = year;
        buildingArea = area;
        buildingPlot = plot;
        buildingStatus = status;
        this.turns = turns;
        buildingTurnsTillFinish = turnsTillFinish;

        IdText.text = id.ToString();

        float heightOffset = 25f;
        switch (type) {
            default:
            case "lakóház":
                switch (color) {
                    case "blue":
                        buildingMesh = Instantiate(BuildingBlueMesh, transform);
                        break;
                    case "brown":
                        buildingMesh = Instantiate(BuildingBrownMesh, transform);
                        break;
                    case "green":
                        buildingMesh = Instantiate(BuildingGreenMesh, transform);
                        break;
                    case "purple":
                        buildingMesh = Instantiate(BuildingPurpleMesh, transform);
                        break;
                    case "yellow":
                        buildingMesh = Instantiate(BuildingYellowMesh, transform);
                        break;
                }
                break;
            case "kórház":
                buildingMesh = Instantiate(HospitalMesh, transform);
                heightOffset = 25f;
                canvas.transform.position += new Vector3(0, heightOffset, 0);
                break;
            case "iskola":
                heightOffset = -10f;
                canvas.transform.position += new Vector3(0, heightOffset, 0);
                buildingMesh = Instantiate(SchoolMesh, transform);
                break;
        }

        Integrity.fillAmount = GetIntegrityFloat(buildingStatus);

        if (status == "in construction") {

            Timer.fillAmount = (float)turns / buildingTurnsTillFinish;
            buildingMesh.gameObject.SetActive(false);
            HeadsUpDisplay.gameObject.SetActive(true);
        } else {
            if (buildingTurnsTillFinish - turns > 0) {
                Timer.fillAmount = (float)turns / buildingTurnsTillFinish;
                buildingMesh.gameObject.SetActive(true);
                HeadsUpDisplay.gameObject.SetActive(true);
            } else {
                buildingMesh.gameObject.SetActive(true);
                HeadsUpDisplay.gameObject.SetActive(false);
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData) {
        
        if (eventData.button == PointerEventData.InputButton.Right) {
            EventHandlerScript.Instance.SendOnFlatRightClick(this);
        } else if (eventData.button == PointerEventData.InputButton.Left)
        {
            CameraSystem.Instance.targetPosition = this.transform.position;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnPointerEnter(PointerEventData eventData) {
        EventHandlerScript.Instance.SendOnFlatEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        EventHandlerScript.Instance.SendOnFlatExit(this);
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

    private float GetIntegrityFloat(string integrity)
    {
        switch (integrity)
        {
            case "Perfect":
                return 1f;
            case "Good":
                return .8f;
            case "Average":
                return .6f;
            case "Bad":
                return .4f;
            case "Awful":
                return .2f;
            default:
            case "in construction":
                return 0f;
        }
    }
}
