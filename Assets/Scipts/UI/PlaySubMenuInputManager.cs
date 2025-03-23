using SFB;
using System;
using System.IO;
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
        public string BuildingsPath { get; }
        public string PeoplePath { get; }
        public string ServicesPath { get; }
        public string StartDate { get; }
        public int SimulationLength { get; }
        public int MinPopulationHappiness { get; }
        public int StartingPopulationHappiness { get; }
        public int InitialBudget { get; }
        public string HouseConditions { get; }
        public string ServiceCosts { get; }

        public GameParametersEventArgs(string buildingsPath, string peoplePath, string servicesPath, string startDate, int simulationLength, int minHappiness, int startingHappiness, int initialBudget, string houseCondition, string serviceCosts)
        {
            BuildingsPath = buildingsPath;
            PeoplePath = peoplePath;
            ServicesPath = servicesPath;
            StartDate = startDate;
            SimulationLength = simulationLength;
            MinPopulationHappiness = minHappiness;
            StartingPopulationHappiness = startingHappiness;
            InitialBudget = initialBudget;
            HouseConditions = houseCondition;
            ServiceCosts = serviceCosts;
        }
    }

    private ExtensionFilter[] extensions = new[] {
        new ExtensionFilter("Input Data Files", "csv")
    };

    [SerializeField] private PlaySubMenu playSubMenu;

    [SerializeField] private Button inputDefaultButton;

    [SerializeField] private Button inputBuildingsButton;
    [SerializeField] private Button inputPeopleButton;
    [SerializeField] private Button inputServicesButton;

    [SerializeField] private TMP_InputField yearInputField;
    [SerializeField] private TMP_InputField monthInputField;
    [SerializeField] private TMP_InputField simulationLengthInputField;
    
    [SerializeField] private TMP_InputField minPopulationHappinessInputField;
    [SerializeField] private Slider initialPopulationHappinessSlider;
    [SerializeField] private TextMeshProUGUI sliderValueDisplayText;

    [SerializeField] private TMP_InputField initialBudgetField;

    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private ServiceContainerUI serviceContainerUI;
    [SerializeField] private MainMenuHouseHealthContainerUI mainMenuHouseHealthContainerUI;

    private TMP_Text inputBuildingsText;
    private TMP_Text inputPeopleText;
    private TMP_Text inputServicesText;



    private int minHappiness = 1;
    private const int MAX_HAPPINESS = 99;
    private bool gameCanStart = false;
    private void Awake()
    {

        inputBuildingsText = inputBuildingsButton.GetComponentInChildren<TMP_Text>();
        inputPeopleText = inputPeopleButton.GetComponentInChildren<TMP_Text>();
        inputServicesText = inputServicesButton.GetComponentInChildren<TMP_Text>();



        inputDefaultButton.onClick.AddListener(SetDefaultFilePath);

        inputBuildingsButton.onClick.AddListener(OpenBuildingsFilePath);
        inputPeopleButton.onClick.AddListener(OpenPeopleFilePath);
        inputServicesButton.onClick.AddListener(OpenServicesFilePath);


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


    //Only have 3 version cause I cant pass arguments into eventlisteners
    private void OpenBuildingsFilePath()
    {
        inputBuildingsText.text = GetSelectedFilePath();
    }
    private void OpenPeopleFilePath()
    {
        inputPeopleText.text = GetSelectedFilePath();
    }
    private void OpenServicesFilePath()
    {
        inputServicesText.text = GetSelectedFilePath();
    }


    private void SaveSimulationParameters()
    {
        // Check if any input field is empty
        if (inputBuildingsText.text == "PATH" || inputBuildingsText.text == "" ||
            inputPeopleText.text == "PATH" || inputPeopleText.text == "" ||
            inputServicesText.text == "PATH" || inputServicesText.text == "" ||

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
        string buildingsPath = inputBuildingsText.text;
        string peoplePath = inputPeopleText.text;
        string servicesPath = inputServicesText.text;

        string startDate = $"{yearInputField.text}-{monthInputField.text}";
        int simulationLength = int.Parse(simulationLengthInputField.text);
        int minHappiness = int.Parse(minPopulationHappinessInputField.text);
        int startingHappiness = (int)initialPopulationHappinessSlider.value;
        int initialBudget = int.Parse(initialBudgetField.text);

        string serviceCosts = serviceContainerUI.GetServiceCost();
        string houseConditions = mainMenuHouseHealthContainerUI.GetHouseContidions();

        // Fire event with collected values
        OnSimulationStarted?.Invoke(this, new GameParametersEventArgs(buildingsPath, peoplePath, servicesPath, startDate, simulationLength, minHappiness, startingHappiness, initialBudget, houseConditions, serviceCosts));
    }


    private void SetDefaultFilePath()
    {
        inputBuildingsText.text = Application.dataPath + "/InputCSVFiles/SaveCSVFiles/buildingsCSV.csv";
        inputPeopleText.text = Application.dataPath + "/InputCSVFiles/SaveCSVFiles/residentsCSV.csv";
        inputServicesText.text = Application.dataPath + "/InputCSVFiles/SaveCSVFiles/servicesCSV.csv";
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

    public bool CanGameStart()
    {
        return gameCanStart;
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

}