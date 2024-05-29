using UnityEngine;

public abstract class SingletonObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {

                T[] results = Resources.FindObjectsOfTypeAll<T>();
                if (results.Length == 0)
                {
                    return null;
                }
                if (results.Length > 1)
                {
                    return null;
                }
                _instance = results[0];
            }
            return _instance;
        }
    }
}
