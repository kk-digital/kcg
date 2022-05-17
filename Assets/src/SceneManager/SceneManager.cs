using UnityEngine;
using System.Collections.Generic;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }
    private List<string> objects = new List<string>();

    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
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
