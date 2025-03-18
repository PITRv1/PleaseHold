using TMPro;
using UnityEngine;

public class MainMenuHouseHealthInputUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        nameText.text = "BuildingName";
    }

    public void SetName(string name)
    {
        this.name = name;
        nameText.text = name;
    }

    public int GetSelectedDropdownItemIndex() {
        return dropdown != null ? dropdown.value : -1;
    }
}
