using System;
using UnityEngine;

public class GameParameters : MonoBehaviour
{
    public static GameParameters Instance { get; private set; }

    private string startingFilePath;
    private float initialBudget;
    private float startingPopulationHappiness;
    private float minPopulationHappiness;
    private int simulationLength;
    private string startDate;

    private void Awake()
    {
        Instance = this;
    }

    public void SetGameParameters(string startingFilePath, float initialBudget, float startingPopulationHappiness, float minPopulationHappiness, int simulationLength, string startDate)
    {
        Instance.startingFilePath = startingFilePath;
        Instance.initialBudget = initialBudget;
        Instance.startingPopulationHappiness = startingPopulationHappiness;
        Instance.minPopulationHappiness = minPopulationHappiness;
        Instance.simulationLength = simulationLength;
        Instance.startDate = startDate;
    }
}