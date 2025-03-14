using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using NUnit.Framework;

public class PlaySubMenu : SubMenu
{
    [SerializeField] private Button initialInputFileButton;
    [SerializeField] private TextMeshProUGUI pathDisplayText;


    private ExtensionFilter[] extensions = new[] { 
        new ExtensionFilter("Input Data Files", "csv") 
    };

    private void Awake()
    {
        initialInputFileButton.onClick.AddListener(() =>
        {
            pathDisplayText.text = GetSelectedFilePath();
        });
    }

    private string GetSelectedFilePath()
    {
        string filePath = "";
        try
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
            if (paths[0] != null)
            {
                filePath = paths[0];
            }

            return filePath;
        } catch
        {
            print("File was not selected");
            return filePath;
        }
    }
}