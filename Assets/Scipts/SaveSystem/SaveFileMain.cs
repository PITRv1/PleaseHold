using UnityEngine;
using System.IO;
using System;
using Unity.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveCSVMainMenu : MonoBehaviour {

    public enum BuildingColumns {
        Id,
        Name,
        Type,
        Year,
        Size,
        TurnsToFinish,
        Turns,
        Status,
    }

    public enum ServiceColumns {
        Id,
        Name,
        Type,
        BuildingIds,
        Cost,
    }

    public static SaveCSVMainMenu Instance {
        private set;
        get;
    }

    private List<List<string>> fileServiceList;
    private List<List<string>> fileResidentList;
    private List<List<string>> fileBuildingList;
    private List<List<string>> fileProjectsList;

    private string fileResidentPath;
    private string fileBuildingPath;
    private string fileServicePath;
    private string fileProjectsPath;

    private void Awake()
    {
        Instance = this;
        SetBuildingFilePath($@"{Application.dataPath}\CSV Files\Default CSV files\buildingsCSV.csv");
        SetServiceFilePath($@"{Application.dataPath}\CSV Files\Default CSV files\servicesCSV.csv");
        ReloadAllCSV();
    }

    public List<string> ReadLinesFromCSV(string filePath) {

        List<string> listLines = new List<string>();

        using (StreamReader reader = new StreamReader(filePath)) {
            string line;

            while ((line = reader.ReadLine()) != null) {
                listLines.Add(line);
            }
        }

        return listLines;
    }
    public void ReadFromBuildingCSV() {

        fileBuildingList = new List<List<string>>();

        using (StreamReader reader = new StreamReader(fileBuildingPath)) {
            string line;
            bool isHeader = true;

            while ((line = reader.ReadLine()) != null) {
                if (isHeader) { // Skip Header
                    isHeader = false;
                    continue;
                }
                List<string> values = line.Split(',').ToList();
                fileBuildingList.Add(values);
            }
        }
    }

    public void ReloadAllCSV() {
        ReadFromBuildingCSV();
        ReadFromServiceCSV();
    }
    public void ReadFromServiceCSV() {

        fileServiceList = new List<List<string>>();

        using (StreamReader reader = new StreamReader(fileServicePath)) {
            string line;
            bool isHeader = true;

            while ((line = reader.ReadLine()) != null) {
                if (isHeader) { // Skip Header
                    isHeader = false;
                    continue;
                }
                List<string> values = line.Split(',').ToList();
                fileServiceList.Add(values);
            }
        }
    }

    public void SetBuildingFilePath(string givenPath) {
        fileBuildingPath = givenPath;
    }
    public void SetServiceFilePath(string givenPath) {
        fileServicePath = givenPath;
    }
    public string GetBuildingFilePath() {
        return fileBuildingPath;
    }
    public string GetServiceFilePath() {
        return fileServicePath;
    }


}
