using UnityEngine;

public enum MusicList
{
    THEME_FULL,
    THEME_INTRO,
    THEME_LOOP,
    THEME_LONG_INTRO,
    THEME_SMALL_INTRO_SWITCH,
    MAIN_GAME_MUSIC_LOOP,
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicList;
    private static AudioManager audioManagerInstance;
    private AudioSource audioSource;

    private void Awake()
    {
        audioManagerInstance = this;
        audioSource = GetComponent<AudioSource>();
    }

    

    public static void PlayMusic(MusicList music, float volume = 1) 
    {
        if (Exists()) {
            audioManagerInstance.audioSource.PlayOneShot(audioManagerInstance.musicList[(int)music], volume);
        }
        
    }

    public static void PlayMusicLoop(MusicList music, float volume =1f)
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
