using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using static EventHandlerScript;
using System.Windows.Forms;
using Button = UnityEngine.UI.Button;
using static System.Net.WebRequestMethods;
using UnityEngine.UIElements;

public class ServiceField : MonoBehaviour {

    [SerializeField] private Transform flatPrefab;
    [SerializeField] private Button serviceButton;

    private RectTransform rectTransform;

    [SerializeField] TMP_InputField serviceName;
    [SerializeField] TMP_InputField serviceType;
    [SerializeField] TMP_InputField serviceBuilding;
    [SerializeField] TMP_InputField serviceCost;

    [SerializeField] Button acceptButton;
    [SerializeField] Button exitButton;

    private string serviceNameText;
    private string serviceTypeText;
    private string serviceBuildingText;
    private string serviceCostText;

    private bool isShowing;

    private Transform givenGameObject;

    public static ServiceField Instance {
        private set;
        get;
    }

    private void Awake() {
        Instance = this;
        exitButton.onClick.AddListener(() => {
            Hide();
        });
        acceptButton.onClick.AddListener(() => {

            serviceNameText = serviceName.text;
            serviceTypeText = serviceType.text;
            serviceBuildingText = serviceBuilding.text;
            serviceCostText = serviceCost.text;

            serviceName.text = "";
            serviceType.text = "";
            serviceBuilding.text = "";
            serviceCost.text = "";

            string date = GameHandler.Instance.GetDate();
            int currentYear = Int32.Parse(date.Split('-')[0]);
            int currentMonth = Int32.Parse(date.Split('-')[1]);
            float currentDate = currentYear + currentMonth / 100;

            GameHandler.Instance.CreateNewService(
                serviceNameText,
                serviceTypeText,
                serviceBuildingText,
                serviceCostText);

            Hide();
        });
        serviceButton.onClick.AddListener(() => {
            if (isShowing == true) {
                Hide();
            } else {
                Show();
            }
        });
    }

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    private void Hide() {
        isShowing = false;
        gameObject.SetActive(false);
        serviceName.text = "";
        serviceType.text = "";
        serviceBuilding.text = "";
        serviceCost.text = "";
    }
    private void Show() {
        isShowing = true;
        gameObject.SetActive(true);
    }
}
