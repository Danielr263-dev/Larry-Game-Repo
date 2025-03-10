using System.Collections;
using UnityEngine;
using TMPro;

public class IntroDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // UI text for dialogue
    public GameObject blackScreen; // Black screen UI image
    public AudioSource textBeepSound; // Beep sound effect for typing
    public AudioSource backgroundMusic; // Background music (starts AFTER dialogue)
    public float textSpeed = 0.08f; // **Slower speed to prevent beep cutoff**
    public float fadeSpeed = 1.5f; // Speed of fading screen
    private bool flickerEffect = false; // Controls flickering glitch effect

    private string[] dialogueLines =
    {
        "…Where…?",
        "I was just… No, wait. What was I doing?",
        "…Who—",
        "…Who am I?"
    };

    void Start()
    {
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // Start with a fully black screen
        blackScreen.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 1);

        // Start displaying dialogue immediately
        yield return StartCoroutine(DisplayDialogue());

        // Hide the dialogue after it ends
        dialogueText.gameObject.SetActive(false);

        // Start background music AFTER dialogue fully disappears
        if (backgroundMusic != null)
        {
            backgroundMusic.Play();
        }

        // Fade out black screen AFTER the dialogue
        yield return StartCoroutine(FadeOutBlackScreen());
    }

    IEnumerator DisplayDialogue()
    {
        foreach (string line in dialogueLines)
        {
            // Enable glitch effect only for the "…Who am I?" line.
            flickerEffect = line.Contains("…Who am I?");
            yield return StartCoroutine(TypeText(line));
            yield return new WaitForSeconds(1.5f); // Small pause before next line
        }
    }

    IEnumerator TypeText(string text)
    {
        dialogueText.text = ""; // Clear text

        // Set the beep sound to loop and start it once
        if (textBeepSound != null)
        {
            textBeepSound.loop = true;
            textBeepSound.Play();
        }

        // Type out the text letter-by-letter
        for (int i = 0; i < text.Length; i++)
        {
            dialogueText.text += text[i];
            yield return new WaitForSeconds(textSpeed);
        }

        // Slight delay to allow the last letter's beep to play
        yield return new WaitForSeconds(0.1f);
        if (textBeepSound != null)
        {
            textBeepSound.Stop();
            textBeepSound.loop = false; // Optionally disable loop afterwards
        }

        // If this line should have the glitch effect, run a brief flicker loop
        if (flickerEffect)
        {
            float glitchDuration = 1.0f; // Total time for glitching
            float elapsed = 0f;
            while (elapsed < glitchDuration)
            {
                // Randomly decide whether to show the glitch effect
                if (Random.value > 0.5f)
                {
                    // Replace only "Who" with a red-colored "Who"
                    dialogueText.text = text.Replace("Who", "<color=#FF0000>Who</color>");
                }
                else
                {
                    dialogueText.text = text;
                }
                float waitTime = textSpeed; // You can adjust this small flicker duration if desired
                elapsed += waitTime;
                yield return new WaitForSeconds(waitTime);
            }
            // Ensure the final display is the normal text
            dialogueText.text = text;
        }
    }

    IEnumerator FadeOutBlackScreen()
    {
        UnityEngine.UI.Image blackScreenImage = blackScreen.GetComponent<UnityEngine.UI.Image>();
        float alpha = 1f;

        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeSpeed;
            blackScreenImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        blackScreen.SetActive(false);
    }
}
