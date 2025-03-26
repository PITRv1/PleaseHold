using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;

public class OptionsSubMenu : SubMenu
{
    [SerializeField] Slider musicSlider;
    [SerializeField] TextMeshProUGUI musicText;

    [SerializeField] Slider cameraMoveSenSlider;
    [SerializeField] TextMeshProUGUI cameraMoveSensText;

    [SerializeField] Slider cameraOrbitSensSlider;
    [SerializeField] TextMeshProUGUI cameraOrbitSensText;

    [SerializeField] Slider cameraZoomSensSlider;
    [SerializeField] TextMeshProUGUI cameraZoomSensText;


    [SerializeField] Button saveButton;

    [SerializeField] MainMenuAudioManager audioManager;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(UpdateMusicValue);
        cameraMoveSenSlider.onValueChanged.AddListener(UpdateMoveSensValue);
        cameraOrbitSensSlider.onValueChanged.AddListener(UpdateOrbitValue);
        cameraZoomSensSlider.onValueChanged.AddListener(UpdateZoomValue);


        saveButton.onClick.AddListener(SaveToPlayerPrefs);
    }

    private void Start()
    {
        float volumePref = PlayerPrefs.GetFloat("MusicVolume");
        if (volumePref == 0f) { volumePref = 1f;}
        musicSlider.value = volumePref;
        UpdateMusicValue(volumePref);

        float moveSensPref = PlayerPrefs.GetFloat("CameraMoveSens");
        cameraMoveSenSlider.value = moveSensPref;
        UpdateMoveSensValue(moveSensPref);

        float orbitSensPref = PlayerPrefs.GetFloat("CameraOrbitSens");
        cameraOrbitSensSlider.value = orbitSensPref;
        UpdateOrbitValue(orbitSensPref);

        float zoomSensPref = PlayerPrefs.GetFloat("CameraOrbitSens");
        cameraZoomSensSlider.value = zoomSensPref;
        UpdateZoomValue(zoomSensPref);
    }

    private void UpdateMusicValue(float value)
    {
        float displayValue = value * 100f;
        musicText.text = displayValue.ToString("F0");
    }
    private void UpdateMoveSensValue(float value)
    {
        float displayValue = value * 100f;
        cameraMoveSensText.text = displayValue.ToString("F0");
    }
    private void UpdateOrbitValue(float value)
    {
        float displayValue = value * 100f;
        cameraOrbitSensText.text = displayValue.ToString("F0");
    }

    private void UpdateZoomValue(float value)
    {
        float displayValue = value * 100f;
        cameraZoomSensText.text = displayValue.ToString("F0");
    }

    private void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("CameraMoveSens", cameraMoveSenSlider.value);
        PlayerPrefs.SetFloat("CameraOrbitSens", cameraOrbitSensSlider.value);
        PlayerPrefs.SetFloat("CameraZoomSens", cameraZoomSensSlider.value);


        audioManager.SetVolume(musicSlider.value);
    }
}
