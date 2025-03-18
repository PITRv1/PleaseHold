using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameEventSystem : MonoBehaviour {

    private List<string> fileLines;
    private void Start() {
        GameHandler.Instance.NewMonthEvent += GameHandler_NewMonthEvent;
        fileLines = new List<string>();
    }

    private void GameHandler_NewMonthEvent(object sender, System.EventArgs e) {
        string filePath = Application.dataPath + $@"\OutputFiles\{GameHandler.Instance.GetDate()}.txt";

        fileLines.Add("");

        fileLines.Add("Épületek állapota:");

        List<string> buildings = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetBuildingFilePath());

        buildings.RemoveAt(0);

        foreach (string building in buildings) {
            string[] line = building.Split(',');

            fileLines.Add("\t-" + line[(int)SaveCSV.BuildingColumns.Name] + ": " + line[(int)SaveCSV.BuildingColumns.Status]);
        }

        fileLines.Add("");

        fileLines.Add("Budget: " + GameHandler.Instance.GetBudget().ToString());

        File.WriteAllLines(filePath, fileLines);

        fileLines = new List<string>();
    }
    public void AddToOutput(string line) {
        fileLines.Add(line);
    }
}
