using System;
using System.Collections.Generic;
using UnityEngine;
using static TextUITraduction;

public class LanguageManager : Singleton<LanguageManager>
{
    private SaveSystemManager saveSystemManager;

    private LanguageCode currentLanguage;

    private  readonly List<LocalizedTextUI> listeners = new();
    private readonly List<LocalizedSpriteUI> spriteListeners = new();
    private  readonly List<Action<LanguageCode>> callbackListeners = new();
    private  readonly List<ILocalizedElement> dynamicElements = new();

    private void Awake()
    {
        saveSystemManager = SaveSystemManager.Instance;
        currentLanguage = saveSystemManager.GetGameData().langue;
    }

    void Start()
    {
    }

    public void Register(LocalizedTextUI l)
    {
        if (!listeners.Contains(l))
        {
            listeners.Add(l);
            Debug.Log("Lacal register");
        }
    }

    public void Unregister(LocalizedTextUI l)
    {
        if (listeners.Contains(l))
            listeners.Remove(l);
    }

    public void Register(LocalizedSpriteUI l)
    {
        if (!spriteListeners.Contains(l))
        {
            spriteListeners.Add(l);
            Debug.Log("Lacal register");
        }
    }

    public void Unregister(LocalizedSpriteUI l)
    {
        if (spriteListeners.Contains(l))
            spriteListeners.Remove(l);
    }

    public  void Register(Action<LanguageCode> callback)
    {
        if (!callbackListeners.Contains(callback))
        {
            callbackListeners.Add(callback);
            Debug.Log("Call register");
        }
    }

    public  void Unregister(Action<LanguageCode> callback)
    {
        if (callbackListeners.Contains(callback))
            callbackListeners.Remove(callback);
    }

    public void Register(ILocalizedElement element)
    {
        if (!dynamicElements.Contains(element))
            dynamicElements.Add(element);
    }

    public void Unregister(ILocalizedElement element)
    {
        dynamicElements.Remove(element);
    }
    public void RefreshAll()
    {
        currentLanguage = saveSystemManager.GetGameData().langue;
        foreach (var l in listeners)
        {   
            l.UpdateText(currentLanguage);
        }

        foreach (var l in spriteListeners)
        {
            l.UpdateSprite(currentLanguage);
        }

        foreach (var callback in callbackListeners)
            callback(currentLanguage);

        foreach (var e in dynamicElements)
            e.RefreshLocalized();
    }

    public LanguageCode GetCurrentLanguage()
    {
        return currentLanguage;
    }
}
