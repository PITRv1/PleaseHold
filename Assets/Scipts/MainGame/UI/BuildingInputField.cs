using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using static EventHandlerScript;
using System.Windows.Forms;
using Button = UnityEngine.UI.Button;
using static System.Net.WebRequestMethods;

public class BuildingInputField : MonoBehaviour {

    [SerializeField] private Transform flatPrefab;

    private RectTransform rectTransform;

    [SerializeField] TMP_InputField buildingName;
    [SerializeField] TMP_Dropdown buildingTypeDropdown;
    [SerializeField] TMP_InputField buildingArea;
    [SerializeField] TMP_InputField buildingTurnsToBuild;

    [SerializeField] Button acceptButton;
    [SerializeField] Button exitButton;

    private string buildingNameText;
    private string buildingTypeText;
    private string buildingAreaText;
    private string buildingTurnsToBuildText;

    private int averageBuildCostUK = 2400;

    private Plot plot;

    public static BuildingInputField Instance {
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
            string buildingTypeText = buildingTypeDropdown.options[buildingTypeDropdown.value].text;
            string buildingTurnsToBuildText = buildingTurnsToBuild.text;

            buildingName.text = "";
            //buildingTypeDropdown.options[buildingTypeDropdown.value].text = "";
            buildingArea.text = "";
            buildingTurnsToBuild.text = "";

            string date = GameHandler.Instance.GetDate();
            int currentYear = Int32.Parse(date.Split('-')[0]);
            int currentMonth = Int32.Parse(date.Split('-')[1]);
            float currentDate = currentYear + currentMonth / 100;

            GameHandler.Instance.CreateNewProject(
                buildingNameText, 
                (averageBuildCostUK * Int32.Parse(buildingAreaText)).ToString(), 
                GameHandler.Instance.GetDate(),
                GetEndDate(currentYear, currentMonth, Int32.Parse(buildingTurnsToBuildText)), 
                SaveCSV.Instance.GetCSVLength(SaveCSV.Instance.GetBuildingFilePath()).ToString(),
                "build");

            string[] colors = { "blue", "brown", "green", "purple", "yellow" };

            GameHandler.Instance.CreateNewBuilding(buildingNameText, buildingTypeText, GameHandler.Instance.GetDate().ToString(), buildingAreaText, buildingTurnsToBuildText, "0", "in construction", plot, colors[UnityEngine.Random.Range(0, colors.Length)]);

            Hide();
        });
    }
    private string GetEndDate(int currentYear, int currentMonth, int turnsToBuild) {
        int year = currentYear;
        int month = currentMonth;

        month += turnsToBuild;
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

    private void Start() {
        EventHandlerScript.Instance.OnPlotRightClick += EventHandlerScript_OnPlotRightClick;
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    private void EventHandlerScript_OnPlotRightClick(object sender, BuildingEventArgsPlot e) {
        plot = e.plot;

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
        CameraSystem.Instance.EnableCamInputs();
        gameObject.SetActive(false);
        buildingName.text = "";
        //buildingTypeDropdown.options[0] = buildingTypeDropdown.options[0];
        buildingArea.text = "";
    }
    private void Show() {
        CameraSystem.Instance.DisableCamInputs();
        gameObject.SetActive(true);
    }
}
