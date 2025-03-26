using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource MainMenuLoopingMusic;
    [SerializeField] AudioSource MainMenuStartingSound;

    private void Awake() {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        if (musicVolume == 0) { musicVolume = 1f; }
        MainMenuLoopingMusic.volume = musicVolume;
        MainMenuStartingSound.volume = musicVolume;
    }

    private void Start()
    {
        Invoke("StartNext", MainMenuStartingSound.clip.length);
    }

    private void StartNext() {
        MainMenuLoopingMusic.Play();
    }

    public void SetVolume(float musicVolume) {
        MainMenuLoopingMusic.volume = musicVolume;
        MainMenuStartingSound.volume = musicVolume;
    }
}
