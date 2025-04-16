using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ColorBlindnessMaterials", menuName = "Settings/ColorBlindnessMaterials")]
public class ColorBlindnessMaterials : ScriptableObject
{
    public List<Material> materials;

}
