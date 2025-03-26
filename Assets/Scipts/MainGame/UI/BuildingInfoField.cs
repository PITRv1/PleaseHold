using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoField : MonoBehaviour {

    [SerializeField] TextMeshProUGUI buildingNameValue;
    [SerializeField] TextMeshProUGUI buildingTypeValue;
    [SerializeField] TextMeshProUGUI buildingYearValue;
    [SerializeField] TextMeshProUGUI buildingAreaValue;
    [SerializeField] TextMeshProUGUI buildingStatusValue;

    private RectTransform rectTransform;


    public static BuildingInfoField Instance {
        private set;
        get;
    }

    private void Awake() {
        Instance = this;
    }
    private void Start() {
        EventHandlerScript.Instance.OnFlatEnter += Flat_OnFlatEnter;
        EventHandlerScript.Instance.OnFlatExit += Flat_OnFlatExit;
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    private void Flat_OnFlatExit(object sender, EventHandlerScript.BuildingEventArgsFlat e) {
        Hide();
    }

    private void Flat_OnFlatEnter(object sender, EventHandlerScript.BuildingEventArgsFlat e) {
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

        if ((screenWidth - width - mouseX) < 0) width += screenWidth - width - mouseX;

        if (mouseY > screenHeight / 2) {
            rectTransform.anchoredPosition = mousePos + new Vector2(width / 2, -height / 2);
        } else {
            rectTransform.anchoredPosition = mousePos + new Vector2(width / 2, height / 2);
        }

        buildingNameValue.text = e.flat.GetBuildingName();
        buildingTypeValue.text = e.flat.GetBuildingType();
        buildingYearValue.text = e.flat.GetBuildingYear().ToString();
        buildingAreaValue.text = e.flat.GetBuildingArea().ToString();
        buildingStatusValue.text = e.flat.GetBuildingStatus();

        Show();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
    private void Show() {
        gameObject.SetActive(true);
    }
}
