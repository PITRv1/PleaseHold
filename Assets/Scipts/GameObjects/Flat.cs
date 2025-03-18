using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Flat : Buildings, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] Flat flat;
    [SerializeField] Image Timer;
    [SerializeField] Transform HeadsUpDisplay;

    private Transform buildingPlot;

    public enum States {
        InConstruction,
        Built,
    }

    private States state;

    private int buildingid;
    private string buildingName;
    private string buildingType;
    private int buildingYear;
    private float buildingArea;
    private string buildingStatus;
    private int buildingTurnsWhenFinish;

    private void Start() {
        GameHandler.Instance.NewMonthEvent += GameHandler_NewMonthEvent;
    }

    private void GameHandler_NewMonthEvent(object sender, System.EventArgs e) {
        if (buildingTurnsWhenFinish > 0) {
            buildingTurnsWhenFinish -= 1;

            if (buildingTurnsWhenFinish <= 0) {
                buildingStatus = "excelent";
                SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.Columns.Status, SaveCSV.Instance.GetBuildingFilePath(), buildingStatus);
                state = States.Built;
            }
            Timer.fillAmount = GameHandler.Instance.GetTurnCount() / buildingTurnsWhenFinish;
            SaveCSV.Instance.EditOneValueOnLine(buildingid, SaveCSV.Columns.TurnsToFinish, SaveCSV.Instance.GetBuildingFilePath(), buildingTurnsWhenFinish.ToString());
        }
    }

    public void Initialize(int id, string name, string type, int year, float area, int turnsTillFinish, string status, Transform plot) {
        buildingid = id;
        buildingName = name;
        buildingType = type;
        buildingYear = year;
        buildingArea = area;
        buildingPlot = plot;
        buildingStatus = status;
        buildingTurnsWhenFinish = turnsTillFinish;

        if (buildingTurnsWhenFinish > 0) {
            Timer.fillAmount = GameHandler.Instance.GetTurnCount() / buildingTurnsWhenFinish;
        } else {
            Timer.fillAmount = 0;
        }


        if (buildingTurnsWhenFinish - GameHandler.Instance.GetTurnCount() > 0) {

        }

    }
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log(buildingid);
        Debug.Log(buildingName);
        Debug.Log(buildingType);
        Debug.Log(buildingYear);
        Debug.Log(buildingArea);
    }

    public void OnPointerDown(PointerEventData eventData) {
    }

    public void OnPointerEnter(PointerEventData eventData) {
        EventHandlerScript.Instance.SendOnFlatEnter(transform);
    }

    public void OnPointerExit(PointerEventData eventData) {
        EventHandlerScript.Instance.SendOnFlatExit(transform);
    }

    public void OnPointerUp(PointerEventData eventData) {
    }

    public void SetPlot(Transform plot) {
        buildingPlot = plot;
    }

    public int GetBuildingId() { return buildingid; }
    public string GetBuildingName() { return buildingName; }
    public string GetBuildingType() { return buildingType; }
    public int GetBuildingYear() { return buildingYear; }
    public float GetBuildingArea() { return buildingArea; }
    public string GetBuildingStatus() { return buildingStatus; }
}
