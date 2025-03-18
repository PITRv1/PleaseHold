using TMPro;
using UnityEngine;

public class UpperBarContainer : MonoBehaviour {

    [SerializeField] TextMeshProUGUI DateText;
    [SerializeField] TextMeshProUGUI HappinessText;
    [SerializeField] TextMeshProUGUI BudgetText;

    public static UpperBarContainer Instance {
        private set;
        get;
    }

    private void Awake() {
        Instance = this;
    }
    public void ChangeDate(string newText) {
        DateText.text = newText;
    }
    public void ChangeHappiness(string newText) {
        HappinessText.text = newText;
    }
    public void ChangeBudget(string newText) {
        BudgetText.text = newText;
    }
}
