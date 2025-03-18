using UnityEngine;

public class MainMenuHouseHealthContainerUI : MonoBehaviour
{
    [SerializeField] private MainMenuHouseHealthInputUI buildingTemplate;
    
    private void Start()
    {
        for (int i = 0; i < 5; i++) // Corrected loop condition
        {
            Instantiate(buildingTemplate, this.transform); // Corrected Instantiate()
        }
    }
}