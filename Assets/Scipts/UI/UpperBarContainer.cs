using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpperBarContainer : MonoBehaviour {

    [SerializeField] TextMeshProUGUI DateText;
    [SerializeField] TextMeshProUGUI HappinessText;
    [SerializeField] Image HappinessDisplay;
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
    public void ChangeHappiness(float newText) {
        if (newText > 0) {
            HappinessText.text = (newText * 100).ToString("0.00") + "%";
            HappinessDisplay.fillAmount = newText;
        } else {
            HappinessText.text = "0%";
            HappinessDisplay.fillAmount = 0.0f;
        }
        
    }
    public void ChangeBudget(string newText) {
        BudgetText.text = newText;
    }
}
