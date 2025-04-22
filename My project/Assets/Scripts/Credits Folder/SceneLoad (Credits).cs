using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Call this from your Credits button
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");  // or whatever your main scene is named
    }

    // Call this from your Main Menu “Credits” button
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
