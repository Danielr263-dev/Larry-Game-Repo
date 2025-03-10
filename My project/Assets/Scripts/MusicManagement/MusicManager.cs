using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource musicSource;
    private string currentScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.volume = 0.5f;
    }

    private void Update()
    {
        // Automatically stop & destroy MusicManager when entering FightScene
        if (SceneManager.GetActiveScene().name == "FightScene")
        {
            StartCoroutine(FadeOutAndDestroy(1.5f));
        }
    }

    /// <summary>
    /// Plays new music, fading out the old one first if needed.
    /// </summary>
    public void PlayMusic(AudioClip newMusic, string sceneName, float fadeDuration = 1.5f)
    {
        if (musicSource.clip == newMusic && sceneName == currentScene)
            return; // Prevent restarting the same music

        currentScene = sceneName;

        if (musicSource.isPlaying)
        {
            StartCoroutine(FadeOutMusic(newMusic, fadeDuration));
        }
        else
        {
            PlayNewMusic(newMusic);
        }
    }

    /// <summary>
    /// Stops music smoothly by fading out before stopping.
    /// </summary>
    public void StopMusic(float fadeDuration = 1.5f)
    {
        if (musicSource.isPlaying)
        {
            StartCoroutine(FadeOutAndStop(fadeDuration));
        }
    }

    /// <summary>
    /// Fades out the current music and then plays the new one.
    /// </summary>
    private IEnumerator FadeOutMusic(AudioClip newMusic, float fadeDuration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume; // Reset volume for future use
        PlayNewMusic(newMusic);
    }

    /// <summary>
    /// Fades out the current music completely before stopping.
    /// </summary>
    private IEnumerator FadeOutAndStop(float fadeDuration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;
    }

    /// <summary>
    /// Fades out and destroys MusicManager when transitioning to FightScene.
    /// </summary>
    private IEnumerator FadeOutAndDestroy(float fadeDuration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.Stop();
        Destroy(gameObject); // Ensures MusicManager is removed in FightScene
    }

    /// <summary>
    /// Plays new music with an optional fade-in effect.
    /// </summary>
    private void PlayNewMusic(AudioClip newMusic)
    {
        if (newMusic != null)
        {
            musicSource.clip = newMusic;
            musicSource.Play();
            StartCoroutine(FadeInMusic(1.5f)); // Adjust fade-in duration if needed
        }
        else
        {
            StopMusic();
        }
    }

    /// <summary>
    /// Fades in the new music to avoid sudden volume jumps.
    /// </summary>
    private IEnumerator FadeInMusic(float fadeDuration)
    {
        musicSource.volume = 0;
        float targetVolume = 0.5f; // Adjust as needed

        while (musicSource.volume < targetVolume)
        {
            musicSource.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
