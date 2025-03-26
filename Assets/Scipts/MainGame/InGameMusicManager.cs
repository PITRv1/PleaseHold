using UnityEngine;

public class InGameAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MainMenuStartingSound;

    private void Awake()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        if (musicVolume == 0) { musicVolume = 1f; }
        MainMenuStartingSound.volume = musicVolume;
    }

    public void SetVolume(float musicVolume)
    {
        MainMenuStartingSound.volume = musicVolume;
    }
}