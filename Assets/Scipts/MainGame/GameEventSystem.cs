using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameEventSystem : MonoBehaviour {

    private List<string> fileLines;

    public static GameEventSystem Instance {
        private set;
        get;
    }
    private void Awake() {
        Instance = this;
    }
    private void Start() {
        GameHandler.Instance.NewMonthEvent += GameHandler_NewMonthEvent;
        fileLines = new List<string>();
    }

    private void GameHandler_NewMonthEvent(object sender, System.EventArgs e) {
        string filePath = Application.dataPath + $@"\OutputFiles\{GameHandler.Instance.GetOldDate()}.txt";

        fileLines.Add("");

        fileLines.Add("Épületek állapota:");

        List<string> buildings = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetBuildingFilePath());

        buildings.RemoveAt(0);

        foreach (string building in buildings) {
            string[] line = building.Split(',');

            fileLines.Add("\t-" + line[(int)SaveCSV.BuildingColumns.Name] + ": " + line[(int)SaveCSV.BuildingColumns.Status]);
        }

        fileLines.Add("");

        fileLines.Add("City happiness: " + (GameHandler.Instance.GetHappinessPercentige() * 100).ToString("0.00") + "%");

        fileLines.Add("");

        fileLines.Add("Budget: " + GameHandler.Instance.GetBudget().ToString());

        File.WriteAllLines(filePath, fileLines);

        fileLines = new List<string>();
    }
    public void AddToOutput(string line) {
        fileLines.Add(line);
    }

    public void EndOutput(int endReason) {
        fileLines = new List<string>();

        fileLines.Add("Vége a szimulációnak.");

        fileLines.Add("");

        switch (endReason) {
            case 1:
                fileLines.Add("Túllépte a költségvetési korlátokat.");
                break;
            case 2:
                fileLines.Add("A boldogság a szükséges minimum alá esett.");
                break;
            case 3:
                fileLines.Add("A szimulációs időszak lejárt.");
                break;
        }

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

        fileLines.Add("City happiness: " + (GameHandler.Instance.GetHappinessPercentige() * 100).ToString("0.00") + "%");

        fileLines.Add("");

        fileLines.Add("Budget: " + GameHandler.Instance.GetBudget().ToString());

        File.WriteAllLines(filePath, fileLines);

        fileLines = new List<string>();
    }
}
