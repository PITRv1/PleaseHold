using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndServicePrefabUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button delButton;

    private void Awake()
    {
        delButton.onClick.AddListener(DeleteSelf);
    }

    public void SetName(string name) { nameText.text = name;}
    public void SetCost(string cost) { costText.text = cost; }


    private void DeleteSelf()
    {

    }
}
