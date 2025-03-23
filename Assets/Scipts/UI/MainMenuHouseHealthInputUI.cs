using TMPro;
using UnityEngine;

public class MainMenuHouseHealthInputUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TMP_Dropdown dropdown;

    public void SetName(string name)
    {
        this.name = name;
        nameText.text = name;
    }

    public int GetSelectedDropdownItemIndex() {
        return dropdown != null ? dropdown.value : -1;
    }
    public void SetDropdownValue(int index)
    {
        if (dropdown != null && index >= 0 && index < dropdown.options.Count)
        {
            dropdown.value = index;
            dropdown.RefreshShownValue();
        }
    }
}
