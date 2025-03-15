using UnityEngine;

public class GameParamSaver : GameParameters
{
    [SerializeField] private MainMenuInputFieldManager mainMenuInputFieldManager;

    private void Start()
    {
        mainMenuInputFieldManager.OnSimulationStarted += MainMenuInputFieldManager_OnSimulationStarted;
    }

    private void MainMenuInputFieldManager_OnSimulationStarted(object sender, MainMenuInputFieldManager.GameParametersEventArgs e)
    {
        GameParameters.Instance.SetGameParameters(
            e.StartingFilePath,
            e.InitialBudget,
            e.StartingPopulationHappiness,
            e.MinPopulationHappiness,
            e.SimulationLength,
            e.StartDate);
    }
}