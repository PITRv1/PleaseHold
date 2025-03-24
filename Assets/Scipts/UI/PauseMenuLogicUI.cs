using UnityEngine;
using UnityEngine.UI;

public class PauseMenuLogicUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private PauseMenuHandeler pauseMenuHandeler;

    private void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        optionsButton.onClick.AddListener(ShowHideOptionsTab);
        mainMenuButton.onClick.AddListener(exitToMainMenu);

    }

    private void ResumeGame()
    {
        pauseMenuHandeler.Hide();
    }

    private void ShowHideOptionsTab()
    {

    }

    private void exitToMainMenu()
    {
        SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
    }
}
