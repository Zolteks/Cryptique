using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour
    where T : Component
{
    private static T m_instance;
    public static T Instance {
        get {
            if (m_instance == null)
            {
                var objs = FindObjectsByType<T>(FindObjectsSortMode.None);
                if (objs.Length > 0)
                    m_instance = objs[0];
                if (objs.Length > 1) {
                    Debug.LogError ("There is more than one " + typeof(T).Name + " in the scene.");
                }
                if (m_instance == null) {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    m_instance = obj.AddComponent<T>();
                }
            }
            return m_instance;
        }
    }
}


public class SingletonPersistent<T> : MonoBehaviour
    where T : Component
{
    public static T Instance { get; private set; }
	
    public virtual void Awake ()
    {
        //if (Instance == null) {
            Instance = this as T;
            DontDestroyOnLoad (this);
        //} else {
        //    Destroy (gameObject);
        //}
    }
}