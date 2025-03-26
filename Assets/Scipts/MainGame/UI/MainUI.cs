using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

    [SerializeField] Button newMonthButton;

    [SerializeField] private Button newServiceButton;
    [SerializeField] private Button endServiceButton;
    [SerializeField] private Button newProjectButton;

    [SerializeField] private NewServiceTab newServiceTab;
    [SerializeField] private EndServiceTab endServiceTab;
    [SerializeField] private NewProjectTab newProjectTab;

    private void Awake() {
        newMonthButton.onClick.AddListener(() => {
            GameHandler.Instance.NewMonth();
        });

        newServiceButton.onClick.AddListener(ShowNewServiceTab);
        endServiceButton.onClick.AddListener(ShowDelServiceTab);
        newProjectButton.onClick.AddListener(ShowNewProjectTab);
    }

    private void ShowNewServiceTab()
    {
        HideAll();
        newServiceTab.Show();
    }

    private void ShowDelServiceTab()
    {
        HideAll();
        endServiceTab.Show();
    }
    private void ShowNewProjectTab()
    {
        HideAll();
        newProjectTab.Show();
    }

    private void HideAll()
    {
        newServiceTab.Hide();
        endServiceTab.Hide();
        newProjectTab.Hide();
    }
}
