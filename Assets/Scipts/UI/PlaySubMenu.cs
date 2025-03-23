using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.Cinemachine;
using System.Collections;

public class PlaySubMenu : SubMenu
{
    public event EventHandler OnGameCanContinue;

    public event EventHandler OnSimStarted;
    public event EventHandler OnGameCanStart;

    [SerializeField] private Button simulationContinueButton;
    [SerializeField] private Button simulationStartButton;
    [SerializeField] private PlaySubMenuInputManager inputManager;
    [SerializeField] private CameraChangeController cameraChangeController;
    [SerializeField] private FadeControllerUI canvasFadeControllerUI;

    public void Awake()
    {
        simulationContinueButton.onClick.AddListener(ContinueSimulation);
        simulationStartButton.onClick.AddListener(StartSimulation);
    }


    private void ContinueSimulation()
    {
        OnGameCanContinue?.Invoke(this, EventArgs.Empty);

        cameraChangeController.Transition();
        canvasFadeControllerUI.FadeOut(2f);
    }

    private void StartSimulation()
    {
        OnSimStarted?.Invoke(this, EventArgs.Empty);

        if (inputManager.CanGameStart())
        {
            OnGameCanStart?.Invoke(this, EventArgs.Empty);
            cameraChangeController.Transition();
            canvasFadeControllerUI.FadeOut(2f);
        }
    }
}