using UnityEngine;

public class GameParamSaver : GameParameters
{
    [SerializeField] private PlaySubMenuInputManager mainMenuInputFieldManager;

    private void Start()
    {
        mainMenuInputFieldManager.OnSimulationStarted += MainMenuInputFieldManager_OnSimulationStarted;
    }

    private void MainMenuInputFieldManager_OnSimulationStarted(object sender, PlaySubMenuInputManager.GameParametersEventArgs e)
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