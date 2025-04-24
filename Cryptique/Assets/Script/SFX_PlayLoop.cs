using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class SFX_PlayLoop : MonoBehaviour
{
    [SerializeField] public SFXData sfxData;
    [SerializeField] public string selectedSFXName;
    [SerializeField] private int minWaitTime = 1;
    [SerializeField] private int maxWaitTime = 60;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;

    private SFXData.SFX sfx;

    private bool isWaiting = false;

    private void Start()
    {
        sfx = sfxData?.GetSFXByName(selectedSFXName);
    }

    private void Update()
    {
        if(!isWaiting)
        {
            SFXManager.Instance.PlaySFX(sfx.clip, transform.position, sfxMixerGroup);
            StartCoroutine(WaitForRandomTime(Random.Range(minWaitTime, maxWaitTime)));
        }
    }

    public IEnumerator WaitForRandomTime(int waitTime)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }

}


[CustomEditor(typeof(SFX_PlayLoop))]
public class SFX_PlayLoopEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SFX_PlayLoop sfxPlay = (SFX_PlayLoop)target;

        // Champs SFXData
        SerializedProperty sfxDataProp = serializedObject.FindProperty("sfxData");
        EditorGUILayout.PropertyField(sfxDataProp);

        // Champs min/max temps
        SerializedProperty minWaitProp = serializedObject.FindProperty("minWaitTime");
        SerializedProperty maxWaitProp = serializedObject.FindProperty("maxWaitTime");
        EditorGUILayout.PropertyField(minWaitProp);
        EditorGUILayout.PropertyField(maxWaitProp);

        // Champs AudioMixerGroup
        SerializedProperty sfxMixerGroupProp = serializedObject.FindProperty("sfxMixerGroup");
        EditorGUILayout.PropertyField(sfxMixerGroupProp);

        // Dropdown pour choisir la SFX
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
