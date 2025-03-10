using UnityEngine;

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

    public void PlayMusic(AudioClip newMusic, string sceneName)
    {
        if (musicSource.clip == newMusic && sceneName == currentScene)
            return; // Prevent restarting the same music

        if (newMusic != null)
        {
            musicSource.clip = newMusic;
            musicSource.Play();
        }
        else
        {
            StopMusic(); // If null, stop music
        }

        currentScene = sceneName;
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
