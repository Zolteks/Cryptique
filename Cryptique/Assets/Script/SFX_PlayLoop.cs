using System.Collections;
using UnityEditor;
using UnityEngine;

public class SFX_PlayLoop : MonoBehaviour
{
    [SerializeField] public SFXData sfxData;
    [SerializeField] public string selectedSFXName;

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
            AudioSource.PlayClipAtPoint(sfx.clip, transform.position);
            Debug.Log("DU SONNNN");
            StartCoroutine(WaitForRandomTime(Random.Range(1, 60)));
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
