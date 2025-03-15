using System.Collections.Generic;
using UnityEngine;

public class SubMenuManagerUI : MonoBehaviour
{
    [SerializeField] private PlaySubMenu playSubMenu;
    [SerializeField] private OptionsSubMenu optionsSubMenu;
    [SerializeField] private CreditsSubMenu creditsSubMenu;

    private void Awake()
    {
        MainButtonUI.Instance.OnPlayMenuButtonClick += MainButtonUI_OnPlayMenuButtonClick;
        MainButtonUI.Instance.OnOptionsMenuButtonClick += MainButtonUI_OnOptionsMenuButtonClick;
        MainButtonUI.Instance.OnCreditsMenuButtonClick += MainButtonUI_OnCreditsMenuButtonClick;

        HideAllSubMenus();
    }

    private void MainButtonUI_OnCreditsMenuButtonClick(object sender, System.EventArgs e)
    {
        HideAllSubMenus();
        creditsSubMenu.gameObject.SetActive(true);
    }

    private void MainButtonUI_OnOptionsMenuButtonClick(object sender, System.EventArgs e)
    {
        HideAllSubMenus();
        optionsSubMenu.gameObject.SetActive(true);
    }

    private void MainButtonUI_OnPlayMenuButtonClick(object sender, System.EventArgs e)
    {
        HideAllSubMenus();
        playSubMenu.gameObject.SetActive(true);
    }

    private void HideAllSubMenus()
    {
        playSubMenu.gameObject.SetActive(false);
        optionsSubMenu.gameObject.SetActive(false);
        creditsSubMenu.gameObject.SetActive(false);
    }
}
