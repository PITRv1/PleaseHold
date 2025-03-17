using System.IO;
using UnityEngine;

public class GameParamSaver : MonoBehaviour
{
    [SerializeField] private PlaySubMenuInputManager mainMenuInputFieldManager;

    private void Start()
    {
        mainMenuInputFieldManager.OnSimulationStarted += MainMenuInputFieldManager_OnSimulationStarted;
    }

    private void MainMenuInputFieldManager_OnSimulationStarted(object sender, PlaySubMenuInputManager.GameParametersEventArgs e)
    {
        SaveObject saveObject = new SaveObject
        {
            buildingsPath = e.BuildingsPath,
            residentsPath = e.PeoplePath,
            servicesPath = e.ServicesPath,
            initialBudget = e.InitialBudget,
            startingPopulationHappiness = e.StartingPopulationHappiness,
            minPopulationHappiness = e.MinPopulationHappiness,
            simulationLength = e.SimulationLength,
            startDate = e.StartDate
        };
        string json = JsonUtility.ToJson(saveObject);

        File.WriteAllText(Application.dataPath + "/SaveFiles/GameParametersSaveFile.txt", json);
    }

    private class SaveObject
    {
        public string buildingsPath;
        public string residentsPath;
        public string servicesPath;

        public float initialBudget;
        public float startingPopulationHappiness;
        public float minPopulationHappiness;
        public int simulationLength;
        public string startDate;
    }
}