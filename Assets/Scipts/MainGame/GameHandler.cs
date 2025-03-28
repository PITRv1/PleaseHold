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

    [SerializeField] float repairHappines;
    [SerializeField] float newBuildingHappines;
    [SerializeField] float newServiceHappines;
    [SerializeField] float endServiceHappines;
    [Range(0, 1)]
    [SerializeField] float governmentFundingNormal;
    [SerializeField] EventDisplay eventDisplay;
    [SerializeField] EndGameUI endGameUI;
    [SerializeField] NPCSpawner npcSpawner;

    [SerializeField] float BuildingCost = 2.4f;

    [Header("Random Event Chances (must equate to one)")]

    [Range(0, 1)]
    [SerializeField] float nothingChance = 0.5f;
    [Range(0, 1)]
    [SerializeField] float fireChance = 0.05f;
    [Range(0, 1)]
    [SerializeField] float pipeBreakChance = 0.15f;
    [Range(0, 1)]
    [SerializeField] float fundingChance = 0.2f;
    [Range(0, 1)]
    [SerializeField] float famineChance = 0.1f;

    [Header("Building Burning Chances (apart from the starting house)")]

    [Range(0, 1)]
    [SerializeField] float oneHouse = 0.8f;
    [Range(0, 1)]
    [SerializeField] float twoHouses = 0.5f;
    [Range(0, 1)]
    [SerializeField] float threeHouses = 0.2f;
    [Range(0, 1)]
    [SerializeField] float fourHouses = 0.1f;
    [Range(0, 1)]
    [SerializeField] float fiveHouses = 0.08f;
    [Range(0, 1)]
    [SerializeField] float sixHouses = 0.05f;
    [Range(0, 1)]
    [SerializeField] float sevenHouses = 0.01f;


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
    private string oldDate;
    private float oldPopulationHappiness;
    private float oldBudget;

    [SerializeField] private string[] firstNames = { "Varga", "Molnár", "Kovács", "Takács", "Tóth", "Szabó", "Nagy" };
    [SerializeField] private string[] lastNames = { "Varga", "Molnár", "Kovács", "Takács", "Tóth", "Szabó", "Nagy" };
    [SerializeField] private string[] jobs = { "nincs", "tanuló", "programozó", "orvos", "sofőr", "építész", "könyvelő", "ápoló", "szakács", "pincér" };
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
        initalBudget = (int)float.Parse(SaveCSV.Instance.GetInitialBudget());
        simLength = Int32.Parse(SaveCSV.Instance.GetSimulationLength());

        populationHappiness = populationStartHappiness;

        population = SaveCSV.Instance.GetCSVLength(SaveCSV.Instance.GetResidentFilePath()) - 1;

        budget = initalBudget;

        endDate = GetEndDate(simStartYear, simStartMonth, simLength);

        UpdateDate();
        UpdateOldDate();
        UpdateOldBudget();
        UpdateOldHappiness();
        UpdateHUD();
        npcSpawner.SpawnNPCs(population);
        SaveToJson();
    }
    public void NewMonth() {
        turnCount += 1;
        UpdateDate();
        HandleProjects();
        HandleServices();
        NewMonthEvent?.Invoke(this, EventArgs.Empty);
        UpdateOldDate();
        UpdateOldBudget();
        UpdateOldHappiness();
        population = SaveCSV.Instance.GetCSVLength(SaveCSV.Instance.GetResidentFilePath());
        RandomEvent();
        GovernmentFunding();
        SaveToJson();
        CheckGameState();
        populationHappiness = Mathf.Clamp(populationHappiness, populationMinHappiness, populationMaxHappiness); // Just make sure it works bruh
        UpdateHUD();
        npcSpawner.SpawnNPCs(population);
    }

    private void GovernmentFunding() {
        List<string> lines = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetServiceFilePath());

        lines.RemoveAt(0);

        float funding = 0f;

        foreach (string line in lines) {
            string[] splitLine = line.Split(',');

            funding += float.Parse(splitLine[(int)SaveCSV.ServiceColumns.Cost]) * governmentFundingNormal;
        }

        budget += funding;
        GameEventSystem.Instance.AddToOutput($"Az állam a szolgáltatásaid alapján adott {funding} pénzt.");
    }

    public float GetBuildingCost() {
        return BuildingCost;
    }

    public string GetOldDate() {
        return oldDate;
    }
    public float GetOldBudget() {
        return oldBudget;
    }
    public float GetOldHappiness() {
        return oldPopulationHappiness;
    }

    private void UpdateOldDate() {
        oldDate = date;
    }
    private void UpdateOldBudget() {
        oldBudget = budget;
    }
    private void UpdateOldHappiness() {
        oldPopulationHappiness = populationHappiness;
    }
    private void RandomEvent() {
        float chance = UnityEngine.Random.value;

        if (chance <= fireChance) {

            // add house selection logic here it shuldnt be that hard

            List<Flat> flatList = FlatHandler.Instance.GetFlatList();
            List<Plot> plotList = PlotHandler.Instance.GetPlotList();

            Flat flat = flatList[UnityEngine.Random.Range(0, flatList.Count)];

            Plot plot = flat.transform.parent?.GetComponent<Plot>();

            int plotIndex = plotList.IndexOf(plot);
            int plotBlockIndex = (plotIndex + 1) % 8; // There are 8 buildings in a block
            if (plotBlockIndex == 0) plotBlockIndex = 8;
            int plotStartDistance = plotIndex - (Math.Abs(1 - plotBlockIndex));
            List<Plot> plotListSlice = plotList.GetRange(plotStartDistance, 8);

            Plot[] plotBlock = { plotListSlice[4], plotListSlice[2], plotListSlice[1], plotListSlice[0], plotListSlice[3], plotListSlice[5], plotListSlice[6], plotListSlice[7] }; // Rearange the list

            plotIndex = Array.IndexOf(plotBlock, plot);

            float houseBurnChance = UnityEngine.Random.value;

            houseBurnChance *= oneHouse + twoHouses + threeHouses + fourHouses + fiveHouses + sixHouses + sevenHouses;

            int housesToBurn;

            if (houseBurnChance <= oneHouse) {
                housesToBurn = 1;
            } else if (houseBurnChance <= oneHouse + twoHouses) {
                housesToBurn = 2;
            } else if (houseBurnChance <= oneHouse + twoHouses + threeHouses) {
                housesToBurn = 3; ;
            } else if (houseBurnChance <= oneHouse + twoHouses + threeHouses + fourHouses) {
                housesToBurn = 4;
            } else if (houseBurnChance <= oneHouse + twoHouses + threeHouses + fourHouses + fiveHouses) {
                housesToBurn = 5;
            } else if (houseBurnChance <= oneHouse + twoHouses + threeHouses + fourHouses + fiveHouses + sixHouses) {
                housesToBurn = 6;
            } else {
                housesToBurn = 7;
            }

            int lastDir = UnityEngine.Random.Range(0, 2); // Two cuz it's exclusive

            int lastHouse = housesToBurn % 2;

            int twoDir = (housesToBurn - lastHouse) / 2;

            int plotIndexLeft = twoDir;
            int plotIndexRight = twoDir;

            List<string> indexes = new();

            if (lastDir == 0) plotIndexLeft += lastHouse; else plotIndexRight += lastHouse;

            int currIndex = plotIndex;

            int burnedHauseCount = 0;

            if (plot.TryGetFlatOnPlot(out Flat flatOnPlotFirst) == true) {

                flatOnPlotFirst.SetStatus(5);
                burnedHauseCount++;
                indexes.Add(flatOnPlotFirst.GetBuildingId().ToString());
            }

            for (int index = 1; index < plotIndexLeft + 1; index++) {

                int wrappedIndexLeft = ((plotIndex - index) % plotBlock.Length + plotBlock.Length) % plotBlock.Length;

                Plot burnedPlot = plotBlock[wrappedIndexLeft];

                if (burnedPlot.TryGetFlatOnPlot(out Flat flatOnPlot) == false) {
                    break;
                }

                int houseStatus = 6 - index;

                houseStatus = Mathf.Clamp(houseStatus, 2, 5);

                indexes.Add(flatOnPlot.GetBuildingId().ToString());
                if (flatOnPlot.GetBuildingStatusInt() < houseStatus) flatOnPlot.SetStatus(houseStatus);
                burnedHauseCount++;
            }
            for (int index = 1; index < plotIndexRight + 1; index++) {

                int wrappedIndexRight = ((plotIndex + index) % plotBlock.Length + plotBlock.Length) % plotBlock.Length;

                Plot burnedPlot = plotBlock[wrappedIndexRight];

                if (burnedPlot.TryGetFlatOnPlot(out Flat flatOnPlot) == false) {
                    break;
                }

                int houseStatus = 6 - index;

                houseStatus = Mathf.Clamp(houseStatus, 2, 5);
                indexes.Add(flatOnPlot.GetBuildingId().ToString());
                if (flatOnPlot.GetBuildingStatusInt() < houseStatus) flatOnPlot.SetStatus(houseStatus);
                burnedHauseCount++;
            }

            float decrease = 0.1f;

            decrease += (burnedHauseCount - (burnedHauseCount % 2)) / 2 * 0.05f;

            populationHappiness -= (populationMaxHappiness - populationMinHappiness) * decrease;

            string strIndexes = string.Join(", ", indexes);

            eventDisplay.SetEventName("FIREEE");
            eventDisplay.SetEventText($"A fire breaks damaging {burnedHauseCount} buildings ({strIndexes}) in the process. The happiness falls by {decrease * 100f}%.");
            eventDisplay.SetEventColor(Color.red);

            GameEventSystem.Instance.AddToOutput($"Egy tűz kitört, megsebezve {burnedHauseCount} épületet ({strIndexes}). A boldogság {decrease * 100f}%-al csökkent.");

        } else if (chance <= famineChance + fireChance) {
            eventDisplay.SetEventName("City wide famine");
            eventDisplay.SetEventText("A food shortage has hit the city. Lose 15% population happiness.");
            eventDisplay.SetEventColor(Color.red);

            GameEventSystem.Instance.AddToOutput("A várost éhínség sújtotta, és elvesztette a lakosság a boldogságának 15%-át.");

            populationHappiness -= (populationMaxHappiness - populationMinHappiness) * 0.15f;
        } else if (chance <= pipeBreakChance + famineChance + fireChance) {
            eventDisplay.SetEventName("Water pipe disaster");
            eventDisplay.SetEventText("A major water pipe in the city has raptured. You use 10.000$ to fix it but people are 5% less happy.");
            eventDisplay.SetEventColor(Color.red);

            GameEventSystem.Instance.AddToOutput("A városban elszakadt egy nagy cső, ami 10.000 dolláros kárt okozott. Az emberek 5%-kal kevésbé boldogak.");

            budget -= 10000f;
            populationHappiness -= (populationMaxHappiness - populationMinHappiness) * 0.05f;

        } else if (chance <= fundingChance + pipeBreakChance + famineChance + fireChance) {
            eventDisplay.SetEventName("Extra Government funding");
            eventDisplay.SetEventText("The city by miracle has won an application for extra government funding that it applied for. Gain 100.000$ extra budget.");
            eventDisplay.SetEventColor(Color.green);

            GameEventSystem.Instance.AddToOutput("A város 100.000 dollár plusz támogatást kapott.");

            budget += 100000f;
        } else {
            eventDisplay.SetEventName("Nothing happens");
            eventDisplay.SetEventText("This month nothing happened.");
            eventDisplay.SetEventColor(Color.blue);

            GameEventSystem.Instance.AddToOutput("Ebben a hónapban nem történt semmi.");
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
        string job = jobs[UnityEngine.Random.Range(0, jobs.Length)];

        population += 1;

        string newLine = $"{SaveCSV.Instance.GetCSVLength(residentPath).ToString()},{name},{age},{job},{houseId}";
        SaveCSV.Instance.WriteNewLineIntoCSV(residentPath, newLine);

        GameEventSystem.Instance.AddToOutput($"Van egy új lakos a városodban {name} néven.");
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

    public string GetEndDate(int currentYear, int currentMonth, int turnsToEnd) {
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
        List<int> deleteFromCSV = new List<int>();
        for (int i = 0; i < projectCSV.Count(); i++) {

            if (i == 0) continue;

            List<string> splitLine = projectCSV[i].Split(',').ToList();

            if (splitLine[(int)SaveCSV.ProjectColumns.EndDate] == date) {
                GameEventSystem.Instance.AddToOutput("Befejeződött egy projekt " + SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetProjectsFilePath())[i].Split(',')[(int)SaveCSV.ProjectColumns.Name] + " néven");
                switch (splitLine[(int)SaveCSV.ProjectColumns.Type]) {
                    case "repair":
                        populationHappiness += repairHappines;
                        break;
                    case "build":
                        populationHappiness += newBuildingHappines;
                        break;
                }

                deleteFromCSV.Add(i);
            } else {
                budget -= float.Parse(splitLine[(int)SaveCSV.ProjectColumns.Cost]);
            }
        }
        foreach (int index in deleteFromCSV) {
            SaveCSV.Instance.DeleteFromCSV(projectPath, index);
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

            if (splitLine[(int)SaveCSV.ServiceColumns.Id] == id.ToString()) {
                GameEventSystem.Instance.AddToOutput("Befejeződött egy szolgáltatás " + splitLine[(int)SaveCSV.ServiceColumns.Name] + " néven");
                break;
            }
        }
        SaveCSV.Instance.DeleteFromCSV(filePath, id);
        populationHappiness -= endServiceHappines;

    }

    private void CheckGameState() {
        float happinesPercent = ((populationHappiness - populationMinHappiness) / (populationMaxHappiness - populationMinHappiness));

        if (budget <= 0) {
            if (!(happinesPercent > 0)) {
                happinesPercent = 0;
            }
            GameEventSystem.Instance.EndOutput(1);
            endGameUI.SetStats("Exceeded budget constraints", budget.ToString(), happinesPercent, turnCount.ToString(), simLength.ToString());
            endGameUI.Show();
        }
        if (populationHappiness < populationMinHappiness) {
            if (!(happinesPercent > 0)) {
                happinesPercent = 0;
            }
            GameEventSystem.Instance.EndOutput(2);
            endGameUI.SetStats("Happiness fell below the required minimum", budget.ToString(), happinesPercent, turnCount.ToString(), simLength.ToString());
            endGameUI.Show();
        }
        if (turnCount >= simLength) {
            if (!(happinesPercent > 0)) {
                happinesPercent = 0;
            }
            GameEventSystem.Instance.EndOutput(3);
            endGameUI.SetStats("Simulation time period completed", budget.ToString(), happinesPercent, turnCount.ToString(), simLength.ToString());
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

        if (month < 10) {
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
        FlatHandler.Instance.AddToFlatList(flat);
        plot.SetPlot(flat);
        plot.isReserved = true;
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

    public float GetHappiness() {
        return populationHappiness;
    }

    public float GetOldHappinessPercetige() {
        return ((oldPopulationHappiness - populationMinHappiness) / (populationMaxHappiness - populationMinHappiness)) >= 0f ? ((oldPopulationHappiness - populationMinHappiness) / (populationMaxHappiness - populationMinHappiness)) : 0f;
    }

    public float GetHappinessPercentige() {
        return ((populationHappiness - populationMinHappiness) / (populationMaxHappiness - populationMinHappiness)) >= 0f ? ((populationHappiness - populationMinHappiness) / (populationMaxHappiness - populationMinHappiness)) : 0f;
    }

    public float GetBudget() {
        return budget;
    }

    public int SimulationLength() {
        return simLength;
    }
}
