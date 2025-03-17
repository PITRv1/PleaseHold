using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameHandler : MonoBehaviour {

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

    private void Start() {
        InputFieldBackground.Instance.CreateNewBuildingProject += InputField_CreateNewBuildingProject;
    }

    private void InputField_CreateNewBuildingProject(object sender, InputFieldBackground.CreateNewBuildingProjectParams e) {
        //string projectsCSV = SaveCSV.Instance.GetPro
        //SaveCSV.Instance.WriteNewLineIntoCSV()
    }

    private void AddToTurn_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        turnCount += 1;
        changeTurnCountUI?.Invoke(this, new UI(turnCount));
    }

    private void UpdateDate() {
        int year = simStartYear;
        int month = simStartMonth;

        month += turnCount;
        int addYears = month / 12;
        year += addYears;
        int leftoverMonth = month % 12;
        month = leftoverMonth;

        date = year.ToString() + '-' + month.ToString();
    }

    private void CreateFlat() {

        //string buildingId = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetBuildingFilePath()).Count.ToString();
        //string[] newLine = { $"{buildingId}, {buildingNameText}, {buildingTypeText}, {buildingYearText}, {buildingAreaText}" };

        //SaveCSV.Instance.WriteNewLinesIntoCSV(SaveCSV.Instance.GetBuildingFilePath(), newLine);

        //Transform flat = Instantiate(flatPrefab, givenGameObject.transform);

        //Flat flatScript = flat.GetComponent<Flat>();
        //Plot plotScript = givenGameObject.GetComponent<Plot>();

        //int id = Int32.Parse(buildingId);
        //string name = buildingNameText;
        //string type = buildingTypeText;
        //int year = Int32.Parse(buildingYearText);
        //float area = float.Parse(buildingAreaText);

        //flatScript.Initialize(id, name, type, year, area, givenGameObject);
        //plotScript.isReserved = true;
    }

    public string GetDate() {
        UpdateDate();
        return date;
    }
}
