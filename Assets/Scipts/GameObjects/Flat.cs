using UnityEngine;

public class Flat : Buildings {

    private int buildingid;
    private string buildingName;
    private string buildingType;
    private int buildingYear;
    private float buildingArea;

    public void SetBuildingId(int newBuildingId) {
        buildingid = newBuildingId;
    }

    public int GetBuildingId() {
        return buildingid;
    }
    public void SetBuildingName(string newBuildingName) {
        buildingName = newBuildingName;
    }

    public string GetBuildingName() {
        return buildingName;
    }
    public void SetBuildingType(string newBuildingType) {
        buildingType = newBuildingType;
    }

    public string GetBuildingType() {
        return buildingType;
    }
    public void SetBuildingYear(int newBuildingYear) {
        buildingYear = newBuildingYear;
    }

    public int GetBuildingYear() {
        return buildingYear;
    }
    public void SetBuildingArea(float newBuildingArea) {
        buildingArea = newBuildingArea;
    }

    public float GetBuildingArea() {
        return buildingArea;
    }

}
