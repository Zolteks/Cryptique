using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SFX_Ambiance : MonoBehaviour
{
    [SerializeField] public SFXData sfxData;
    [SerializeField] public string selectedSFXName;

    SaveSystemManager saveSystemManager;

    void Start()
    {
        saveSystemManager = SaveSystemManager.Instance;
        PlaySFX();
    }

    public void PlaySFX()
    {
        var sfx = sfxData?.GetSFXByName(selectedSFXName);
        if (sfx != null)
        {
            AudioSource.PlayClipAtPoint(sfx.clip, transform.position);
        }
    }
}


[CustomEditor(typeof(SFX_Ambiance))]
public class SFX_TempeteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SFX_Ambiance sfxPlay = (SFX_Ambiance)target;

        // Champs de base
        SerializedProperty sfxDataProp = serializedObject.FindProperty("sfxData");
        EditorGUILayout.PropertyField(sfxDataProp);

        if (sfxPlay.sfxData != null)
        {
            string[] sfxNames = sfxPlay.sfxData.GetSFXNames();
            int currentIndex = Mathf.Max(0, System.Array.IndexOf(sfxNames, sfxPlay.selectedSFXName));

            int newIndex = EditorGUILayout.Popup("Selected SFX", currentIndex, sfxNames);

            if (newIndex >= 0 && newIndex < sfxNames.Length)
            {
                sfxPlay.selectedSFXName = sfxNames[newIndex];
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Aucune SFXData assignée", MessageType.Warning);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
