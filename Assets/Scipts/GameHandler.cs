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

    private int turnCount;
    private int simStartYear;
    private int simStartMonth;
    private int simStartLength;
    private float populationStartHappiness;
    private int initalBudget;

    private int simYear;
    private int simMonth;
    private int simLength;
    private float populationHappiness;
    private int budget;


    public static GameHandler Instance {
        private set;
        get;
    }

    private void Awake() {
        Instance = this;
        cameraInputActions = new CameraInput_Actions();
        Debug.Log(cameraInputActions.Camera.addToTurn.GetBindingDisplayString());
        cameraInputActions.Camera.Enable();
        cameraInputActions.Camera.addToTurn.performed += AddToTurn_performed;
    }

    private void AddToTurn_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        turnCount += 1;
        changeTurnCountUI?.Invoke(this, new UI(turnCount));
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
}
