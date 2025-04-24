using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class SFX_Ambiance : MonoBehaviour
{
    [SerializeField] public SFXData sfxData;
    [SerializeField] public string selectedSFXName;
    [SerializeField] private bool loop = false;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

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
            audioSource = SFXManager.Instance.PlaySFX(sfx.clip, transform.position, sfxMixerGroup, loop);
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

        // Champs AudioMixerGroup
        SerializedProperty sfxMixerGroupProp = serializedObject.FindProperty("sfxMixerGroup");
        EditorGUILayout.PropertyField(sfxMixerGroupProp);

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
