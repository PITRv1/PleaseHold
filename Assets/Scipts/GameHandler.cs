using UnityEngine;

public class GameHandler : MonoBehaviour {

    private CameraInput_Actions cameraInputActions;

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
        cameraInputActions.Camera.addToTurn.performed += AddToTurn_performed;
    }

    private void AddToTurn_performed(UnityEngine.InputSystem.InputAction.CallbackContext context) {
        throw new System.NotImplementedException();
    }
}
