using UnityEngine;

public class ColorBlindnessController : MonoBehaviour
{
    public enum ColorBlindnessMode
    {
        None = 0,
        Protanopia = 1,
        Deuteranopia = 2,
        Tritanopia = 3
    }

    [Header("Référence à la Feature de daltonisme")]
    [SerializeField] private ColorBlindnessFeature colorBlindnessFeature;

    [Header("Mode actuel de daltonisme")]
    public ColorBlindnessMode currentMode = ColorBlindnessMode.None;

    void Start()
    {
        UpdateShaderMode();
    }

    public void SetColorBlindnessMode(ColorBlindnessMode mode)
    {
        currentMode = mode;
        UpdateShaderMode();
    }

    private void UpdateShaderMode()
    {
        if (colorBlindnessFeature != null)
        {
            foreach (var material in colorBlindnessFeature.GetMaterials())
            {
                if (material != null)
                {
                    material.SetFloat("_Mode", (float)currentMode);
                }
            }
        }
        else
        {
            Debug.LogWarning("La Feature de daltonisme n'est pas assignée.");
        }
    }
}
