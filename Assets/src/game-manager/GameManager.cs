using System.Collections.Generic;
using System.Collections;
using UnityEngine;


// https://docs.unity3d.com/ScriptReference/MonoBehaviour.html

public class GameManager : MonoBehaviour
{
    public string filePath = "/SimpleSpriteSheet/Table1.png";

    //Todo, just make public, dont do getter/setter
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Null GameManager");
            return _instance;
        }
    }

    //Documentation: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
    //Awake is called when the script instance is being loaded.
    //The Awake function is called on all objects in the Scene before any object's Start function is called. 
    //Unity calls Awake only once during the lifetime of the script instance.
    private void Awake()
    {
        _instance = this;
        InitStage1();
    }

    //Documentation: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    //Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    //Start is called exactly once in the lifetime of the script.
    void Start()
    {
        //TODO: Either put all intialiation in Awake, or put all in Start()
        SpriteSheetManager spriteSheetManager = new SpriteSheetManager();
        spriteSheetManager.GetImageID(filePath); // finds png, add it to struct list and returns index
        spriteSheetManager.GetImage(0); // returns the sprite struct of the given index
    }

    //Documentation: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    //Update is called every frame, if the MonoBehaviour is enabled.
    void Update()
    {
        //TODO: put per frame calls here
        //Example: Drawing calls
    }


    //Documentation: https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html
    //Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.
    //MonoBehaviour.FixedUpdate has the frequency of the physics system
    private void FixedUpdate()
    {
         //TODO: Put physics update calls here
    }

 //Documentation: https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html 
 //LateUpdate is called every frame
 //LateUpdate is called after all Update functions have been called. This is useful to order script execution.
    private void LateUpdate()
    {
        //Note: Not Used right now
    }
    
    
    //Documentation: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html 
    //OnGUI is called for rendering and handling GUI events.
    //OnGUI is the only function that can implement the "Immediate Mode" GUI (IMGUI) system for rendering and handling GUI events.
    private void OnGUI()
    {
        //Note: Not used right now
    }

//Documentation: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDisable.html
//This function is called when the behaviour becomes disabled.
//This is also called when the object is destroyed and can be used for any cleanup code.
    private void OnDisable()
    {
        //Note: Dont use
    }

    //https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnEnable.html
    private void OnEnable()
    {
        //Note: Dont use
    }
    // https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDestroy.html

    private void OnDestroy()
    {
        
    }
  
   //https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationQuit.html
    public void OnApplicationQuit()
    {
        TearDown();
    }

    //All memory allocations/setups go here
    //File loading should not occur at this stage
    public void InitStage1()
    {
        //TODO: Intialize all managers here
    }

    //Load settings from files and other init, that requires systems to be intialized
    public void InitStage2()
    {
        //TODO: Start loading the files
        // file loading operations here
    }

    public void TearDown()
    {
        //TODO: Call code to tear down object
    }
}
