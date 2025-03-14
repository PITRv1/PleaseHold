using UnityEngine;
using System.IO;
using System;
using Unity.Collections;
using System.Collections.Generic;
using System.Linq;

public class TestWriteInto : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string filePath = @"D:\csvs\buildings_1.csv";

        List<List<string>> fileList = new List<List<string>>();

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
        foreach (List<string> file in fileList)
            foreach (string str in file) {
                Debug.Log(str);
            }

    }
}
