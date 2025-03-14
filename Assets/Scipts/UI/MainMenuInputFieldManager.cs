using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class MainMenuInputFieldManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField yearInputField;
    [SerializeField] private TMP_InputField monthInputField;
    [SerializeField] private TMP_InputField simulationLenghtInputField;

    private void Awake()
    {
        yearInputField.onEndEdit.AddListener(ValidateYear);
        monthInputField.onEndEdit.AddListener(ValidateMonth);
        simulationLenghtInputField.onEndEdit.AddListener(ValidateSimulationLenght);
    }

    private void ValidateMonth(string input)
    {
        if (int.TryParse(input, out int month))
        {
            month = Mathf.Clamp(month, 0, 12);
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
}
