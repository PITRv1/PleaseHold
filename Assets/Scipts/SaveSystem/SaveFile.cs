using UnityEngine;
using System.IO;
using System;
using Unity.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveCSV : MonoBehaviour {

    //public event EventHandler ShowInputField;

    public enum FileTypes {
        Building,
        Service,
        Resident,
    }

    public enum Columns {
        Id,
        Name,
        Type,
        Year,
        Size,
    }

    public static SaveCSV Instance {
        private set;
        get;
    }

    private List<List<string>> fileServiceList;
    private List<List<string>> fileResidentList;
    private List<List<string>> fileBuildingList;

    private string fileResidentPath;
    private string fileBuildingPath;
    private string fileServicePath;

    private void Awake()
    {
        Instance = this;
        SetResidentFilePath(@"D:\csvs\residentsCSV.csv");
        SetBuildingFilePath(@"D:\csvs\buildingsCSV.csv");
        SetServiceFilePath(@"D:\csvs\servicesCSV.csv");
        ReloadAllCSV();
        DeleteFromCSV(fileResidentPath, 3);
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

    public List<string> GetSettingsFromJson() {
        List<string> returnList = new List<string>();



        return returnList;
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
        ReadFromResidentCSV();
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
    public void ReadFromResidentCSV() {

        fileResidentList = new List<List<string>>();

        using (StreamReader reader = new StreamReader(fileResidentPath)) {
            string line;
            bool isHeader = true;

            while ((line = reader.ReadLine()) != null) {
                if (isHeader) { // Skip Header
                    isHeader = false;
                    continue;
                }
                List<string> values = line.Split(',').ToList();
                fileResidentList.Add(values);
            }
        }
    }

    public void DeleteFromCSV(string filePath, int id) {

        List<string> listLines = ReadLinesFromCSV(filePath);

        listLines.RemoveAt(id);

        Debug.Log(listLines);

        for (int i = 0; i < listLines.Count; i++) {

            if (i == 0) continue; // If it is the header, skip

            List<string> splitLine = listLines[i].Split(',').ToList();

            Debug.Log(splitLine);

            splitLine[(int)Columns.Id] = i.ToString();

            Debug.Log(splitLine[(int)Columns.Id]);

            string changedLine = string.Join(",", splitLine);

            listLines[i] = changedLine;

        }

        File.WriteAllLines(filePath, listLines);
        ReloadAllCSV();

    }

    public void WriteIntoCSV(string filePath, int id, Columns value, string newValue) {

        List<string> listLines = ReadLinesFromCSV(filePath);

        List<string> originalLine = listLines[id].Split(',').ToList();

        originalLine[(int)value] = newValue;

        string changedLine = string.Join(",", originalLine);

        listLines[id] = changedLine;

        File.WriteAllLines(filePath, listLines);

        ReloadAllCSV();
    }

    public void WriteNewLinesIntoCSV(string filePath, string[] lines) {
        List<string> listLines = ReadLinesFromCSV(filePath);

        foreach (string line in lines) {
            listLines.Add(line);
        }

        File.WriteAllLines(filePath, listLines);

        ReloadAllCSV();
    }

    public void SetBuildingFilePath(string givenPath) {
        fileBuildingPath = givenPath;
    }
    public void SetResidentFilePath(string givenPath) {
        fileResidentPath = givenPath;
    }
    public void SetServiceFilePath(string givenPath) {
        fileServicePath = givenPath;
    }

    public List<List<string>> GetBuildingFileList() {
        return fileBuildingList;
    }
    public List<List<string>> GetResidentFileList() {
        return fileResidentList;
    }
    public List<List<string>> GetServiceFileList() {
        return fileServiceList;
    }

    public string GetBuildingFilePath() {
        return fileBuildingPath;
    }
    public string GetResidentFilePath() {
        return fileResidentPath;
    }
    public string GetServiceFilePath() {
        return fileServicePath;
    }


}
