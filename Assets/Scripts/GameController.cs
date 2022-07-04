using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    RootSystems _systems;
    Contexts _contexts;
    // Start is called before the first frame update
    void Awake()
    {
        _contexts = Contexts.sharedInstance;
   


        _systems = new RootSystems(Contexts.sharedInstance);
        _systems.Initialize();


    }

    void Update()
    {
        _systems.Execute();
        _systems.Cleanup();
    }


}
