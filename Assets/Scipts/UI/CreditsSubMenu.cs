using UnityEngine;
using UnityEngine.UI;

public class CreditsSubMenu : SubMenu
{
    [SerializeField] Button teamBioButton;
    [SerializeField] Button daniButton;
    [SerializeField] Button mateButton;
    [SerializeField] Button petiButton;

    [SerializeField] GameObject teamBio;
    [SerializeField] GameObject daniBio;
    [SerializeField] GameObject mateBio;
    [SerializeField] GameObject petiBio;

    private FadeControllerUI teamBioFader;
    private FadeControllerUI daniBioFader;
    private FadeControllerUI mateBioFader;
    private FadeControllerUI petiBioFader;

    private void Awake()
    {
        teamBioFader = teamBio.GetComponent<FadeControllerUI>();
        daniBioFader = daniBio.GetComponent<FadeControllerUI>();
        mateBioFader = mateBio.GetComponent<FadeControllerUI>();
        petiBioFader = petiBio.GetComponent<FadeControllerUI>();

        teamBioButton.onClick.AddListener(() => { HideAll(); teamBioFader.FadeIn(.2f); });
        daniButton.onClick.AddListener(() => { HideAll(); daniBioFader.FadeIn(.2f); });
        mateButton.onClick.AddListener(() => { HideAll(); mateBioFader.FadeIn(.2f); });
        petiButton.onClick.AddListener(() => { HideAll(); petiBioFader.FadeIn(.2f); });
    }

    private void Start()
    {
        HideAll();
        teamBioFader.FadeIn(.2f);
    }

    private void HideAll()
    {
        teamBioFader.FadeOut(.2f);
        daniBioFader.FadeOut(.2f);
        mateBioFader.FadeOut(.2f);
        petiBioFader.FadeOut(.2f);
    }
}