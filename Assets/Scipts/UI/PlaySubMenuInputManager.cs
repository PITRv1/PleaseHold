using SFB;
using System;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class PlaySubMenuInputManager : MonoBehaviour
{
    public event EventHandler<GameParametersEventArgs> OnSimulationStarted;
    public class GameParametersEventArgs : EventArgs
    {
        public string StartingFilePath { get; }
        public string StartDate { get; }
        public int SimulationLength { get; }
        public int MinPopulationHappiness { get; }
        public int StartingPopulationHappiness { get; }
        public int InitialBudget { get; }

        public GameParametersEventArgs(string startingFilePath, string startDate, int simulationLength, int minHappiness, int startingHappiness, int initialBudget)
        {
            StartingFilePath = startingFilePath;
            StartDate = startDate;
            SimulationLength = simulationLength;
            MinPopulationHappiness = minHappiness;
            StartingPopulationHappiness = startingHappiness;
            InitialBudget = initialBudget;
        }
    }

    private ExtensionFilter[] extensions = new[] {
        new ExtensionFilter("Input Data Files", "csv")
    };

    [SerializeField] private PlaySubMenu playSubMenu;

    [SerializeField] private Button initialInputFileButton;
    [SerializeField] private TextMeshProUGUI pathDisplayText;

    [SerializeField] private TMP_InputField yearInputField;
    [SerializeField] private TMP_InputField monthInputField;
    [SerializeField] private TMP_InputField simulationLengthInputField;
    
    [SerializeField] private TMP_InputField minPopulationHappinessInputField;
    [SerializeField] private Slider initialPopulationHappinessSlider;
    [SerializeField] private TextMeshProUGUI sliderValueDisplayText;

    [SerializeField] private TMP_InputField initialBudgetField;

    [SerializeField] private TextMeshProUGUI errorText;

    private int minHappiness = 1;
    private const int MAX_HAPPINESS = 99;
    private bool gameCanStart = false;
    private void Awake()
    {
        initialInputFileButton.onClick.AddListener(() =>
        {
            pathDisplayText.text = GetSelectedFilePath();
        });

        yearInputField.onEndEdit.AddListener(ValidateYear);
        monthInputField.onEndEdit.AddListener(ValidateMonth);
        simulationLengthInputField.onEndEdit.AddListener(ValidateSimulationLenght);

        minPopulationHappinessInputField.onEndEdit.AddListener(ValidateMinHappiness);
        initialPopulationHappinessSlider.onValueChanged.AddListener(UpdateSlider);

        initialBudgetField.onEndEdit.AddListener(ValidateInitialBudget);

        minPopulationHappinessInputField.text = minHappiness.ToString();
        initialPopulationHappinessSlider.minValue = minHappiness + 1;
        initialPopulationHappinessSlider.maxValue = MAX_HAPPINESS;
        initialPopulationHappinessSlider.value = initialPopulationHappinessSlider.minValue;

        errorText.gameObject.SetActive(false);
        UpdatePercentageText();
    }

    private void Start()
    {
        playSubMenu.OnSimStarted += PlaySubMenu_OnSimStarted;
    }

    private void PlaySubMenu_OnSimStarted(object sender, EventArgs e)
    {
        SaveSimulationParameters();
    }

    private void SaveSimulationParameters()
    {
        // Check if any input field is empty
        if (pathDisplayText.text == "PATH" || pathDisplayText.text == "" ||
            string.IsNullOrWhiteSpace(yearInputField.text) ||
            string.IsNullOrWhiteSpace(monthInputField.text) ||
            string.IsNullOrWhiteSpace(simulationLengthInputField.text) ||
            string.IsNullOrWhiteSpace(minPopulationHappinessInputField.text) ||
            string.IsNullOrWhiteSpace(initialBudgetField.text))
        {
            gameCanStart = false;
            errorText.gameObject.SetActive(true);
            return;
        }
        gameCanStart = true;

        errorText.gameObject.SetActive(false);
        // Collect values
        string startingFilePath = pathDisplayText.text;
        string startDate = $"{yearInputField.text}-{monthInputField.text}";
        int simulationLength = int.Parse(simulationLengthInputField.text);
        int minHappiness = int.Parse(minPopulationHappinessInputField.text);
        int startingHappiness = (int)initialPopulationHappinessSlider.value;
        int initialBudget = int.Parse(initialBudgetField.text);

        // Fire event with collected values
        OnSimulationStarted?.Invoke(this, new GameParametersEventArgs(startingFilePath, startDate, simulationLength, minHappiness, startingHappiness, initialBudget));
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
        }
        catch
        {
            Debug.Log("File was not selected");
            return filePath;
        }
    }

    private void ValidateMonth(string input)
    {
        if (int.TryParse(input, out int month))
        {
            month = Mathf.Clamp(month, 1, 12);
            monthInputField.text = month.ToString("D2");
        }
        else
        {
            monthInputField.text = "01";
        }
    }

    private void ValidateYear(string input)
    {
        if (int.TryParse(input, out int year))
        {
            year = Mathf.Max(year, 0);
            yearInputField.text = year.ToString("D4");
        }
        else
        {
            yearInputField.text = "0000";
        }
    }

    private void ValidateSimulationLenght(string input)
    {
        if (int.TryParse(input, out int length))
        {
            length = Mathf.Max(length, 1);
            simulationLengthInputField.text = length.ToString();
        }
        else
        {
            simulationLengthInputField.text = "1";
        }
    }

    private void ValidateMinHappiness(string input)
    {
        if (int.TryParse(input, out int value))
        {
            minHappiness = Mathf.Clamp(value, 1, MAX_HAPPINESS - 1);
            minPopulationHappinessInputField.text = minHappiness.ToString();
        }
        else
        {
            minHappiness = 1;
            minPopulationHappinessInputField.text = "1";
        }

        initialPopulationHappinessSlider.minValue = minHappiness + 1;

        if (initialPopulationHappinessSlider.value < initialPopulationHappinessSlider.minValue)
        {
            initialPopulationHappinessSlider.value = initialPopulationHappinessSlider.minValue;
        }

        UpdatePercentageText();
    }

    private void UpdateSlider(float value)
    {
        if (value < initialPopulationHappinessSlider.minValue)
        {
            initialPopulationHappinessSlider.value = initialPopulationHappinessSlider.minValue;
        }

        UpdatePercentageText();
    }

    private void UpdatePercentageText()
    {
        sliderValueDisplayText.text = $"{initialPopulationHappinessSlider.value}%";
    }

    private void ValidateInitialBudget(string input)
    {
        if (int.TryParse(input, out int length))
        {
            length = Mathf.Max(length, 1);
            initialBudgetField.text = length.ToString();
        }
        else
        {
            initialBudgetField.text = "1";
        }
    }

    public bool CanGameStart()
    {
        return gameCanStart;
    }
}