using System;
using UnityEngine;
using UnityEngine.UI;

public class NavigationButtonsUI : MonoBehaviour
{
    public static NavigationButtonsUI Instance { get; private set; }

    public event EventHandler OnGameCanContinue;

    public event EventHandler OnPlayMenuButtonClick;
    public event EventHandler OnOptionsMenuButtonClick;
    public event EventHandler OnCreditsMenuButtonClick;

    [SerializeField] private Button simulationContinueButton;

    [SerializeField] private Button playMenuButton;
    [SerializeField] private Button optionsMenuButton;
    [SerializeField] private Button creditsMenuButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private CameraTransitionController cameraChangeController;
    [SerializeField] private FadeControllerUI canvasFadeControllerUI;


    private void Awake()
    {
        Instance = this;

        simulationContinueButton.onClick.AddListener(ContinueSimulation);

        playMenuButton.onClick.AddListener(() =>
        {
            OnPlayMenuButtonClick?.Invoke(this, EventArgs.Empty);
        });

        optionsMenuButton.onClick.AddListener(() =>
        {
            OnOptionsMenuButtonClick?.Invoke(this, EventArgs.Empty);
        });

        creditsMenuButton.onClick.AddListener(() =>
        {
            OnCreditsMenuButtonClick?.Invoke(this, EventArgs.Empty);
        });


        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
    private void ContinueSimulation()
    {
        OnGameCanContinue?.Invoke(this, EventArgs.Empty);

        cameraChangeController.Transition();
        canvasFadeControllerUI.FadeOut(2f);
    }
}
