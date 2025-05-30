using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Clips")]
    public AudioClip gameMusic;
    public AudioClip laserSfx;
    public AudioClip boomSfx;
    public AudioClip hurtSfx;

    [Header("Settings")]
    [Range(0f,1f)] public float musicVolume = 0.3f;
    [Range(0f,1f)] public float sfxVolume   = 0.5f;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    public void SetMusicVolume(float v)
    {
        musicVolume      = Mathf.Clamp01(v);
        musicSource.volume = musicVolume;
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    /// <summary>Call to immediately update the SFX volume.</summary>
    public void SetSfxVolume(float v)
    {
        sfxVolume      = Mathf.Clamp01(v);
        sfxSource.volume = sfxVolume;
        PlayerPrefs.SetFloat("SfxVolume", sfxVolume);
    }

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Music source
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = gameMusic;
            musicSource.loop = true;
            musicSource.volume = musicVolume;
            musicSource.Play();

            // SFX source
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.volume = sfxVolume;
        }
        else
        {
            Destroy(gameObject);
        }

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);
        sfxVolume   = PlayerPrefs.GetFloat("SfxVolume",   sfxVolume);
    }

    /// <summary>Play a one‐shot sound effect.</summary>
    public void PlaySfx(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }
}
