using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using static EventHandlerScript;
using System.Windows.Forms;
using Button = UnityEngine.UI.Button;
using static System.Net.WebRequestMethods;
using System.Collections.Generic;

public class BuildingField : MonoBehaviour {

    [SerializeField] private Transform flatPrefab;

    private RectTransform rectTransform;

    [SerializeField] TMP_InputField repairCost;
    [SerializeField] TMP_InputField repairTurnsToBuild;

    [SerializeField] Button acceptButton;
    [SerializeField] Button exitButton;

    private string repairCostText;
    private string buildingTurnsToBuildText;

    private Transform givenGameObject;
    public static BuildingField Instance {
        private set;
        get;
    }

    private void Awake() {
        Instance = this;
        exitButton.onClick.AddListener(() => {
            Hide();
        });
        acceptButton.onClick.AddListener(() => {

            repairCostText = repairCost.text;
            buildingTurnsToBuildText = repairTurnsToBuild.text;

            repairCost.text = "";
            repairTurnsToBuild.text = "";

            string date = GameHandler.Instance.GetDate();
            int currentYear = Int32.Parse(date.Split('-')[0]);
            int currentMonth = Int32.Parse(date.Split('-')[1]);
            float currentDate = currentYear + currentMonth / 100;
            Flat flatScript = givenGameObject.GetComponent<Flat>();

            GameHandler.Instance.CreateNewBuildingProject(
                flatScript.GetBuildingName() + "-Repair",
                repairCostText,
                GameHandler.Instance.GetDate(),
                GetEndDate(currentYear, currentMonth, Int32.Parse(buildingTurnsToBuildText)),
                flatScript.GetBuildingId().ToString());

            List<string> lines = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetBuildingFilePath());
            SaveCSV.Instance.EditOneValueOnLine(flatScript.GetBuildingId(), SaveCSV.BuildingColumns.TurnsToFinish, SaveCSV.Instance.GetBuildingFilePath(), buildingTurnsToBuildText);
            SaveCSV.Instance.EditOneValueOnLine(flatScript.GetBuildingId(), SaveCSV.BuildingColumns.Turns, SaveCSV.Instance.GetBuildingFilePath(), "0");

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
        EventHandlerScript.Instance.OnFlatRightClick += EventHandlerScript_OnFlatRightClick;
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    private void EventHandlerScript_OnFlatRightClick(object sender, BuildingEventArgs e) {
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
        repairCost.text = "";
        repairTurnsToBuild.text = "";
    }
    private void Show() {
        //CameraSystem.Instance.DisableInput();
        gameObject.SetActive(true);
    }
}
