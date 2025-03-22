using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndServiceTabUI : MonoBehaviour
{
    [SerializeField] private EndServicePrefabUI servicePrefab;
    [SerializeField] private GameObject serviceContainer;
    [SerializeField] private Button exitButton;


    private List<string> serviceNameList;
    private bool isShowing;

    private FadeControllerUI fadeControllerUI;


    private void Awake()
    {
        fadeControllerUI = GetComponent<FadeControllerUI>();

        exitButton.onClick.AddListener(() => {
            if (isShowing == true)
            {
                Hide();
            }
            else
            {
                Show();
            }
        });
    }


    private void Start()
    {
        UpdateServiceContainer();
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

            string[] parameters = serviceName.Split(";");

            EndServicePrefabUI instance = Instantiate(servicePrefab, serviceContainer.transform);


            //instance.SetName(parameters[0]);
            //instance.SetName(parameters[(int)SaveCSV.ServiceColumns.Name]);
            //instance.SetCost(parameters[(int)SaveCSV.ServiceColumns.Cost]);
        }
    }


    private void Hide()
    {
        CameraSystem.Instance.EnableCamInputs();
        isShowing = false;
        fadeControllerUI.FadeOut(.2f);
    }
    private void Show()
    {
        CameraSystem.Instance.DisableCamInputs();
        isShowing = true;
        UpdateServiceContainer();
        fadeControllerUI.FadeIn(.2f);
    }
}
