using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class LocalizedSpriteUI : MonoBehaviour
{
    [SerializeField] private SpriteUITraduction localisationData;

    [SerializeField]
    private string key;
    private Image image;

    LanguageManager languageManager;

    private void Awake()
    {
        languageManager = LanguageManager.Instance;
        image = GetComponent<Image>();
    }
    private void Start()
    {
        languageManager.Register(this);
        UpdateSprite(languageManager.GetCurrentLanguage());
    }
    private void OnDestroy()
    {
        languageManager.Unregister(this);
    }
    public void UpdateSprite(LanguageCode language)
    {
        if (localisationData == null) return;
        image.sprite = localisationData.GetSprite(key, language);
    }
    public string GetKey() => key;
}
