using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[DisallowMultipleComponent]
public class LocalizedTextUI : MonoBehaviour
{
    [SerializeField] private TextUITraduction localisationData;

    [SerializeField]
    private string key;
    private TextMeshProUGUI text;

    LanguageManager languageManager;

    private void Awake()
    {
        languageManager = LanguageManager.Instance;
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        languageManager.Register(this);
        UpdateText(languageManager.GetCurrentLanguage());
    }

    private void OnDestroy()
    {
        languageManager.Unregister(this);
    }

    public void UpdateText(LanguageCode language)
    {
        if (localisationData == null) return;
        text.text = localisationData.GetText(key, language);
    }

    public string GetKey() => key;
}