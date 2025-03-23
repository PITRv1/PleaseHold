using UnityEngine;

public enum MusicList
{
    THEME_FULL,
    THEME_INTRO,
    THEME_LOOP
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
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlayMusic(MusicList music, float volume = 1) 
    {
        audioManagerInstance.audioSource.PlayOneShot(audioManagerInstance.musicList[(int)music], volume);
    }
}
