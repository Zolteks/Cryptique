#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneDropdownAttribute))]
public class SceneDropdownDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        List<string> scenes = new List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            scenes.Add(name);
        }

        int selectedIndex = Mathf.Max(scenes.IndexOf(property.stringValue), 0);
        int newIndex = EditorGUI.Popup(position, label.text, selectedIndex, scenes.ToArray());

        if (newIndex >= 0 && newIndex < scenes.Count)
        {
            property.stringValue = scenes[newIndex];
        }
    }
}
#endif

public class SceneDropdownAttribute : PropertyAttribute { }
