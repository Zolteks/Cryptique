using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class OptionSelectorComponent : MonoBehaviour
{
    [SerializeField] protected string optionKey;
    [SerializeField] protected TextMeshProUGUI optionText;
    [SerializeField] protected List<string> options = new();

    protected int currentIndex = 0;

    protected SaveSystemManager saveSystemManager;

    protected virtual void Awake()
    {
        saveSystemManager = SaveSystemManager.Instance;

        LoadFromSave();
        UpdateText();
    }

    protected virtual void Start()
    {
        GetComponent<OptionRouter>()?.Register(optionKey, OnOptionExternallyChanged);
    }

    protected virtual void OnDestroy()
    {
        GetComponent<OptionRouter>()?.Unregister(optionKey, OnOptionExternallyChanged);
    }

    protected void NotifyAndSave()
    {
        var selected = options[currentIndex];
        OptionChangeNotifier.Notify(optionKey, selected);
        SaveToGameData(selected);
    }

    public void SetNext()
    {
        if (!IsValid()) return;
        currentIndex = (currentIndex + 1) % options.Count;
        NotifyAndSave();
        UpdateText();
    }

    public void SetPrevious()
    {
        if (!IsValid()) return;
        currentIndex = (currentIndex - 1 + options.Count) % options.Count;
        NotifyAndSave();
        UpdateText();
    }

    protected virtual void OnOptionExternallyChanged(object value)
    {
        string selected = value as string;
        if (options.Contains(selected))
        {
            currentIndex = options.IndexOf(selected);
            UpdateText();
        }
    }

    protected abstract void LoadFromSave();
    protected abstract void SaveToGameData(string value);

    protected void UpdateText()
    {
        if (IsValid()) optionText.text = options[currentIndex];
    }

    private bool IsValid()
    {
        if (options.Count == 0)
        {
            Debug.LogError($"[OptionSelectorComponent] No options set for {gameObject.name}");
            return false;
        }
        return true;
    }
}
