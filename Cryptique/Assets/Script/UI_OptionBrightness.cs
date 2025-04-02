using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OptionBrightness : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Image darkOverlay;


    private void Start()
    {
        SetBrightness(brightnessSlider.value);
    }

    public void SetBrightness(float value)
    {
        darkOverlay.color = new Color(0, 0, 0, value);
    }
}
