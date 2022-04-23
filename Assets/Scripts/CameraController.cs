using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;

    readonly Contexts _contexts;
    void Start()
    {
        var _contexts = Contexts.sharedInstance;
        var e = _contexts.game.CreateEntity();
        e.AddCamera(cam);

    }

}
