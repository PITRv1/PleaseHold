using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using static EventHandlerScript;
using System.Windows.Forms;
using Button = UnityEngine.UI.Button;
using static System.Net.WebRequestMethods;

public class InputFieldBackground : MonoBehaviour {

    [SerializeField] private Transform flatPrefab;

    private RectTransform rectTransform;

    [SerializeField] TMP_InputField buildingName;
    [SerializeField] TMP_InputField buildingType;
    [SerializeField] TMP_InputField buildingArea;
    [SerializeField] TMP_InputField buildingTurnsToBuild;

    [SerializeField] Button acceptButton;
    [SerializeField] Button exitButton;

    private string buildingNameText;
    private string buildingTypeText;
    private string buildingAreaText;
    private string buildingTurnsToBuildText;

    private int averageBuildCostUK = 2400;

    private Transform givenGameObject;

    public static InputFieldBackground Instance {
        private set;
        get;
    }

    private void Awake() {
        Instance = this;
        exitButton.onClick.AddListener(() => {
            Hide();
        });
        acceptButton.onClick.AddListener(() => {

            string buildingNameText = buildingName.text;
            string buildingAreaText = buildingArea.text;
            string buildingTypeText = buildingType.text;
            string buildingTurnsToBuildText = buildingTurnsToBuild.text;

            buildingName.text = "";
            buildingType.text = "";
            buildingArea.text = "";
            buildingTurnsToBuild.text = "";

            string date = GameHandler.Instance.GetDate();
            int currentYear = Int32.Parse(date.Split('-')[0]);
            int currentMonth = Int32.Parse(date.Split('-')[1]);
            float currentDate = currentYear + currentMonth / 100;

            GameHandler.Instance.CreateNewBuildingProject(
                buildingNameText, 
                (averageBuildCostUK * Int32.Parse(buildingAreaText)).ToString(), 
                GameHandler.Instance.GetDate(), 
                GetEndDate(currentYear, currentMonth, 
                Int32.Parse(buildingTurnsToBuildText), 
                GameHandler.Instance.GetTurnCount()), 
                SaveCSV.Instance.GetCSVLength(SaveCSV.Instance.GetBuildingFilePath()).ToString());

            GameHandler.Instance.CreateNewBuilding(buildingNameText, buildingTypeText, GameHandler.Instance.GetDate().ToString(), buildingAreaText, buildingTurnsToBuildText, "in construction", givenGameObject);

            Hide();
        });
    }
    private string GetEndDate(int currentYear, int currentMonth, int turnsToBuild, int turnCount) {
        int year = currentYear;
        int month = currentMonth;

        month += turnsToBuild + turnCount;
        int years = (month - 1) / 12;  // Subtract 1 to handle the 0th month issue
        int leftoverMonths = (month - 1) % 12 + 1; // Ensure months are in range 1-12
        year += years;
        month = leftoverMonths;

        return year.ToString() + '-' + month.ToString();
    }

    private void Start() {
        EventHandlerScript.Instance.OnPlotRightClick += EventHandlerScript_OnPlotRightClick;
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    private void EventHandlerScript_OnPlotRightClick(object sender, BuildingEventArgs e) {
        givenGameObject = e.gameObject;

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            Input.mousePosition,
            null,  // Canvas in Screen Space - Overlay doesn't need a Camera
            out mousePos
        );

        float screenHeight = UnityEngine.Screen.height;  // Get screen height
        float screenWidth = UnityEngine.Screen.width;  // Get screen width
        float mouseY = Input.mousePosition.y;  // Get mouse Y position
        float mouseX = Input.mousePosition.x;  // Get mouse X position

        float width = rectTransform.sizeDelta.x;
        float height = rectTransform.sizeDelta.y;

        if ((screenWidth - width - mouseX) < 0) width += (screenWidth - width - mouseX) * 2;

        if (mouseY > screenHeight / 2) {
            rectTransform.anchoredPosition = mousePos + new Vector2(width / 2, -height / 2);
        } else {
            rectTransform.anchoredPosition = mousePos + new Vector2(width / 2, height / 2);
        }
        Show();
    }

    private void Hide() {
        //CameraSystem.Instance.;
        gameObject.SetActive(false);
        buildingName.text = "";
        buildingType.text = "";
        buildingArea.text = "";
    }
    private void Show() {
        //CameraSystem.Instance.DisableInput();
        gameObject.SetActive(true);
    }
}
