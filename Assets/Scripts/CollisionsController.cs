using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionsController : MonoBehaviour
{
    readonly Contexts _contexts;
    void OnCollisionEnter2D(Collision2D coll)
    {
       // UnityEngine.Debug.Log("boom");
        var _contexts = Contexts.sharedInstance;
        var e = _contexts.game.CreateEntity();
        e.AddCollision(coll);
        e.isDestroyed = true;

    }

}
