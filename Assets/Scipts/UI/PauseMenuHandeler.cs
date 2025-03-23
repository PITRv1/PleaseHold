using UnityEngine;

public class PauseMenuHandeler : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    private FadeControllerUI fadeControllerUI;
    private bool isOpen = false;

    private void Awake()
    {
        fadeControllerUI = pauseMenu.GetComponent<FadeControllerUI>();
    }

    private void Start()
    {
        CameraSystem.Instance.OnPauseKeyPressed += CameraSystem_OnPauseKeyPressed;
        pauseMenu.gameObject.SetActive(false);
    }

    private void CameraSystem_OnPauseKeyPressed(object sender, System.EventArgs e)
    {
        if (isOpen)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        isOpen = true;
        //CameraSystem.Instance.DisableCamInputs();
        fadeControllerUI.FadeIn(.075f);
    }
    private void Hide()
    {
        isOpen = false;
        //CameraSystem.Instance.EnableCamInputs();
        fadeControllerUI.FadeOut(.075f);
    }
}
