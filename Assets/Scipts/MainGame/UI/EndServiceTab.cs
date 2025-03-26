using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndServiceTab : MonoBehaviour
{
    [SerializeField] private EndServicePrefab servicePrefab;
    [SerializeField] private GameObject serviceContainer;
    [SerializeField] private Button exitButton;


    private List<string> serviceNameList;
    private bool isShowing;

    private FadeControllerUI fadeControllerUI;


    private void Awake()
    {
        fadeControllerUI = GetComponent<FadeControllerUI>();

        exitButton.onClick.AddListener(Hide);
    }


    private void Start()
    {
        UpdateServiceContainer();

        // Set self to inactive instantly upon start
        CameraSystem.Instance.EnableCamInputs();
        isShowing = false;
        gameObject.SetActive(false);
    }

    private void UpdateServiceContainer()
    {
        foreach (Transform child in serviceContainer.transform)
        {
            Destroy(child.gameObject);
        }

        serviceNameList = SaveCSV.Instance.ReadLinesFromCSV(SaveCSV.Instance.GetServiceFilePath());

        for (int i = 1; i < serviceNameList.Count; i++)
        {
            string serviceName = serviceNameList[i];

            string[] parameters = serviceName.Split(",");

            EndServicePrefab instance = Instantiate(servicePrefab, serviceContainer.transform);


            instance.SetName(parameters[(int)SaveCSV.ServiceColumns.Name]);
            instance.SetCost(parameters[(int)SaveCSV.ServiceColumns.Cost]);
            instance.SetId(parameters[(int)SaveCSV.ServiceColumns.Id]);
        }
    }


    public void Hide()
    {
        CameraSystem.Instance.EnableCamInputs();
        isShowing = false;
        fadeControllerUI.FadeOut(.2f);
    }
    public void Show()
    {
        CameraSystem.Instance.DisableCamInputs();
        isShowing = true;
        UpdateServiceContainer();
        fadeControllerUI.FadeIn(.2f);
    }
}
