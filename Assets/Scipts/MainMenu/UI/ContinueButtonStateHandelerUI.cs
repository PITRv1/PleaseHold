using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonStateHandelerUI : MonoBehaviour
{
    [SerializeField] Button continueButton;

    private string saveFilePath;

    private void Start()
    {
        saveFilePath = Application.dataPath + "/SaveFiles/NewGameParametersSaveFile.txt";


        if (!File.Exists(saveFilePath))
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
        }
    }
}
