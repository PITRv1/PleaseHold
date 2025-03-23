using UnityEngine;
using System.IO;
using System;
using Unity.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using static GameParamSaver;

public class SaveCSV : MonoBehaviour {
    public enum FileTypes {
        Building,
        Service,
        Resident,
    }

    public enum BuildingColumns {
        Id,
        Name,
        Type,
        Year,
        Size,
        TurnsToFinish,
        Turns,
        Status,
        Plot,
    }

    public enum ServiceColumns {
        Id,
        Name,
        Type,
        BuildingIds,
        Cost,
    }

    public enum ProjectColumns {
        Id,
        Name,
        Cost,
        StartDate,
        EndDate,
        BuildingId,
        Type,
    }

    public static SaveCSV Instance {
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

    private string initialBudget;
    private string startingPopulationHappiness;
    private string minPopulationHappiness;
    private string simulationLength;
    private string startDate;
    private string houseConditions;
    private string serviceCosts;

    private void Awake()
    {
        Instance = this;

        string buildPath = "";
        string residentPath = "";
        string servicePath = "";

        bool isNew = false;

        string filePath = Application.dataPath + "/SaveFiles/NewGameParametersSaveFile.txt";
        if (File.Exists(filePath)) {
            //You need to run setup in main menu or manually change the path in the save file
            //since this now reads out of the SaveFile

            string json = File.ReadAllText(filePath);
            GameHandler.SaveObject saveObject = JsonUtility.FromJson<GameHandler.SaveObject>(json);

            buildPath = saveObject.buildingsPath;
            residentPath = saveObject.residentsPath;
            servicePath = saveObject.servicesPath;

            initialBudget = saveObject.budget.ToString();
            startingPopulationHappiness = saveObject.populationHappiness.ToString();
            minPopulationHappiness = saveObject.minPopulationHappiness.ToString();
            simulationLength = saveObject.simulationLength.ToString();
            startDate = saveObject.date;
            houseConditions = null;
            serviceCosts = null;
        } else {
            filePath = Application.dataPath + "/SaveFiles/GameParametersSaveFile.txt";
            if (File.Exists(filePath)) {
                //You need to run setup in main menu or manually change the path in the save file
                //since this now reads out of the SaveFile

                isNew = true;

                string json = File.ReadAllText(filePath);
                GameParamSaver.SaveObject saveObject = JsonUtility.FromJson<GameParamSaver.SaveObject>(json);

                buildPath = saveObject.buildingsPath;
                residentPath = saveObject.residentsPath;
                servicePath = saveObject.servicesPath;

                initialBudget = saveObject.initialBudget.ToString();
                startingPopulationHappiness = saveObject.startingPopulationHappiness.ToString();
                minPopulationHappiness = saveObject.minPopulationHappiness.ToString();
                simulationLength = saveObject.simulationLength.ToString();
                startDate = saveObject.startDate;
                houseConditions = saveObject.houseConditions;
                serviceCosts = saveObject.serviceCosts;
            } else {
                Debug.LogError("Save file not found!");
            }
        }

        if (!Directory.EnumerateFileSystemEntries($@"{Application.dataPath}\InputCSVFiles\StartCSVFiles").Any() || isNew == true) {

            filePath = Application.dataPath + "/SaveFiles/GameParametersSaveFile.txt";
            if (File.Exists(filePath)) {
                //You need to run setup in main menu or manually change the path in the save file
                //since this now reads out of the SaveFile

                isNew = true;

                string json = File.ReadAllText(filePath);
                GameParamSaver.SaveObject saveObject = JsonUtility.FromJson<GameParamSaver.SaveObject>(json);

                buildPath = saveObject.buildingsPath;
                residentPath = saveObject.residentsPath;
                servicePath = saveObject.servicesPath;

                houseConditions = saveObject.houseConditions;
                serviceCosts = saveObject.serviceCosts;
            } else {
                Debug.LogError("Save file not found!");
            }

            MakeEditableCSVs(residentPath, buildPath, servicePath, Application.dataPath + "/InputCSVFiles/SaveCSVFiles/projectsCSV.csv");

            SetResidentFilePath($@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\residentsSaveCSV.csv");
            SetBuildingFilePath($@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\buildingsSaveCSV.csv");
            SetServiceFilePath($@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\servicesSaveCSV.csv");
            SetProjectsFilePath($@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\projectsSaveCSV.csv");

            AddValueToLine(0, "turns to finish", fileBuildingPath);
            AddValueToLine(0, "turns", fileBuildingPath);
            AddValueToLine(0, "status", fileBuildingPath);
            AddValueToLine(0, "plot", fileBuildingPath);
            AddValueToLine(0, "cost", fileServicePath);
            AddValueToLine(0, "type", fileProjectsPath);

            AddTurnsToFinish(fileBuildingPath);
            AddTurnsToFinish(fileBuildingPath);

            if (houseConditions != null && serviceCosts != null) {
                string[] houseConditionsArray = houseConditions.Split(',');
                string[] serviceCostsArray = serviceCosts.Split(',');

                for (int i = 0; i < houseConditionsArray.Length; i++) {
                    switch (houseConditionsArray[i]) {
                        case "0":
                            AddValueToLine(i + 1, "Awful", fileBuildingPath);
                            break;
                        case "1":
                            AddValueToLine(i + 1, "Bad", fileBuildingPath);
                            break;
                        case "2":
                            AddValueToLine(i + 1, "Average", fileBuildingPath);
                            break;
                        case "3":
                            AddValueToLine(i + 1, "Good", fileBuildingPath);
                            break;
                        case "4":
                            AddValueToLine(i + 1, "Perfect", fileBuildingPath);
                            break;
                    }

                }
                for (int i = 0; i < serviceCostsArray.Length; i++) {
                    AddValueToLine(i + 1, serviceCostsArray[i], fileServicePath);
                }
            }

            AddHolderPlot(fileBuildingPath);

            int sYear = Int32.Parse(startDate.Split('-')[0]);

            foreach (string line in ReadLinesFromCSV(fileBuildingPath)) {
                if (sYear < line[(int)BuildingColumns.Year]) Debug.LogError("One of the building is built in the future");
            }

        } else {

            SetResidentFilePath($@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\residentsSaveCSV.csv");
            SetBuildingFilePath($@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\buildingsSaveCSV.csv");
            SetServiceFilePath($@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\servicesSaveCSV.csv");
            SetProjectsFilePath($@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\projectsSaveCSV.csv");

        }

        ReloadAllCSV();

    }

    public string GetInitialBudget() {
        return initialBudget;
    }
    public string GetStartingPopulationHappiness() {
        return startingPopulationHappiness;
    }
    public string GetMinPopulationHappiness() {
        return minPopulationHappiness;
    }
    public string GetSimulationLength() {
        return simulationLength;
    }
    public string GetStartDate() {
        return startDate;
    }

    private void AddTurnsToFinish(string filePath) {

        int fileLength = GetCSVLength(filePath);

        for (int i = 0; i < fileLength; i++) {
            if (i == 0) continue;
            AddValueToLine(i, "0", filePath);
        }

        ReloadAllCSV();
    }

    private void AddHolderPlot(string filePath) {

        int fileLength = GetCSVLength(filePath);

        for (int i = 0; i < fileLength; i++) {
            if (i == 0) continue;
            AddValueToLine(i, "-1", filePath);
        }

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

    public int GetCSVLength(string filePath) {

        List<string> listLines = ReadLinesFromCSV(filePath);

        return listLines.Count();
    }

    public List<string> GetSettingsFromJson() {
        List<string> returnList = new List<string>();

        return returnList;
    }
    
    private void MakeEditableCSVs(string resPath, string buildPath, string servPath, string projPath) {

        string residentPath = $@"{resPath}";
        string buildingPath = $@"{buildPath}";
        string servicePath = $@"{servPath}";
        string projectsPath = $@"{projPath}";


        File.Copy(residentPath, $@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\residentsSaveCSV.csv", true);
        File.Copy(buildingPath, $@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\buildingsSaveCSV.csv", true);
        File.Copy(servicePath, $@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\servicesSaveCSV.csv", true);
        File.Copy(projectsPath, $@"{Application.dataPath}\InputCSVFiles\StartCSVFiles\projectsSaveCSV.csv", true);
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

        for (int i = 0; i < listLines.Count; i++) {

            string[] line = listLines[i].Split(',');
            
            if (line[0] == id.ToString()) {  // 0 is always the id, just to generalize

                listLines.RemoveAt(i);

            }
        }

        File.WriteAllLines(filePath, listLines);
        ReloadAllCSV();

    }

    public void UpdateIds(string filePath) {

        List<string> listLines = ReadLinesFromCSV(filePath);

        for (int i = 0; i < listLines.Count; i++) {

            if (i == 0) continue; // If it is the header, skip

            List<string> splitLine = listLines[i].Split(',').ToList();


            splitLine[(int)BuildingColumns.Id] = i.ToString();

            string changedLine = string.Join(",", splitLine);

            listLines[i] = changedLine;

        }

        File.WriteAllLines(filePath, listLines);
        ReloadAllCSV();
    }

    public void EditOneValueOnLine(int id, BuildingColumns selectValue, string filePath, string newValue) {

        List<string> listLines = ReadLinesFromCSV(filePath);
        List<string> splitLine = listLines[id].Split(',').ToList();

        splitLine[(int)selectValue] = newValue;
        string changedLine = string.Join(",", splitLine);

        listLines[id] = changedLine;
        File.WriteAllLines(filePath, listLines);
        ReloadAllCSV();
    }

    public void EditAllValuesOnLine(int id, string newLine, string filePath) {
        List<string> listLines = ReadLinesFromCSV(filePath);
        listLines[id] = newLine;
        File.WriteAllLines(filePath, listLines);
        ReloadAllCSV();
    }

    public void AddValueToLine(int id, string value, string filePath) {
        // Just add the value without the ',', ok?
        List<string> listLines = ReadLinesFromCSV(filePath);
        listLines[id] += ","+value;
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

    public void WriteNewLineIntoCSV(string filePath, string line) {
        List<string> listLines = ReadLinesFromCSV(filePath);
        listLines.Add(line);

        File.WriteAllLines(filePath, listLines);

        ReloadAllCSV();
    }

    //Don't delete, this for checking if csv is good

    //string date = GameHandler.Instance.GetDate();
    //int currentYear = Int32.Parse(date.Split('-')[0]);
    //int currentMonth = Int32.Parse(date.Split('-')[1]);
    //float currentDate = currentYear + currentMonth / 100;

    //if (currentDate)

    public void SetBuildingFilePath(string givenPath) {
        fileBuildingPath = givenPath;
    }
    public void SetResidentFilePath(string givenPath) {
        fileResidentPath = givenPath;
    }
    public void SetServiceFilePath(string givenPath) {
        fileServicePath = givenPath;
    }
    public void SetProjectsFilePath(string givenPath) {
        fileProjectsPath = givenPath;
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
    public List<List<string>> GetProjectFileList() {
        return fileProjectsList;
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
    public string GetProjectsFilePath() {
        return fileProjectsPath;
    }


}
