using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class MainMenuTransition : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private PlaySubMenuInputManager inputManager;
    [SerializeField] private PlaySubMenu playSubMenu;


    private void Start()
    {
        playSubMenu.OnGameCanStart += PlaySubMenu_OnGameCanStart;
        playSubMenu.OnGameCanContinue += PlaySubMenu_OnGameCanContinue;

        AudioManager.PlayMusic(MusicList.THEME_SMALL_INTRO_SWITCH);

        //Invoke("PlayMainTheme", AudioManager.GetClipLength(MusicList.THEME_SMALL_INTRO_SWITCH) - 1f);

    }

    //private void playmaintheme()
    //{
    //    audiomanager.playmusicloop(musiclist.theme_loop);
    //}

    private void Update()
    {
        if (!AudioManager.IsPlaying())
        {
            AudioManager.PlayMusicLoop(MusicList.THEME_LOOP);
        }
    }

    private void PlaySubMenu_OnGameCanContinue(object sender, System.EventArgs e)
    {
        StartCoroutine(WaitForCameraTransition());
    }

    private void PlaySubMenu_OnGameCanStart(object sender, System.EventArgs e)
    {
        if (inputManager.CanGameStart())
        {
            StartCoroutine(WaitForCameraTransition());
        }
    }

    private IEnumerator WaitForCameraTransition()
    {
        bool blendStarted = false;

        while (cinemachineBrain.ActiveBlend == null)
        {
            yield return null;
        }

        blendStarted = true;

        while (cinemachineBrain.ActiveBlend != null)
        {
            yield return null;
        }

        if (blendStarted)
        {
            SceneLoader.Load(SceneLoader.Scene.MainGameScene);
        }
    }

}
