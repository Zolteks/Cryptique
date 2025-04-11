using System;
using System.Collections.Generic;
using UnityEngine;
using static TextUITraduction;

public class LanguageManager : Singleton<LanguageManager>
{
    private SaveSystemManager saveSystemManager;

    private string currentLanguage;

    private  readonly List<LocalizedTextUI> listeners = new();
    private  readonly List<Action<string>> callbackListeners = new();

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

    public  void Register(Action<string> callback)
    {
        if (!callbackListeners.Contains(callback))
        {
            callbackListeners.Add(callback);
            Debug.Log("Call register");
        }
    }

    public  void Unregister(Action<string> callback)
    {
        if (callbackListeners.Contains(callback))
            callbackListeners.Remove(callback);
    }

    public void RefreshAll()
    {
        currentLanguage = saveSystemManager.GetGameData().langue;
        foreach (var l in listeners)
        {   
            l.UpdateText(currentLanguage);
        }

        foreach (var callback in callbackListeners)
            callback(currentLanguage);
    }

    public string GetCurrentLanguage()
    {
        Debug.Log("Current Language: " + currentLanguage);
        return currentLanguage;
    }
}
