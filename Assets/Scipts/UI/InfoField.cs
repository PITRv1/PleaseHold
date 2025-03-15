using TMPro;
using UnityEngine;

public class InfoField : MonoBehaviour {

    [SerializeField] TextMeshProUGUI buildingNameValue;
    [SerializeField] TextMeshProUGUI buildingTypeValue;
    [SerializeField] TextMeshProUGUI buildingYearValue;
    [SerializeField] TextMeshProUGUI buildingAreaValue;

    private RectTransform rectTransform;


    public static InfoField Instance {
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

    private void Flat_OnFlatExit(object sender, EventHandlerScript.BuildingEventArgs e) {
        Hide();
    }

    private void Flat_OnFlatEnter(object sender, EventHandlerScript.BuildingEventArgs e) {
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
            rectTransform.anchoredPosition = mousePos + new Vector2(250, -200);
        } else {
            rectTransform.anchoredPosition = mousePos + new Vector2(250, 200);
        }
        Flat flatScript = e.gameObject.GetComponent<Flat>();

        Debug.Log(flatScript.GetBuildingName());

        buildingNameValue.text = flatScript.GetBuildingName();
        buildingTypeValue.text = flatScript.GetBuildingType();
        buildingYearValue.text = flatScript.GetBuildingYear().ToString();
        buildingAreaValue.text = flatScript.GetBuildingArea().ToString();
        Show();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
    private void Show() {
        gameObject.SetActive(true);
    }
}
