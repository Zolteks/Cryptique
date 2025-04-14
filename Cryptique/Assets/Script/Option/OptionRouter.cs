using System.Collections.Generic;
using System;
using UnityEngine;

public class OptionRouter : MonoBehaviour
{
    private Dictionary<string, Action<object>> listeners = new();

    private void OnEnable() => OptionChangeNotifier.OnOptionChanged += Handle;
    private void OnDisable() => OptionChangeNotifier.OnOptionChanged -= Handle;

    public void Register(string key, Action<object> callback)
    {
        if (!listeners.ContainsKey(key)) listeners[key] = callback;
        else listeners[key] += callback;
    }

    public void Unregister(string key, Action<object> callback)
    {
        if (listeners.ContainsKey(key)) listeners[key] -= callback;
    }

    private void Handle(string key, object value)
    {
        if (listeners.TryGetValue(key, out var action))
            action?.Invoke(value);
    }
}
