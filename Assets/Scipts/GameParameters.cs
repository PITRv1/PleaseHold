using System;
using UnityEngine;

public class GameParameters : MonoBehaviour
{
    public static GameParameters Instance { get; private set; }

    private string buildingsPath;
    private string peoplePath;
    private string servicesPath;

    private float initialBudget;
    private float startingPopulationHappiness;
    private float minPopulationHappiness;
    private int simulationLength;
    private string startDate;

    private void Awake()
    {
        Instance = this;
    }

    public void SetGameParameters(
        string buildingsPath,
        string peoplePath,
        string servicesPath,
        float initialBudget, float startingPopulationHappiness, float minPopulationHappiness, int simulationLength, string startDate)

    {
        Instance.buildingsPath = buildingsPath;
        Instance.peoplePath = peoplePath;
        Instance.servicesPath = servicesPath;

        Instance.initialBudget = initialBudget;
        Instance.startingPopulationHappiness = startingPopulationHappiness;
        Instance.minPopulationHappiness = minPopulationHappiness;
        Instance.simulationLength = simulationLength;
        Instance.startDate = startDate;
    }
}