using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeGOSingleton : MonoBehaviour
{
    public static MakeGOSingleton instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
