using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SFB;

public class PlaySubMenu : SubMenu
{
    [SerializeField] private Button initialInputFileButton;
    [SerializeField] private TextMeshProUGUI pathDisplayText;


    private ExtensionFilter[] extensions = new [] { new ExtensionFilter("csv") };

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
        var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        print(paths[0]);
        if (paths[0] != null)
        {
            filePath = paths[0];
        }
        
        return filePath;
    }
}