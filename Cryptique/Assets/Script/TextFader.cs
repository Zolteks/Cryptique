using UnityEngine;
using TMPro;

public class TextFader : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textUI;
    [SerializeField]
    private float fadeDuration = 2f; // Durée du fade in/out
    private Color originalColor;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (textUI == null)
            textUI = GetComponent<TextMeshProUGUI>();

        originalColor = textUI.color;
    }

    public void FadeIn()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeText(0f, 1f));
    }

    public void FadeOut()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeText(1f, 0f));
    }

    private System.Collections.IEnumerator FadeText(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        Color color = originalColor;
        color.a = startAlpha;
        textUI.color = color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            textUI.color = color;
            yield return null;
        }

        // Assure que l'alpha est exactement à la fin
        color.a = endAlpha;
        textUI.color = color;
    }
}
