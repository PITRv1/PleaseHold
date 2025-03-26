using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class NewProjectTabUI : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    [SerializeField] TMP_InputField projectName;
    [SerializeField] TMP_InputField projectType;
    [SerializeField] TMP_InputField projectTurnsToComplete;
    [SerializeField] TMP_InputField projectBuildings;
    [SerializeField] TMP_InputField projectCost;

    [SerializeField] Button acceptButton;


    private string projectNameText;
    private string projectTypeText;
    private string projectTurnsToCompleteText;
    private string projectBuildingsText;
    private string projectCostText;


    private bool isShowing;

    private FadeControllerUI fadeControllerUI;

    private void Awake()
    {
        fadeControllerUI = GetComponent<FadeControllerUI>();

    }

    private void Start()
    {
        CameraSystem.Instance.EnableCamInputs();
        isShowing = false;
        gameObject.SetActive(false);

        exitButton.onClick.AddListener(Hide);

        acceptButton.onClick.AddListener(() => {

            projectNameText = projectName.text;
            projectTypeText = projectType.text;
            projectTurnsToCompleteText = projectTurnsToComplete.text;
            projectBuildingsText = projectBuildings.text;
            projectCostText = projectCost.text;

            projectName.text = "";
            projectType.text = "";
            projectTurnsToComplete.text = "";
            projectBuildings.text = "";
            projectCost.text = "";

            string date = GameHandler.Instance.GetDate();
            int currentYear = Int32.Parse(date.Split('-')[0]);
            int currentMonth = Int32.Parse(date.Split('-')[1]);
            float currentDate = currentYear + currentMonth / 100;

            //Project Creation Logic Here

            GameHandler.Instance.CreateNewProject(projectNameText, projectCostText, date, GameHandler.Instance.GetEndDate(currentYear, currentMonth, Int32.Parse(projectTurnsToCompleteText)), projectBuildingsText, "repair");
            List<string> lines = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetBuildingFilePath());
            string[] buildingIds = projectBuildingsText.Split(';');
            foreach (string id in buildingIds) {
                int buildId = Int32.Parse(id);

                if (buildId > SaveCSV.Instance.GetCSVLength(SaveCSV.Instance.GetBuildingFilePath())) Debug.LogError("One of the building ids is invalid");

                SaveCSV.Instance.EditOneValueOnLine(buildId, SaveCSV.BuildingColumns.TurnsToFinish, SaveCSV.Instance.GetBuildingFilePath(), projectTurnsToCompleteText);
                SaveCSV.Instance.EditOneValueOnLine(buildId, SaveCSV.BuildingColumns.Turns, SaveCSV.Instance.GetBuildingFilePath(), "0");
            }
            
            Hide();
        });
    }

    public void Hide()
    {
        CameraSystem.Instance.EnableCamInputs();
        isShowing = false;
        fadeControllerUI.FadeOut(.2f);

        projectName.text = "";
        projectType.text = "";
        projectTurnsToComplete.text = "";
        projectBuildings.text = "";
        projectCost.text = "";
    }
    public void Show()
    {
        CameraSystem.Instance.DisableCamInputs();
        isShowing = true;
        fadeControllerUI.FadeIn(.2f);
    }
}
