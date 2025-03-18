using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameHandler : MonoBehaviour {

    [SerializeField] Transform flatPrefab;

    public event EventHandler NewMonthEvent;

    private CameraInput_Actions cameraInputActions;

    public event EventHandler<UI> changeTurnCountUI;

    public class UI : EventArgs {
        public int turnCount;

        public UI(int givenTurnCount) {
            turnCount = givenTurnCount;
        }
    }

    private int turnCount = 12;
    private int simStartYear = 2025;
    private int simStartMonth = 3;
    private int simStartLength;
    private float populationStartHappiness;
    private int initalBudget;

    private int simLength;
    private float populationHappiness;
    private int budget;

    private string date;


    public static GameHandler Instance {
        private set;
        get;
    }

    private void Awake() {
        Instance = this;
        cameraInputActions = new CameraInput_Actions();
        cameraInputActions.Camera.Enable();
        cameraInputActions.Camera.addToTurn.performed += AddToTurn_performed;
    }

    public void NewMonth() {
        turnCount += 1;
        UpdateDate();
    }

    public void CreateNewBuildingProject(string buildingNameText, string cost, string startingDate, string endDate, string buildingId) {
        string projectsCSVPath = SaveCSV.Instance.GetProjectsFilePath();

        string newLine = $"{SaveCSV.Instance.GetCSVLength(projectsCSVPath).ToString()},{buildingNameText},{cost},{startingDate},{endDate},{{{buildingId}}}";
        SaveCSV.Instance.WriteNewLineIntoCSV(projectsCSVPath, newLine);
    }

    public void CreateNewBuilding(string name, string type, string date, string usefulArea, string turnsToBuild, string status, Transform plot) {

        string buildingsCSVPath = SaveCSV.Instance.GetBuildingFilePath();
        string id = SaveCSV.Instance.GetCSVLength(buildingsCSVPath).ToString();
        string newLine = $"{id},{name},{type},{date},{usefulArea},{turnsToBuild + turnCount},in construction";

        SaveCSV.Instance.WriteNewLineIntoCSV(buildingsCSVPath, newLine);
        CreateFlat(id, name, type, date, usefulArea, turnsToBuild + turnCount, status, plot);
    }

    private void AddToTurn_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        turnCount += 1;
        changeTurnCountUI?.Invoke(this, new UI(turnCount));
    }

    private void UpdateDate() {
        int year = simStartYear;
        int month = simStartMonth;

        month += turnCount;
        int years = (month - 1) / 12;  // Subtract 1 to handle the 0th month issue
        int leftoverMonths = (month - 1) % 12 + 1; // Ensure months are in range 1-12
        year += years;
        month = leftoverMonths;

        date = year.ToString() + '-' + month.ToString();

        UpperBarContainer.Instance.ChangeDate(date);
    }

    private void CreateFlat(string buildCSVLen, string buildingNameText, string buildingTypeText, string buildingYearText, string buildingAreaText, string turnsToBuild, string status, Transform plot) {

        string buildingId = buildCSVLen;
        Transform flat = Instantiate(flatPrefab, plot.transform);

        Flat flatScript = flat.GetComponent<Flat>();
        Plot plotScript = plot.GetComponent<Plot>();

        int id = Int32.Parse(buildingId);
        string name = buildingNameText;
        string type = buildingTypeText;
        int year = Int32.Parse(buildingYearText.Split('-')[0]);
        float area = float.Parse(buildingAreaText);

        flatScript.Initialize(id, name, type, year, area, Int32.Parse(turnsToBuild), status, plot);
        plotScript.isReserved = true;
    }

    public string GetDate() {
        UpdateDate();
        return date;
    }
    
    public int GetTurnCount() {
        return turnCount;
    }
}
