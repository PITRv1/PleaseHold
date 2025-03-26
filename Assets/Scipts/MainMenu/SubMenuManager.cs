using System.Collections.Generic;
using UnityEngine;

public class SubMenuManager : MonoBehaviour
{
    [SerializeField] private PlaySubMenu playSubMenu;
    [SerializeField] private OptionsSubMenu optionsSubMenu;
    [SerializeField] private CreditsSubMenu creditsSubMenu;

    private FadeControllerUI playSubMenuFadeController;
    private FadeControllerUI optionsSubMenuFadeController;
    private FadeControllerUI creditsSubMenuFadeController;


    private void Awake()
    {
        playSubMenuFadeController = playSubMenu.GetComponent<FadeControllerUI>();
        optionsSubMenuFadeController = optionsSubMenu.GetComponent<FadeControllerUI>();
        creditsSubMenuFadeController = creditsSubMenu.GetComponent<FadeControllerUI>();

        NavigationButtonsUI.Instance.OnPlayMenuButtonClick += MainButtonUI_OnPlayMenuButtonClick;
        NavigationButtonsUI.Instance.OnOptionsMenuButtonClick += MainButtonUI_OnOptionsMenuButtonClick;
        NavigationButtonsUI.Instance.OnCreditsMenuButtonClick += MainButtonUI_OnCreditsMenuButtonClick;

        
    }

    private void Start()
    {
        playSubMenu.gameObject.SetActive(false);
        optionsSubMenu.gameObject.SetActive(false);
        creditsSubMenu.gameObject.SetActive(false);
    }

    private void MainButtonUI_OnCreditsMenuButtonClick(object sender, System.EventArgs e)
    {
        HideAllSubMenus();
        creditsSubMenuFadeController.FadeIn(.3f);
    }

    private void MainButtonUI_OnOptionsMenuButtonClick(object sender, System.EventArgs e)
    {
        HideAllSubMenus();
        optionsSubMenuFadeController.FadeIn(.3f);
    }

    private void MainButtonUI_OnPlayMenuButtonClick(object sender, System.EventArgs e)
    {
        HideAllSubMenus();
        playSubMenuFadeController.FadeIn(.3f);
    }

    private void HideAllSubMenus()
    {
        playSubMenuFadeController.FadeOut(.2f);
        creditsSubMenuFadeController.FadeOut(.2f);
        optionsSubMenuFadeController.FadeOut(.2f);
    }
}
