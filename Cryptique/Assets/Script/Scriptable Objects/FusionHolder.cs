using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Cryptique/FusionHolder")]
public class FusionHolder : ScriptableObject
{
    [SerializeField] public List<OBJ_Item> items;
    [SerializeField] public OBJ_Item reward;
}
