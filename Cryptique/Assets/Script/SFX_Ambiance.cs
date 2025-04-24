using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SFX_Ambiance : MonoBehaviour
{
    [SerializeField] public SFXData sfxData;
    [SerializeField] public string selectedSFXName;
    [SerializeField] private bool loop = false;

    private AudioSource audioSource;

    void Start()
    {
        PlaySFX();
    }

    public void PlaySFX()
    {
        var sfx = sfxData?.GetSFXByName(selectedSFXName);
        if (sfx != null)
        {
            audioSource = SFXManager.Instance.PlaySFX(sfx.clip, transform.position, loop);
        }
    }

    public void SlowDisableAudioAmbiance()
    {
        SFXManager.Instance.FadeOutAndDestroy(audioSource, 5f);
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

        SerializedProperty loopProp = serializedObject.FindProperty("loop");
        EditorGUILayout.PropertyField(loopProp);

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
