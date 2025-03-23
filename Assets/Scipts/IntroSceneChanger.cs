using UnityEngine;
using UnityEngine.Video;

public class IntroSceneChanger : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        AudioManager.PlayMusic(MusicList.THEME_INTRO);
    }

    void Update()
    {
        if (videoPlayer.frame >= (long)videoPlayer.frameCount-1) {
            SceneLoader.Load(SceneLoader.Scene.MainMenuScene);
        }
    }


}
