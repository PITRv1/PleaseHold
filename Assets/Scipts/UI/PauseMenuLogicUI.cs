using UnityEngine;
using UnityEngine.UI;

public class PauseMenuLogicUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button mainMenuButton;

    [SerializeField] private GameObject optionsTab;

    [SerializeField] private PauseMenuHandeler pauseMenuHandeler;

    private FadeControllerUI optionsTabFadeController;
    private bool optionsTabShowing = false;

    private void Awake()
    {
        optionsTabFadeController = optionsTab.GetComponent<FadeControllerUI>();
    }

    private void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        optionsButton.onClick.AddListener(ShowHideOptionsTab);
        mainMenuButton.onClick.AddListener(exitToMainMenu);

        optionsTab.gameObject.SetActive(false);
    }

    private void ResumeGame()
    {
        pauseMenuHandeler.Hide();
    }

    private void ShowHideOptionsTab()
    {
        if (!optionsTabShowing)
        {
            optionsTabShowing = true;
            optionsTabFadeController.FadeIn(.1f);
        }
        else {
            optionsTabShowing = false;
            optionsTabFadeController.FadeOut(.1f);
        }
    }

    private void exitToMainMenu()
    {
        SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
    }
}
