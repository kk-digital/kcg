using UnityEngine;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour
{
    private List<string> objects = new List<string>();

    private static SceneManager _instance;
    public static SceneManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SceneManager>();
            }

            return _instance;
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    //add object to the list
    public void Register(string typeName)
    {
        objects.Add(typeName);
    }

    //remove object from the list
    public void Unregister(string typeName)
    {
        objects.Remove(typeName);
    }
}
