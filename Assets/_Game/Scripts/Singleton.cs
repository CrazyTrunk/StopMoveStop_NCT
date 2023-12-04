using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_ShuttingDown = false;

    private static T instance;
    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                return null;
            }
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name + " (Singleton)").AddComponent<T>();
                }
            }
            return instance;
        }
    }
    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }
    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}
