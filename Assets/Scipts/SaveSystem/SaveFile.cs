using UnityEngine;
using System.IO;
using System;
using Unity.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

public class SaveCSV : MonoBehaviour {

    //public event EventHandler ShowInputField;

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

    private List<List<string>> fileList;

    private string filePath;
    
    private void Awake()
    {
        Instance = this;
        SetFilePath(@"C:\UnityProjects\Please_Hold\Assets\InputCSVFiles\StartCSVFiles\buildings_1.csv");
        ReadFromCSV();
    }

    public List<string> ReadLinesFromCSV() {

        List<string> listLines = new List<string>();

        using (StreamReader reader = new StreamReader(filePath)) {
            string line;

            while ((line = reader.ReadLine()) != null) {
                listLines.Add(line);
            }
        }

        return listLines;
    }
    public void ReadFromCSV() {

        fileList = new List<List<string>>();

        using (StreamReader reader = new StreamReader(filePath)) {
            string line;
            bool isHeader = true;

            while ((line = reader.ReadLine()) != null) {
                if (isHeader) { // Skip Header
                    isHeader = false;
                    continue;
                }

                List<string> values = line.Split(',').ToList();

                int id = int.Parse(values[0]);
                string name = values[1];
                string type = values[2];
                int year = int.Parse(values[3]);
                double area = double.Parse(values[4]);

                Debug.Log($"ID: {id}, Name: {name}, Type: {type}, Year: {year}, Area: {area}");
                fileList.Add(values);
            }
        }
    }

    public void WriteIntoCSV(int id, Columns value, string newValue) {

        List<string> listLines = ReadLinesFromCSV();

        List<string> originalLine = listLines[id].Split(',').ToList();

        originalLine[(int)value] = newValue;

        string changedLine = string.Join(",", originalLine);

        listLines[id] = changedLine;

        File.WriteAllLines(filePath, listLines);

        ReadFromCSV();
    }

    public void WriteNewLinesIntoCSV(string[] lines) {
        List<string> listLines = ReadLinesFromCSV();

        foreach (string line in lines) {
            listLines.Add(line);
        }

        File.WriteAllLines(filePath, listLines);

        ReadFromCSV();
    }

    public void SetFilePath(string givenPath) {
        filePath = givenPath;
    }

    public List<List<string>> GetFileList() {
        return fileList;
    }


}
