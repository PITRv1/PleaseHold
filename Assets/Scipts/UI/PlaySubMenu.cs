using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.Cinemachine;
using System.Collections;

public class PlaySubMenu : SubMenu
{
    public event EventHandler OnSimStarted;
    public event EventHandler OnGameCanStart;


    [SerializeField] private Button simulationStartButton;
    [SerializeField] private PlaySubMenuInputManager inputManager;
    [SerializeField] private CameraChangeController cameraChangeController;
    [SerializeField] private FadeControllerUI canvasFadeControllerUI;

    public void Awake()
    {
        simulationStartButton.onClick.AddListener(StartSimulation);
    }


    private void StartSimulation()
    {
        OnSimStarted?.Invoke(this, EventArgs.Empty);

        if (inputManager.CanGameStart())
        {
            OnGameCanStart?.Invoke(this, EventArgs.Empty);
            cameraChangeController.Transition();
        }
    }

    private void Update()
    {
        if (inputManager.CanGameStart())
        {
            canvasFadeControllerUI.FadeOut(2f);
        }
    }
}