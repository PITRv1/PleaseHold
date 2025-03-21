using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static GameParamSaver;

public class GameHandler : MonoBehaviour {

    [SerializeField] Flat flatPrefab;

    public event EventHandler NewMonthEvent;

    private CameraInput_Actions cameraInputActions;

    public event EventHandler<UI> changeTurnCountUI;

    public class UI : EventArgs {
        public int turnCount;

        public UI(int givenTurnCount) {
            turnCount = givenTurnCount;
        }
    }

    private int turnCount = 0;
    private int simStartYear;
    private int simStartMonth;
    private float populationStartHappiness;
    private float populationMinHappiness;
    private int initalBudget;
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

        simStartYear = Int32.Parse(SaveCSV.Instance.GetStartDate().Split('-')[0]);
        simStartMonth = Int32.Parse(SaveCSV.Instance.GetStartDate().Split('-')[1]);
        populationStartHappiness = float.Parse(SaveCSV.Instance.GetStartingPopulationHappiness());
        populationMinHappiness = float.Parse(SaveCSV.Instance.GetMinPopulationHappiness());
        initalBudget = (int) float.Parse(SaveCSV.Instance.GetInitialBudget());
        simLength = Int32.Parse(SaveCSV.Instance.GetSimulationLength());

        population = SaveCSV.Instance.GetCSVLength(SaveCSV.Instance.GetResidentFilePath()) - 1;

        budget = initalBudget;

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
        SaveToJson();
    }

    private void SaveToJson() {
        SaveObject saveObject = new SaveObject {
            buildingsPath = SaveCSV.Instance.GetBuildingFilePath(),
            residentsPath = SaveCSV.Instance.GetResidentFilePath(),
            servicesPath = SaveCSV.Instance.GetServiceFilePath(),
            budget = budget,
            populationHappiness = populationHappiness,
            minPopulationHappiness = populationMinHappiness,
            simulationLength = simLength,
            date = date,
        };
        string json = JsonUtility.ToJson(saveObject);

        File.WriteAllText(Application.dataPath + "/SaveFiles/NewGameParametersSaveFile.txt", json);
    }

    public class SaveObject {
        public string buildingsPath;
        public string residentsPath;
        public string servicesPath;

        public float budget;
        public float populationHappiness;
        public float minPopulationHappiness;
        public int simulationLength;
        public string date;
    }


    public void NewResident(string houseId) {
        string residentPath = SaveCSV.Instance.GetResidentFilePath();

        string name = firstNames[UnityEngine.Random.Range(0, firstNames.Length)] + " " + lastNames[UnityEngine.Random.Range(0, lastNames.Length)];
        string age = (Int32.Parse(date.Split('-')[0]) - UnityEngine.Random.Range(5, 80)).ToString();
        string job = "Im lazy";

        string newLine = $"{SaveCSV.Instance.GetCSVLength(residentPath).ToString()},{name},{age},{job},{houseId}";
        SaveCSV.Instance.WriteNewLineIntoCSV(residentPath, newLine);
    }

    public void CreateNewProject(string buildingNameText, string cost, string startingDate, string endDate, string buildingId) {
        string projectsCSVPath = SaveCSV.Instance.GetProjectsFilePath();

        string newLine = $"{SaveCSV.Instance.GetCSVLength(projectsCSVPath).ToString()},{buildingNameText},{cost},{startingDate},{endDate},{{{buildingId}}}";
        SaveCSV.Instance.WriteNewLineIntoCSV(projectsCSVPath, newLine);

        GameEventSystem.Instance.AddToOutput("Létrejött egy új projekt " + buildingNameText + " néven");

    }

    public void CreateNewBuilding(string name, string type, string date, string usefulArea, string turnsToBuild, string turns, string status, Plot plot) {

        List<Plot> plotList = PlotHandler.Instance.GetPlotList();

        string buildingsCSVPath = SaveCSV.Instance.GetBuildingFilePath();
        string id = SaveCSV.Instance.GetCSVLength(buildingsCSVPath).ToString();
        string newLine = $"{id},{name},{type},{date.Split('-')[0]},{usefulArea},{turnsToBuild},{turns},in construction,{plotList.IndexOf(plot)}";

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
                GameEventSystem.Instance.AddToOutput("Befejeződött egy szolgáltatás " + SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetServiceFilePath())[i].Split(',')[(int)SaveCSV.ServiceColumns.Name] + " néven");
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
        GameEventSystem.Instance.AddToOutput("Befejeződött egy szolgáltatás " + SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetServiceFilePath())[id].Split(',')[(int)SaveCSV.ServiceColumns.Name] + " néven");
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

    private void CreateFlat(string buildCSVLen, string buildingNameText, string buildingTypeText, string buildingYearText, string buildingAreaText, string turnsToBuild, string turns, string status, Plot plot) {

        string buildingId = buildCSVLen;
        Flat flat = Instantiate(flatPrefab, plot.transform);

        int id = Int32.Parse(buildingId);
        string name = buildingNameText;
        string type = buildingTypeText;
        int year = Int32.Parse(buildingYearText.Split('-')[0]);
        float area = float.Parse(buildingAreaText);

        flat.Initialize(id, name, type, year, area, Int32.Parse(turnsToBuild), Int32.Parse(turns), status, plot.transform);
        plot.isReserved = true;
        GameEventSystem.Instance.AddToOutput("Létrejött egy új épület " + buildingNameText + " néven");

    }

    public void CreateNewService(string name, string type, string buildingIds, string cost) {

        string servicePath = SaveCSV.Instance.GetServiceFilePath();
        string id = SaveCSV.Instance.GetCSVLength(servicePath).ToString();
        string newLine = $"{id},{name},{type},{buildingIds},{cost}";

        SaveCSV.Instance.WriteNewLineIntoCSV(servicePath, newLine);
        GameEventSystem.Instance.AddToOutput("Létrejött egy új szolgáltatás " + name + " néven");

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
