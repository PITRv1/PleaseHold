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
            startDate = e.StartDate,
            houseConditions = e.HouseConditions,
            serviceCosts = e.ServiceCosts
        };
        string json = JsonUtility.ToJson(saveObject);

        File.WriteAllText(Application.dataPath + "/SaveFiles/GameParametersSaveFile.txt", json);

        string oldSaveFilePath = Application.dataPath + "/SaveFiles/NewGameParametersSaveFile.txt";
        if (File.Exists(oldSaveFilePath))
        {
            File.Delete(oldSaveFilePath);
        }
    }

    public class SaveObject
    {
        public string buildingsPath;
        public string residentsPath;
        public string servicesPath;

        public float initialBudget;
        public float startingPopulationHappiness;
        public float minPopulationHappiness;
        public int simulationLength;
        public string startDate;

        public string houseConditions;
        public string serviceCosts;
    }
}