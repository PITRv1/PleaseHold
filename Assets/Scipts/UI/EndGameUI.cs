using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI causeText;
    [SerializeField] TextMeshProUGUI endBudgetText;
    [SerializeField] TextMeshProUGUI endHappinessText;
    [SerializeField] TextMeshProUGUI turnsSimulatedText;
    [SerializeField] TextMeshProUGUI simulationLengthText;

    [SerializeField] Button mainMenuButton;
    [SerializeField] Button exitGameButton;

    private FadeControllerUI fadeController;


    private void Awake()
    {
        fadeController = GetComponent<FadeControllerUI>();

        mainMenuButton.onClick.AddListener(ChangeToMainMenu);
        exitGameButton.onClick.AddListener(ExitGame);

        gameObject.SetActive(false);
    }

    public void SetStats(string cause, string budget, string happiness, string turn, string simlength)
    {
        causeText.text = cause;
        endBudgetText.text = budget;
        endHappinessText.text = happiness;
        turnsSimulatedText.text = turn;
        simulationLengthText.text = simlength;
    }

    public void Show()
    {
        string oldSaveFilePath = Application.dataPath + "/SaveFiles/NewGameParametersSaveFile.txt";
        if (File.Exists(oldSaveFilePath))
        {
            print(oldSaveFilePath);
            File.Delete(oldSaveFilePath);
        }

        CameraSystem.Instance.DisableCamInputs();
        fadeController.FadeIn(.5f);
    }

    private void ChangeToMainMenu()
    {
        SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
