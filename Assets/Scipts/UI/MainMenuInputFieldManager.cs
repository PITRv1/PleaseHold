using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class MainMenuInputFieldManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField yearInputField;
    [SerializeField] private TMP_InputField monthInputField;
    [SerializeField] private TMP_InputField simulationLenghtInputField;
    
    [SerializeField] private TMP_InputField minPopulationHappinessInputField;
    [SerializeField] private Slider initialPopulationHappinessSlider;
    [SerializeField] private TextMeshProUGUI sliderValueDisplayText;



    private int minHappiness = 1;
    private const int MAX_HAPPINESS = 99;

    private void Awake()
    {
        yearInputField.onEndEdit.AddListener(ValidateYear);
        monthInputField.onEndEdit.AddListener(ValidateMonth);
        simulationLenghtInputField.onEndEdit.AddListener(ValidateSimulationLenght);

        minPopulationHappinessInputField.onEndEdit.AddListener(ValidateMinHappiness);
        initialPopulationHappinessSlider.onValueChanged.AddListener(UpdateSlider);


        minPopulationHappinessInputField.text = minHappiness.ToString();
        initialPopulationHappinessSlider.minValue = minHappiness + 1;
        initialPopulationHappinessSlider.maxValue = MAX_HAPPINESS;
        initialPopulationHappinessSlider.value = initialPopulationHappinessSlider.minValue; // Ensure valid start


        UpdatePercentageText();
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
            simulationLenghtInputField.text = length.ToString();
        }
        else
        {
            simulationLenghtInputField.text = "1";
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
}

