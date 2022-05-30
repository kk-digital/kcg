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
    public List<SceneManagerObject> SceneObjects;

    public static SceneManager _instance;
    public static SceneManager Instance => _instance;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
          _instance = gameObject.AddComponent(typeof(SceneManager)) as SceneManager;
    }


    //add object to the list
    public int Register(MonoBehaviour obj, SceneObjectType type)
    {
        SceneManagerObject newObject = new SceneManagerObject(obj, type);
        SceneObjects.Add(newObject);

        return SceneObjects.Count - 1;
    }

    //remove object from the list
    public void Unregister(int id)
    {
        if (SceneObjects.Count < id)
        {
            SceneObjects.RemoveAt(id);
        }
    }
}
