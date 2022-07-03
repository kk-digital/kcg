using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public sealed class GunController : MonoBehaviour
{

    public GameObject g;

    void Start()
    {
        var context = Contexts.sharedInstance;
        UnityEngine.Debug.Log("Added Gun");
        var e = context.game.CreateEntity();
        
        e.AddGun(0.3f, 0);
        e.isControllable=true;
        e.isFireable = true;
        e.AddView(gameObject);
        g = gameObject;
    }

    
}
