using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HamburgerMenuUI : MonoBehaviour
{
    [SerializeField] private Button newServiceButton;
    [SerializeField] private Button delServiceButton;
    [SerializeField] private Button newProjectButton;
    [SerializeField] private Button hamburgerButton;

    private FadeControllerUI newServiceButtonFadeC;
    private FadeControllerUI delServiceButtonFadeC;
    private FadeControllerUI newProjectButtonFadeC;

    private bool visible;

    private void Awake()
    {
        newServiceButtonFadeC = newServiceButton.GetComponent<FadeControllerUI>();
        delServiceButtonFadeC = delServiceButton.GetComponent<FadeControllerUI>();
        newProjectButtonFadeC = newProjectButton.GetComponent<FadeControllerUI>();

        hamburgerButton.onClick.AddListener(ShowHide);
    }

    private void ShowHide()
    {
        if (!visible)
        {
            visible = true;
            newServiceButtonFadeC.FadeIn(.2f);
            delServiceButtonFadeC.FadeIn(.15f);
            newProjectButtonFadeC.FadeIn(.1f);

        }
        else
        {
            visible = false;
            newServiceButtonFadeC.FadeOut(.1f);
            delServiceButtonFadeC.FadeOut(.15f);
            newProjectButtonFadeC.FadeOut(.2f);
        }
    }
}
