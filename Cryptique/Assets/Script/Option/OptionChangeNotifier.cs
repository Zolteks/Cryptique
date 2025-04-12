using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class OptionChangeNotifier : MonoBehaviour
{
    public static Action<string, object> OnOptionChanged;

    public static void Notify(string optionName, object newValue)
    {
        Debug.Log($"Option changed: {optionName} to {newValue}");
        OnOptionChanged?.Invoke(optionName, newValue);
    }
}

