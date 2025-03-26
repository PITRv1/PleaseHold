using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class IntroAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicList;
    private static IntroAudioManager audioManagerInstance;
    private AudioSource audioSource;

    private void Awake()
    {
        audioManagerInstance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        Invoke("PlayMainTheme", musicList[(int)MusicList.THEME_SMALL_INTRO_SWITCH].length);
    }

    private void PlayIntro() {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        if (musicVolume == 0) { musicVolume = 1f; }

        PlayMusic(MusicList.THEME_SMALL_INTRO_SWITCH, musicVolume);
        Debug.Log(musicList[(int)MusicList.THEME_SMALL_INTRO_SWITCH].length);
        Invoke("PlayMainTheme", musicList[(int)MusicList.THEME_SMALL_INTRO_SWITCH].length);
    }

    private void PlayMainTheme() {
        PlayMusicLoop(MusicList.THEME_LOOP);
    }

    public static void PlayMusic(MusicList music, float volume = 1) 
    {
        if (Exists()) {
            audioManagerInstance.audioSource.PlayOneShot(audioManagerInstance.musicList[(int)music], volume);
        }
        
    }

    public static void PlayMusicLoop(MusicList music, float volume = 1f)
    {
        if (Exists()) {
            audioManagerInstance.audioSource.clip = audioManagerInstance.musicList[(int)music];
            audioManagerInstance.audioSource.volume = volume;
            audioManagerInstance.audioSource.loop = true;
            audioManagerInstance.audioSource.Play();
        }
    }

    public static bool IsPlaying()
    {
        if (Exists()) {
            return audioManagerInstance.audioSource.isPlaying;
        } else {
            return false;
        }
    }

    public static bool Exists() {
        if (audioManagerInstance) {
            return true;
        } else {
            return false;
        }
    }


    public static float GetClipLength(MusicList music)
    {
        return audioManagerInstance.musicList[(int)music].length;
    }

    public void SetVolume(float volume)
    {
        audioManagerInstance.audioSource.volume = volume;
    }
}
