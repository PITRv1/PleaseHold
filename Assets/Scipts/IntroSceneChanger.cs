using UnityEngine;
using UnityEngine.Video;

public class IntroSceneChanger : MonoBehaviour
{
    [SerializeField] private VideoPlayer VideoPlayer;
    [SerializeField] private AudioSource IntroAudio;

    private void Awake() {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        if (musicVolume == 0) { musicVolume = 1f; }
        IntroAudio.volume = musicVolume;
    }

    private void Update()
    { 
        if ((VideoPlayer.frame >= (long)VideoPlayer.frameCount-1) || Input.GetKey(KeyCode.Space)) {
            SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
        }
    }


}