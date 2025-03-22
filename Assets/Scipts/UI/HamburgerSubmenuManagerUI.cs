using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HamburgerSubmenuManagerUI : MonoBehaviour
{
    [SerializeField] private Button newServiceButton;
    [SerializeField] private Button delServiceButton;
    //[SerializeField] private Button newProjectButton;

    [SerializeField] private ServiceField newServiceTab;
    [SerializeField] private EndServiceTabUI delServiceTab;


    private void Awake()
    {
        newServiceButton.onClick.AddListener(ShowNewServiceTab);
        delServiceButton.onClick.AddListener(ShowDelServiceTab);
        //newProjectButton.onClick.AddListener();

    }

    private void ShowNewServiceTab()
    {
        HideAll();
        newServiceTab.Show();
    }

    private void ShowDelServiceTab() {
        HideAll();
        delServiceTab.Show();
    }

    private void HideAll()
    {
        newServiceTab.Hide();
        delServiceTab.Hide();
    }
}
