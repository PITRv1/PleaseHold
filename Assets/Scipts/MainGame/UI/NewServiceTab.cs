using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using static EventHandlerScript;
using System.Windows.Forms;
using Button = UnityEngine.UI.Button;
using static System.Net.WebRequestMethods;
using UnityEngine.UIElements;

public class NewServiceTab : MonoBehaviour {

    [SerializeField] private Transform flatPrefab;

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
    private FadeControllerUI fadeControllerUI;

    public static NewServiceTab Instance {
        private set;
        get;
    }

    private void Awake() {
        fadeControllerUI = GetComponent<FadeControllerUI>();

        Instance = this;
    }

    private void Start() {
        rectTransform = GetComponent<RectTransform>();

        // Set self to inactive instantly upon start
        CameraSystem.Instance.EnableCamInputs();
        isShowing = false;
        gameObject.SetActive(false);

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
    }

    public void Hide() {
        isShowing = false;
        SaveCSV.Instance.UpdateIds(SaveCSV.Instance.GetServiceFilePath());
        CameraSystem.Instance.EnableCamInputs();
        fadeControllerUI.FadeOut(.2f);

        serviceName.text = "";
        serviceType.text = "";
        serviceBuilding.text = "";
        serviceCost.text = "";

    }
    public void Show() {
        isShowing = true;
        CameraSystem.Instance.DisableCamInputs();
        fadeControllerUI.FadeIn(.2f);
    }
}
