using UnityEngine;
using UnityEngine.Video;

public class IntroSceneChanger : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        if (musicVolume == 0 ) { musicVolume = 100f; }
        AudioManager.PlayMusic(MusicList.THEME_LONG_INTRO, musicVolume);
    }

    void Update()
    {
        //if (!AudioManager.IsPlaying()) {
        //    AudioManager.PlayMusic(MusicList.THEME_SMALL_INTRO_SWITCH);
        //}
        
        if (videoPlayer.frame >= (long)videoPlayer.frameCount-1) {
            SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
        }
    }


}
