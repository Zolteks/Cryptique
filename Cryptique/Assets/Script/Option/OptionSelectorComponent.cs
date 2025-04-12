using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class OptionSelectorComponent<T> : MonoBehaviour
{
    [SerializeField] protected string optionKey;
    [SerializeField] protected TextMeshProUGUI optionText;
    [SerializeField] protected List<T> options = new();

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
        if (value is T typedValue && options.Contains(typedValue))
        {
            currentIndex = options.IndexOf(typedValue);
            UpdateText();
        }
    }

    protected abstract void LoadFromSave();
    protected abstract void SaveToGameData(T value);

    protected void UpdateText()
    {
        if (IsValid())
            optionText.text = options[currentIndex]?.ToString();
    }

    private bool IsValid()
    {
        if (options.Count == 0)
        {
            Debug.LogError($"[OptionSelectorComponent<{typeof(T).Name}>] No options set for {gameObject.name}");
            return false;
        }
        return true;
    }
}
