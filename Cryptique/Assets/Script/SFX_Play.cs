using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class SFX_Play : MonoBehaviour
{
    [SerializeField] public SFXData sfxData;
    [SerializeField] public string selectedSFXName;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    public void PlaySFX()
    {
        var sfx = sfxData?.GetSFXByName(selectedSFXName);
        if (sfx != null)
        {
            SFXManager.Instance.PlaySFX(sfx.clip, transform.position, sfxMixerGroup);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlaySFX();
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}


[CustomEditor(typeof(SFX_Play))]
public class SFX_PlayEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SFX_Play sfxPlay = (SFX_Play)target;

        // Champs de base
        SerializedProperty sfxDataProp = serializedObject.FindProperty("sfxData");
        EditorGUILayout.PropertyField(sfxDataProp);

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
