using UnityEngine;
using TMPro;

public class CinematicTextPlayer : MonoBehaviour
{
    [SerializeField] private CinematicTextData data;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private TextFader textFader;

    private int currentIndex = 0;
    private float timer = 0f;

    private void Start()
    {
        if (data == null || textUI == null)
        {
            Debug.LogError("Missing data or text UI!");
            enabled = false;
            return;
        }
        textUI.text = "";
    }

    private void Update()
    {
        if (currentIndex >= data.entries.Count) return;

        timer += Time.deltaTime;

        var entry = data.entries[currentIndex];

        if (timer >= entry.time)
        {
            textUI.text = entry.text.GetLocalized(LanguageManager.Instance.GetCurrentLanguage());
            textFader.FadeIn();
            Debug.Log($"Displaying text: {textUI.text} at time: {entry.time}");
            currentIndex++;
        }
    }
}
