using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GrimoireShaker : MonoBehaviour
{
    private static GrimoireShaker instance;

    private void Awake()
    {
        instance = this;
    }

    public static void ShakeGrimoire(float duration = 1f, float intensity = 5f, float rotationIntensity = 5f)
    {
        RectTransform grimoireRect = GameObject.Find("PauseGrimoire")?.GetComponent<RectTransform>();
        if (grimoireRect != null && instance != null)
        {
            instance.StartCoroutine(instance.ShakeCoroutine(grimoireRect, duration, intensity, rotationIntensity));
        }
        else
        {
            Debug.LogWarning("PauseGrimoire not found or GrimoireShaker not in scene.");
        }
    }

    private IEnumerator ShakeCoroutine(RectTransform rectTransform, float duration, float intensity, float rotationIntensity)
    {
        Vector3 originalPos = rectTransform.localPosition;
        Quaternion originalRotation = rectTransform.localRotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Déplacement horizontal
            float offsetX = Mathf.Sin(elapsed * 50f) * intensity;

            // Rotation sur l'axe Z
            float offsetRotation = Mathf.Sin(elapsed * 50f) * rotationIntensity;

            // Appliquer les déplacements et rotations
            rectTransform.localPosition = originalPos + new Vector3(offsetX, 0f, 0f);
            rectTransform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, offsetRotation);

            elapsed += Time.unscaledDeltaTime; // supporte le menu pause
            yield return null;
        }

        // Restaurer la position et la rotation d'origine
        rectTransform.localPosition = originalPos;
        rectTransform.localRotation = originalRotation;
    }
}
