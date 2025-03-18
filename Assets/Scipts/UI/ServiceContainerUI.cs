using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServiceContainerUI : MonoBehaviour
{
    [SerializeField] private ServiceInputUI serviceTemplate;

    private List<ServiceInputUI> serviceCostObject;
    private List<string> serviceCostList;

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

    public string GetServiceCost() {

        serviceCostList = new List<string>();

        foreach (ServiceInputUI service in serviceCostObject) // Corrected loop condition
        {
            serviceCostList.Add(service.GetInputValue());
        }
        string stringServiceCost = string.Join(',', serviceCostList);
        return stringServiceCost;
    }
}
