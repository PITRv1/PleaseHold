using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using static EventHandlerScript;

public class InputFieldBackground : MonoBehaviour {

    public event EventHandler<CreateNewBuildingProjectParams> CreateNewBuildingProject;

    public class CreateNewBuildingProjectParams : EventArgs {

        private string buildingNameText;
        private string buildingTypeText;
        private string buildingYearText;
        private string buildingAreaText;
        private string buildingTurnsToBuildText;

        public CreateNewBuildingProjectParams(string buildingNameText, string buildingTypeText, string buildingYearText, string buildingAreaText, string buildingTurnsToBuildText) {
            this.buildingNameText = buildingNameText;
            this.buildingTypeText = buildingTypeText;
            this.buildingYearText = buildingYearText;
            this.buildingAreaText = buildingAreaText;
            this.buildingTurnsToBuildText = buildingTurnsToBuildText;
        }
    }

    [SerializeField] private Transform flatPrefab;

    private RectTransform rectTransform;

    [SerializeField] TMP_InputField buildingName;
    [SerializeField] TMP_InputField buildingType;
    [SerializeField] TMP_InputField buildingYear;
    [SerializeField] TMP_InputField buildingArea;
    [SerializeField] TMP_InputField buildingTurnsToBuild;

    [SerializeField] Button acceptButton;
    [SerializeField] Button exitButton;

    private string buildingNameText;
    private string buildingTypeText;
    private string buildingYearText;
    private string buildingAreaText;
    private string buildingTurnsToBuildText;

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

            buildingNameText = buildingName.text;
            buildingTypeText = buildingType.text;
            buildingYearText = buildingYear.text;
            buildingAreaText = buildingArea.text;
            buildingTurnsToBuildText = buildingTurnsToBuild.text;

            buildingName.text = "";
            buildingType.text = "";
            buildingYear.text = "";
            buildingArea.text = "";
            buildingTurnsToBuild.text = "";

            string date = GameHandler.Instance.GetDate();
            int currentYear = Int32.Parse(date.Split('-')[0]);
            int currentMonth = Int32.Parse(date.Split('-')[1]);
            float currentDate = currentYear + currentMonth / 100;

            buildingTurnsToBuildText = GetEndDate(currentYear, currentMonth, Int32.Parse(buildingTurnsToBuildText));
            CreateNewBuildingProject?.Invoke(this, new CreateNewBuildingProjectParams(buildingNameText, buildingTypeText, buildingYearText, buildingAreaText, buildingTurnsToBuildText));

            Hide();
        });
    }
    private string GetEndDate(int currentYear, int currentMonth, int turns) {
        int year = currentYear;
        int month = currentMonth;

        month += turns;
        int addYears = month / 12;
        year += addYears;
        int leftoverMonth = month % 12;
        month = leftoverMonth;

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

        float screenHeight = Screen.height;  // Get screen height
        float screenWidth = Screen.width;  // Get screen width
        float mouseY = Input.mousePosition.y;  // Get mouse Y position
        float mouseX = Input.mousePosition.x;  // Get mouse X position

        float width = rectTransform.sizeDelta.x;
        float height = rectTransform.sizeDelta.y;

        Debug.Log(screenWidth);
        Debug.Log(width);
        Debug.Log(mouseX);
        Debug.Log(screenWidth - width - mouseX);

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
        buildingYear.text = "";
        buildingArea.text = "";
    }
    private void Show() {
        //CameraSystem.Instance.DisableInput();
        gameObject.SetActive(true);
    }
}
