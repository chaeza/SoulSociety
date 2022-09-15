using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // find
                _instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (_instance == null)
                {
                    // Create
                    _instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                }

            }
            return _instance;
        }
    }
}
