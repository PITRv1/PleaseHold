using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using NUnit.Framework;
using System;

public class PlaySubMenu : SubMenu
{
    public event EventHandler OnSimStarted;


    [SerializeField] private Button simulationStartButton;
    [SerializeField] private PlaySubMenuInputManager inputManager;
    [SerializeField] private CameraChangeController cameraChangeController;
    [SerializeField] private FadeControllerUI fadeControllerUI;

    public void Awake()
    {
        simulationStartButton.onClick.AddListener(StartSimulation);
    }

    private void StartSimulation()
    {
        OnSimStarted?.Invoke(this, EventArgs.Empty);

        if (inputManager.CanGameStart())
        {
            print("time to play");
            cameraChangeController.Transition();
        }
    }

    private void Update()
    {
        if (inputManager.CanGameStart())
        {
            fadeControllerUI.FadeOut(2f);
        }
    }

}