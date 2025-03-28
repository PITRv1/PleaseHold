using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.Cinemachine;
using System.Collections;
using System.IO;

public class PlaySubMenu : SubMenu
{


    public event EventHandler OnSimStarted;
    public event EventHandler OnGameCanStart;

    [SerializeField] private Button simulationStartButton;
    [SerializeField] private PlaySubMenuInputManager inputManager;
    [SerializeField] private CameraTransitionController cameraChangeController;
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
            canvasFadeControllerUI.FadeOut(2f);

            string folderPath = Application.dataPath + "/OutputFiles"; // Deletes Logs

            if (Directory.Exists(folderPath)) {
                // Delete all files
                foreach (string file in Directory.GetFiles(folderPath)) {
                    File.Delete(file);
                }
            }

            string oldSaveFilePath = Application.dataPath + "/SaveFiles/NewGameParametersSaveFile.txt";
            if (File.Exists(oldSaveFilePath)) {
                File.Delete(oldSaveFilePath);
            }
        }
    }
}