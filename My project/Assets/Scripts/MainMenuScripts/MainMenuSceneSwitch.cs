using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneSwitch : MonoBehaviour
{
    public AudioClip confirmationSFX;
    public AudioSource audioSource;

    private bool isLoading = false;

    public void PlayConfirmAndLoad(int sceneIndex)
    {
        Debug.Log("🔘 Button clicked. Calling PlayConfirmAndLoad(" + sceneIndex + ")");

        if (!isLoading)
        {
            isLoading = true;
            Debug.Log("⏳ Starting coroutine to play sound and load scene...");
            StartCoroutine(PlayAndLoad(sceneIndex));
        }
        else
        {
            Debug.Log("⚠️ Already loading — input ignored.");
        }
    }

    IEnumerator PlayAndLoad(int sceneIndex)
    {
        if (audioSource != null && confirmationSFX != null)
        {
            Debug.Log("🔊 Playing confirmation sound: " + confirmationSFX.name);
            audioSource.PlayOneShot(confirmationSFX);
            Debug.Log("⏱ Waiting for " + confirmationSFX.length + " seconds...");
            yield return new WaitForSeconds(confirmationSFX.length);
        }
        else
        {
            Debug.LogWarning("❌ Missing AudioSource or confirmationSFX!");
        }

        Debug.Log("🚪 Loading scene: " + sceneIndex);
        SceneManager.LoadScene(sceneIndex);
    }
}
