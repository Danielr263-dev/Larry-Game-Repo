using UnityEngine;
using UnityEngine.SceneManagement;  // Needed to load scenes

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;  // Name of the scene to transition to

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Ensure only the Player triggers the transition
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
