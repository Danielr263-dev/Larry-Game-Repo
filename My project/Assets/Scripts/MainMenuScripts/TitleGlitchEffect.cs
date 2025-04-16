using UnityEngine;
using TMPro;
using System.Collections;

public class TitleGlitchEffect : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public float glitchDuration = 1.5f;     // How long the glitch lasts
    public float glitchRate = 0.15f;        // How often it updates during glitch
    public float glitchInterval = 3f;       // Time between glitch loops

    private string originalText;
    private Vector2 originalPos;
    private Color originalColor;

    void Start()
    {
        originalText = titleText.text;
        originalPos = titleText.rectTransform.anchoredPosition;
        originalColor = titleText.color;
        StartCoroutine(GlitchLoop());
    }

    IEnumerator GlitchLoop()
    {
        while (true)
        {
            yield return StartCoroutine(Glitch());
            yield return new WaitForSeconds(glitchInterval);
        }
    }

    IEnumerator Glitch()
    {
        float timer = 0f;

        while (timer < glitchDuration)
        {
            // Subtle character glitch
            titleText.text = GlitchText(originalText);

            // Subtle RGB flicker
            titleText.color = GetSubtleGlitchColor();

            // Gentle shake
            titleText.rectTransform.anchoredPosition = originalPos + new Vector2(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.5f, 0.5f)
            );

            yield return new WaitForSeconds(glitchRate);

            // Restore original
            titleText.text = originalText;
            titleText.color = originalColor;
            titleText.rectTransform.anchoredPosition = originalPos;

            yield return new WaitForSeconds(glitchRate);
            timer += glitchRate * 2;
        }

        // Final reset
        titleText.text = originalText;
        titleText.color = originalColor;
        titleText.rectTransform.anchoredPosition = originalPos;
    }

    string GlitchText(string text)
    {
        string chars = "!@#$%^&*()_+-=[]{}|;:,.<>?";
        char[] glitched = text.ToCharArray();

        for (int i = 0; i < glitched.Length; i++)
        {
            if (Random.value < 0.1f) // Lower glitch chance = subtler effect
                glitched[i] = chars[Random.Range(0, chars.Length)];
        }

        return new string(glitched);
    }

    Color GetSubtleGlitchColor()
    {
        Color[] glitchColors = new Color[] {
            Color.red,
            Color.green,
            Color.blue,
            Color.magenta,
            Color.cyan,
            Color.yellow
        };

        Color randomGlitch = glitchColors[Random.Range(0, glitchColors.Length)];
        return Color.Lerp(randomGlitch, originalColor, 0.6f); // blend toward original
    }
}
