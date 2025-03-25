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

    [SerializeField] Button saveButton;

    [SerializeField] AudioManager audioManager;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(UpdateMusicValue);
        cameraMoveSenSlider.onValueChanged.AddListener(UpdateMoveSensValue);
        cameraOrbitSensSlider.onValueChanged.AddListener(UpdateOrbitValue);

        saveButton.onClick.AddListener(SaveToPlayerPrefs);
    }

    private void Start()
    {
        float volumePref = PlayerPrefs.GetFloat("MusicVolume");
        if (volumePref == 0f) { volumePref = 1f;}
        musicSlider.value = volumePref;
        UpdateMoveSensValue(volumePref);

        float moveSensPref = PlayerPrefs.GetFloat("CameraMoveSens");
        if (moveSensPref == 0f) { moveSensPref = 1f; }
        cameraMoveSenSlider.value = moveSensPref;
        UpdateMoveSensValue(moveSensPref);

        float orbitSensPref = PlayerPrefs.GetFloat("CameraOrbitSens");
        if (orbitSensPref == 0f) { orbitSensPref = 1f; }
        cameraOrbitSensSlider.value = orbitSensPref;
        UpdateMoveSensValue(orbitSensPref);
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

    private void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("CameraMoveSens", cameraMoveSenSlider.value);
        PlayerPrefs.SetFloat("CameraOrbitSens", cameraOrbitSensSlider.value);

        audioManager.SetVolume(musicSlider.value);
    }
}
