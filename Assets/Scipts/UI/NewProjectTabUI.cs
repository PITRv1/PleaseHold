using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class NewProjectTabUI : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    private bool isShowing;

    private FadeControllerUI fadeControllerUI;

    private void Awake()
    {
        fadeControllerUI = GetComponent<FadeControllerUI>();

        exitButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        CameraSystem.Instance.EnableCamInputs();
        isShowing = false;
        gameObject.SetActive(false);
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
        fadeControllerUI.FadeIn(.2f);
    }
}
