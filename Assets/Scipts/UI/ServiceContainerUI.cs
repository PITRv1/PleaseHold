using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;

public class ServiceContainerUI : MonoBehaviour
{
    [SerializeField] private ServiceInputUI serviceTemplate;
    [SerializeField] private Button randomizeCostButton;
    [SerializeField] private TMP_InputField rangeFromInputField;
    [SerializeField] private TMP_InputField rangeToInputField;

    private List<ServiceInputUI> serviceCostObject;
    private List<string> serviceCostList;

    private void Awake()
    {
        randomizeCostButton.onClick.AddListener(RandomizeServiceCost);
    }

    private void Start() {
        serviceCostObject = new List<ServiceInputUI>();

        List<string> servicesCSVLines = SaveCSVMainMenu.Instance.ReadLinesFromCSV(SaveCSVMainMenu.Instance.GetServiceFilePath());

        servicesCSVLines.RemoveAt(0);

        foreach (string service in servicesCSVLines) // Corrected loop condition
        {
            string[] line = service.Split(',');
            ServiceInputUI house = Instantiate(serviceTemplate, this.transform); // Corrected Instantiate()
            house.SetName(line[(int)SaveCSVMainMenu.BuildingColumns.Name]);
            serviceCostObject.Add(house);
        }
    }

    private void RandomizeServiceCost()
    {

        float from, to;

        if (!float.TryParse(rangeFromInputField.text, out from) ||
            !float.TryParse(rangeToInputField.text, out to))
        {
            return;
        }

        if (from > to) {
            return;
        }

        foreach (ServiceInputUI service in serviceCostObject)
        {
            float randomIndex = Random.Range(from, to);

            service.SetCost(randomIndex);
        }
    }

    public string GetServiceCost() {

        serviceCostList = new List<string>();

        foreach (ServiceInputUI service in serviceCostObject) // Corrected loop condition
        {
            serviceCostList.Add(service.GetInputValue());
        }
        string stringServiceCost = string.Join(',', serviceCostList);
        return stringServiceCost;
    }

    public bool AllServiceCostFilled()
    {
        foreach (ServiceInputUI service in serviceCostObject) // Corrected loop condition
        {
            if (service.GetInputValue() == "")
            {
                return false;
            }
        }
        
        return true;
    }
}
