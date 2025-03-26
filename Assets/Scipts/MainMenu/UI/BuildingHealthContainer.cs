using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuildingHealthContainer : MonoBehaviour
{
    [SerializeField] private BuildingPrefabHealthInput buildingTemplate;
    [SerializeField] private Button randomizeHouseButton;

    private List<BuildingPrefabHealthInput> houseConditionArray;
    private List<string> houseConitions;

    private void Awake()
    {
        randomizeHouseButton.onClick.AddListener(RandomizeHouseIntegrity);
    }

    private void Start()
    {

        houseConditionArray = new List<BuildingPrefabHealthInput>();

        List<string> buildingCSVLines = SaveCSVMainMenu.Instance.ReadLinesFromCSV(SaveCSVMainMenu.Instance.GetBuildingFilePath());

        buildingCSVLines.RemoveAt(0);

        foreach (string building in buildingCSVLines) // Corrected loop condition
        {
            string[] line = building.Split(',');
            BuildingPrefabHealthInput house = Instantiate(buildingTemplate, this.transform); // Corrected Instantiate()
            house.SetName(line[(int)SaveCSVMainMenu.BuildingColumns.Name]);
            houseConditionArray.Add(house);
        }
    }

    private void RandomizeHouseIntegrity()
    {
        foreach (BuildingPrefabHealthInput house in houseConditionArray)
        {
            int randomIndex = Random.Range(0, 5);
            house.SetDropdownValue(randomIndex);
        }
    }

    public string GetHouseContidions() {

        houseConitions = new List<string>();
        foreach (BuildingPrefabHealthInput house in houseConditionArray) // Corrected loop condition
        {
            houseConitions.Add(house.GetSelectedDropdownItemIndex().ToString());
        }
        string stringHouseConditionArray = string.Join(',', houseConitions);
        return stringHouseConditionArray;
    }
}