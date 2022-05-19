using UnityEngine;
using System.Collections.Generic;
using Enums;



// helper class to store  mono objects
public class SceneManagerObject
{
    public MonoBehaviour obj { get; set; }
    public SceneObjectType type { get; set; }

    public SceneManagerObject(MonoBehaviour o, SceneObjectType typ)
    {
        obj = o;
        type = typ;
    }
}

class SceneManager : MonoBehaviour
{
    private List<SceneManagerObject> objects = new List<SceneManagerObject>();

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
    public int Register(MonoBehaviour obj, SceneObjectType type)
    {
        SceneManagerObject newObject = new SceneManagerObject(obj, type);
        objects.Add(newObject);

        return objects.Count - 1;
    }

    //remove object from the list
    public void Unregister(int id)
    {
        if (objects.Count < id)
        {
            objects.RemoveAt(id);
        }
    }
}
