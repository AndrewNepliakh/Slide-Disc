using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            _instance = FindObjectOfType<T>();

            if (_instance == null)
            {
                var go = new GameObject($"[Singleton: {typeof(T).ToString().ToUpper()}]"); 
                go.transform.SetSiblingIndex(0);
                go.AddComponent<T>();
                _instance = go.GetComponent<T>();
            }

            return _instance;
        }
    }
}
