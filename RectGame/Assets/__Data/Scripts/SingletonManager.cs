using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonManager<T> : MonoBehaviour where T : SingletonManager<T>
{
    public static T Instance
    {
        get { 
            _instance ??= FindObjectOfType<T>();
            return _instance;
        }
    }

    private static T _instance;
}
