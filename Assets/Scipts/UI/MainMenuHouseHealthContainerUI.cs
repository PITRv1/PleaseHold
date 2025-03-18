using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuHouseHealthContainerUI : MonoBehaviour
{
    [SerializeField] private MainMenuHouseHealthInputUI buildingTemplate;

    private List<MainMenuHouseHealthInputUI> houseConditionArray;
    private List<string> houseConitions;
    
    private void Start()
    {

        houseConditionArray = new List<MainMenuHouseHealthInputUI>();

        List<string> buildingCSVLines = SaveCSVMainMenu.Instance.ReadLinesFromCSV(SaveCSVMainMenu.Instance.GetBuildingFilePath());

        buildingCSVLines.RemoveAt(0);

        foreach (string building in buildingCSVLines) // Corrected loop condition
        {
            string[] line = building.Split(',');
            MainMenuHouseHealthInputUI house = Instantiate(buildingTemplate, this.transform); // Corrected Instantiate()
            house.SetName(line[(int)SaveCSVMainMenu.BuildingColumns.Name]);
            houseConditionArray.Add(house);
        }
    }
    public string GetHouseContidions() {

        houseConitions = new List<string>();
        foreach (MainMenuHouseHealthInputUI house in houseConditionArray) // Corrected loop condition
        {
            houseConitions.Add(house.GetSelectedDropdownItemIndex().ToString());
        }
        string stringHouseConditionArray = string.Join(',', houseConitions);
        return stringHouseConditionArray;
    }
}