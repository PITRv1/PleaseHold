using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
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
    private int initalBudget = 100000;
    private int population;

    private int simLength;
    private float populationHappiness;
    private float budget;

    private string date;

    private string[] firstNames = { "Varga", "Molnár", "Kovács", "Takács", "Tóth", "Szabó", "Nagy" };
    private string[] lastNames = { "Varga", "Molnár", "Kovács", "Takács", "Tóth", "Szabó", "Nagy" };
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

    private void Start() {
        budget = initalBudget;
        population = SaveCSV.Instance.GetCSVLength(SaveCSV.Instance.GetResidentFilePath());
        UpdateDate();
        UpdateHUD();
    }

    public void NewMonth() {
        turnCount += 1;
        UpdateDate();
        HandleProjects();
        HandleServices();
        NewMonthEvent?.Invoke(this, EventArgs.Empty);
        population = SaveCSV.Instance.GetCSVLength(SaveCSV.Instance.GetResidentFilePath());
        UpdateHUD();
    }

    public void NewResident(string houseId) {
        string residentPath = SaveCSV.Instance.GetResidentFilePath();

        string name = firstNames[UnityEngine.Random.Range(0, firstNames.Length)] + " " + lastNames[UnityEngine.Random.Range(0, lastNames.Length)];
        string age = (Int32.Parse(date.Split('-')[0]) - UnityEngine.Random.Range(5, 80)).ToString();
        string job = "Im lazy";

        string newLine = $"{SaveCSV.Instance.GetCSVLength(residentPath).ToString()},{name},{age},{job},{houseId}";
        SaveCSV.Instance.WriteNewLineIntoCSV(residentPath, newLine);
    }

    public void CreateNewBuildingProject(string buildingNameText, string cost, string startingDate, string endDate, string buildingId) {
        string projectsCSVPath = SaveCSV.Instance.GetProjectsFilePath();

        string newLine = $"{SaveCSV.Instance.GetCSVLength(projectsCSVPath).ToString()},{buildingNameText},{cost},{startingDate},{endDate},{{{buildingId}}}";
        SaveCSV.Instance.WriteNewLineIntoCSV(projectsCSVPath, newLine);
    }

    public void CreateNewBuilding(string name, string type, string date, string usefulArea, string turnsToBuild, string turns, string status, Transform plot) {

        string buildingsCSVPath = SaveCSV.Instance.GetBuildingFilePath();
        string id = SaveCSV.Instance.GetCSVLength(buildingsCSVPath).ToString();
        string newLine = $"{id},{name},{type},{date},{usefulArea},{turnsToBuild},{turns},in construction";

        SaveCSV.Instance.WriteNewLineIntoCSV(buildingsCSVPath, newLine);
        CreateFlat(id, name, type, date, usefulArea, turnsToBuild, turns, status, plot);
    }

    private void AddToTurn_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        turnCount += 1;
        changeTurnCountUI?.Invoke(this, new UI(turnCount));
    }

    private void HandleProjects() {

        string projectPath = SaveCSV.Instance.GetProjectsFilePath();
        List<string> projectCSV = SaveCSV.Instance.ReadLinesFromCSV(projectPath);
        for (int i = 0; i < projectCSV.Count(); i++) {

            if (i == 0) continue;

            List<string> splitLine = projectCSV[i].Split(',').ToList();

            if (splitLine[(int)SaveCSV.ProjectColumns.EndDate] == date) {
                SaveCSV.Instance.DeleteFromCSV(projectPath, i);
            } else {
                budget -= float.Parse(splitLine[(int)SaveCSV.ProjectColumns.Cost]);
            }
        }

        SaveCSV.Instance.UpdateIds(projectPath);
    }
    private void HandleServices() {

        string servicePath = SaveCSV.Instance.GetServiceFilePath();
        List<string> serviceCSV = SaveCSV.Instance.ReadLinesFromCSV(servicePath);
        for (int i = 0; i < serviceCSV.Count(); i++) {

            if (i == 0) continue;

            List<string> splitLine = serviceCSV[i].Split(',').ToList();

            budget -= float.Parse(splitLine[(int)SaveCSV.ServiceColumns.Cost]);
        }
    }

    private void DeleteService(string filePath, int id) {
        SaveCSV.Instance.DeleteFromCSV(filePath, id);
        SaveCSV.Instance.UpdateIds(filePath);
    }

    private void UpdateDate() {
        int year = simStartYear;
        int month = simStartMonth;

        month += turnCount;
        int years = (month - 1) / 12;  // Subtract 1 to handle the 0th month issue
        int leftoverMonths = (month - 1) % 12 + 1; // Ensure months are in range 1-12
        year += years;
        month = leftoverMonths;

        if (month < 10){
            date = year.ToString() + "-0" + month.ToString();
        } else {
            date = year.ToString() + '-' + month.ToString();
        }
    }

    private void UpdateHUD() {
        UpperBarContainer.Instance.ChangeDate(date);
        UpperBarContainer.Instance.ChangeBudget(budget.ToString());
    }

    private void CreateFlat(string buildCSVLen, string buildingNameText, string buildingTypeText, string buildingYearText, string buildingAreaText, string turnsToBuild, string turns, string status, Transform plot) {

        string buildingId = buildCSVLen;
        Transform flat = Instantiate(flatPrefab, plot.transform);

        Flat flatScript = flat.GetComponent<Flat>();
        Plot plotScript = plot.GetComponent<Plot>();

        int id = Int32.Parse(buildingId);
        string name = buildingNameText;
        string type = buildingTypeText;
        int year = Int32.Parse(buildingYearText.Split('-')[0]);
        float area = float.Parse(buildingAreaText);

        flatScript.Initialize(id, name, type, year, area, Int32.Parse(turnsToBuild), Int32.Parse(turns), status, plot);
        plotScript.isReserved = true;
    }

    public void CreateNewService(string name, string type, string buildingIds, string cost) {

        string servicePath = SaveCSV.Instance.GetServiceFilePath();
        string id = SaveCSV.Instance.GetCSVLength(servicePath).ToString();
        string newLine = $"{id},{name},{type},{buildingIds},{cost}";

        SaveCSV.Instance.WriteNewLineIntoCSV(servicePath, newLine);
    }

    public string GetDate() {
        UpdateDate();
        return date;
    }
    
    public int GetTurnCount() {
        return turnCount;
    }

    public float GetBudget() {
        return budget;
    }
}
