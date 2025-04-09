using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskOpacityHandler : MonoBehaviour
{
    [SerializeField] Image panel;

    public void SetOpacity(float opacity)
    {
        Color nextColor = panel.color;
        nextColor.a = opacity;
        panel.color = nextColor;
    }
}
