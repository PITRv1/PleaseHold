using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField] float repairHappines;
    [SerializeField] float newBuildingHappines;
    [SerializeField] float newServiceHappines;
    [SerializeField] float endServiceHappines;
    [SerializeField] EventDisplay eventDisplay;
    [SerializeField] EndGameUI endGameUI; 

    private int turnCount = 0;
    private int simStartYear;
    private int simStartMonth;
    private float populationStartHappiness;
    private float populationMinHappiness;
    private float populationMaxHappiness = 99.0f;
    private int initalBudget;
    private int population;

    private int simLength;
    private float populationHappiness;
    private float budget;

    private string endDate;
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
        populationMinHappiness = Mathf.Clamp(float.Parse(SaveCSV.Instance.GetMinPopulationHappiness()), 1, 99.99f);
        initalBudget = (int) float.Parse(SaveCSV.Instance.GetInitialBudget());
        simLength = Int32.Parse(SaveCSV.Instance.GetSimulationLength());

        populationHappiness = populationStartHappiness;

        population = SaveCSV.Instance.GetCSVLength(SaveCSV.Instance.GetResidentFilePath()) - 1;

        budget = initalBudget;

        endDate = GetEndDate(simStartYear, simStartMonth, simLength);

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
        RandomEvent();
        populationHappiness = Mathf.Clamp(populationHappiness, populationMinHappiness, populationMaxHappiness); // Just make sure it works bruh
        UpdateHUD();
        SaveToJson();
        CheckGameState();
    }

    private void RandomEvent() {
        float chance = UnityEngine.Random.value;

        if (chance <= 0.2f) {
            eventDisplay.SetEventName("City wide famine");
            eventDisplay.SetEventText("A food shortage has hit the city. Lose 15% population happiness.");
            eventDisplay.SetEventColor(Color.red);

            GameEventSystem.Instance.AddToOutput("The city was hit by a famine and lost 15% population happiness.");


            populationHappiness -= 15f;

        } else if (chance <= 0.4f){
            eventDisplay.SetEventName("Extra Government funding");
            eventDisplay.SetEventText("The city by miracle has won an application for extra government funding that it applied for. Gain 100000$ extra budget.");
            eventDisplay.SetEventColor(Color.green);

            GameEventSystem.Instance.AddToOutput("The city has recived 100000$ extra funding.");

            budget += 100000f;

        } else if (chance <= 0.6f) {
            eventDisplay.SetEventName("Water pipe disaster");
            eventDisplay.SetEventText("A major water pipe in the city has raptured. You use 10000$ to fix it but people are 5% less happy.");
            eventDisplay.SetEventColor(Color.red);

            GameEventSystem.Instance.AddToOutput("A major pipe in the city ruptured and caused 10000$ in damages. The people are 5% less happy.");

            budget -= 10000f;
            populationHappiness -= 5f;

        } else if (chance <= 0.8f) {
            eventDisplay.SetEventName("PlaceHolder");
            eventDisplay.SetEventText("Make more events if you see this . . .");
            eventDisplay.SetEventColor(Color.blue);

            GameEventSystem.Instance.AddToOutput("Make more events if you see this . . .");

        }
        else
        {
            eventDisplay.SetEventName("PlaceHolder");
            eventDisplay.SetEventText("Make more events if you see this . . .");
            eventDisplay.SetEventColor(Color.blue);

            GameEventSystem.Instance.AddToOutput("Make more events if you see this . . .");
        }

        eventDisplay.ShowUI();
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

    public void CreateNewProject(string buildingNameText, string cost, string startingDate, string endDate, string buildingId, string type) {
        string projectsCSVPath = SaveCSV.Instance.GetProjectsFilePath();

        string newLine = $"{SaveCSV.Instance.GetCSVLength(projectsCSVPath).ToString()},{buildingNameText},{cost},{startingDate},{endDate},{{{buildingId}}},{type}";
        SaveCSV.Instance.WriteNewLineIntoCSV(projectsCSVPath, newLine);

        GameEventSystem.Instance.AddToOutput("Létrejött egy új projekt " + buildingNameText + " néven");

    }

    public void CreateNewBuilding(string name, string type, string date, string usefulArea, string turnsToBuild, string turns, string status, Plot plot, string color) {

        List<Plot> plotList = PlotHandler.Instance.GetPlotList();

        string buildingsCSVPath = SaveCSV.Instance.GetBuildingFilePath();
        string id = SaveCSV.Instance.GetCSVLength(buildingsCSVPath).ToString();
        string newLine = $"{id},{name},{type},{date.Split('-')[0]},{usefulArea},{turnsToBuild},{turns},in construction,{plotList.IndexOf(plot)},{color}";

        SaveCSV.Instance.WriteNewLineIntoCSV(buildingsCSVPath, newLine);
        CreateFlat(id, name, type, date, usefulArea, turnsToBuild, turns, status, plot, color);
    }

    private string GetEndDate(int currentYear, int currentMonth, int turnsToEnd) {
        int year = currentYear;
        int month = currentMonth;

        month += turnsToEnd;
        int years = (month - 1) / 12;  // Subtract 1 to handle the 0th month issue
        int leftoverMonths = (month - 1) % 12 + 1; // Ensure months are in range 1-12
        year += years;
        month = leftoverMonths;

        if (month < 10) {
            return year.ToString() + "-0" + month.ToString();
        } else {
            return year.ToString() + '-' + month.ToString();
        }

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
                GameEventSystem.Instance.AddToOutput("Befejeződött egy projekt " + SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetProjectsFilePath())[i].Split(',')[(int)SaveCSV.ProjectColumns.Name] + " néven");
                switch (splitLine[(int)SaveCSV.ProjectColumns.Type]){
                    case "repair":
                        populationHappiness += repairHappines;
                        break;
                    case "build":
                        populationHappiness += newBuildingHappines;
                        break;
                }
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

    public void DeleteService(string filePath, int id) {
        List<string> lines = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetServiceFilePath());

        foreach (string line in lines) {
            string[] splitLine = line.Split(',');

            if (splitLine[(int) SaveCSV.ServiceColumns.Id] == id.ToString()) {
                GameEventSystem.Instance.AddToOutput("Befejeződött egy szolgáltatás " + splitLine[(int) SaveCSV.ServiceColumns.Name] + " néven");
                break;
            }   
        }
        SaveCSV.Instance.DeleteFromCSV(filePath, id);
        populationHappiness -= endServiceHappines;

    }

    private void CheckGameState() {
        if (budget <= 0) {
            endGameUI.SetStats("Exceeded budget constraints", budget.ToString(), populationHappiness.ToString(), turnCount.ToString(), simLength.ToString());
            endGameUI.Show();
        }
        if (populationHappiness < populationMinHappiness) {
            endGameUI.SetStats("Happiness fell below the required minimum", budget.ToString(), populationHappiness.ToString(), turnCount.ToString(), simLength.ToString());
            endGameUI.Show();
        }
        if (turnCount >= simLength) {
            endGameUI.SetStats("Simulation time period completed", budget.ToString(), populationHappiness.ToString(), turnCount.ToString(), simLength.ToString());
            endGameUI.Show();
        }
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

    public void UpdateHUD() {
        UpperBarContainer.Instance.ChangeDate(date);
        UpperBarContainer.Instance.ChangeBudget(budget.ToString());
        UpperBarContainer.Instance.ChangeHappiness((populationHappiness - populationMinHappiness) / (populationMaxHappiness - populationMinHappiness));
    }

    private void CreateFlat(string buildCSVLen, string buildingNameText, string buildingTypeText, string buildingYearText, string buildingAreaText, string turnsToBuild, string turns, string status, Plot plot, string color) {

        string buildingId = buildCSVLen;
        Flat flat = Instantiate(flatPrefab, plot.transform);

        int id = Int32.Parse(buildingId);
        string name = buildingNameText;
        string type = buildingTypeText;
        int year = Int32.Parse(buildingYearText.Split('-')[0]);
        float area = float.Parse(buildingAreaText);

        flat.Initialize(id, name, type, year, area, Int32.Parse(turnsToBuild), Int32.Parse(turns), status, plot.transform, color);
        plot.isReserved = true;
        GameEventSystem.Instance.AddToOutput("Létrejött egy új épület " + buildingNameText + " néven");

    }

    public void CreateNewService(string name, string type, string buildingIds, string cost) {

        string servicePath = SaveCSV.Instance.GetServiceFilePath();
        string id = SaveCSV.Instance.GetCSVLength(servicePath).ToString();
        string newLine = $"{id},{name},{type},{buildingIds},{cost}";

        SaveCSV.Instance.WriteNewLineIntoCSV(servicePath, newLine);
        GameEventSystem.Instance.AddToOutput("Létrejött egy új szolgáltatás " + name + " néven");

        populationHappiness += newServiceHappines;

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

    public int SimulationLength()
    {
        return simLength;
    }
}
