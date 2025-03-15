using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using static EventHandlerScript;

public class InputFieldBackground : MonoBehaviour {

    [SerializeField] private Transform flatPrefab;

    private RectTransform rectTransform;

    [SerializeField] TMP_InputField buildingName;
    [SerializeField] TMP_InputField buildingType;
    [SerializeField] TMP_InputField buildingYear;
    [SerializeField] TMP_InputField buildingArea;

    [SerializeField] Button acceptButton;

    private string buildingNameText;
    private string buildingTypeText;
    private string buildingYearText;
    private string buildingAreaText;

    private Transform givenGameObject;

    public static InputFieldBackground Instance {
        private set;
        get;
    }

    private void Awake() {
        Instance = this;
        acceptButton.onClick.AddListener(() => {
            buildingNameText = buildingName.text;
            buildingTypeText = buildingType.text;
            buildingYearText = buildingYear.text;
            buildingAreaText = buildingArea.text;

            buildingName.text = "";
            buildingType.text = "";
            buildingYear.text = "";
            buildingArea.text = "";

            string buildingId = SaveCSV.Instance.ReadLinesFromCSV().Count.ToString();
            string[] newLine = { $"{buildingId}, {buildingNameText}, {buildingTypeText}, {buildingYearText}, {buildingAreaText}" };

            SaveCSV.Instance.WriteNewLinesIntoCSV(newLine);

            Transform flat = Instantiate(flatPrefab, givenGameObject.transform);

            Flat flatScript = flat.GetComponent<Flat>();
            Plot plotScript = givenGameObject.GetComponent<Plot>();

            int id = Int32.Parse(buildingId);
            string name = buildingNameText;
            string type = buildingTypeText;
            int year = Int32.Parse(buildingYearText);
            float area = float.Parse(buildingAreaText);

            flatScript.Initialize(id, name, type, year, area, givenGameObject);
            plotScript.isReserved = true;

            Hide();
        });
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
        float mouseY = Input.mousePosition.y;  // Get mouse Y position

        if (mouseY > screenHeight / 2) {
            rectTransform.anchoredPosition = mousePos + new Vector2(250, -325);
        } else {
            rectTransform.anchoredPosition = mousePos + new Vector2(250, 325);
        }
        Show();
    }

    private void Hide() {
        CameraSystem.Instance.EnableInput();
        gameObject.SetActive(false);
    }
    private void Show() {
        CameraSystem.Instance.DisableInput();
        gameObject.SetActive(true);
    }
}
