using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndServicePrefabUI : MonoBehaviour
{
    public event EventHandler OnServiceDeleted;

    public class DeleteEventArgs : EventArgs
    {
        public string ServiceName;
        public string Cost;
        public string Id;

        public DeleteEventArgs(string serviceName, string serviceCost, string id)
        {
            ServiceName = serviceName;
            Cost = serviceCost;
            Id = id;
        }
    }


    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button delButton;

    private string id;

    private void Awake()
    {
        delButton.onClick.AddListener(DeleteSelf);
    }

    public void SetName(string name) { nameText.text = name;}
    public void SetCost(string cost) { costText.text = cost; }
    public void SetId(string id) { this.id = id; }



    private void DeleteSelf()
    {
        OnServiceDeleted?.Invoke(this, new DeleteEventArgs(
            nameText.text,
            costText.text,
            id
            ));
        Destroy(this.gameObject);
    }
}
