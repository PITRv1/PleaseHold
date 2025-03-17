using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class Flat : Buildings, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] Flat flat;
    private Transform buildingPlot;

    private int buildingid;
    private string buildingName;
    private string buildingType;
    private int buildingYear;
    private float buildingArea;
    private string buildingStatus;

    public void Initialize(int id, string name, string type, int year, float area, Transform plot, string status) {
        buildingid = id;
        buildingName = name;
        buildingType = type;
        buildingYear = year;
        buildingArea = area;
        buildingPlot = plot;
        buildingStatus = status;

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
