using System.Collections.Generic;
using System.Collections;
using UnityEngine;


// https://docs.unity3d.com/ScriptReference/MonoBehaviour.html

public class GameManager : MonoBehaviour
{
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
    /*https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
     * Awake is called when the script instance is being loaded.
     * The Awake function is called on all objects in the Scene before any object's Start function is called. 
     * Unity calls Awake only once during the lifetime of the script instance.
     */



    private void Awake()
    {
        _instance = this;
        Init();
    }

    public string filePath = "/SimpleSpriteSheet/Table1.png";

    /* https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
     * Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
     * Start is called exactly once in the lifetime of the script.
     * The Start function can be defined as a Coroutine, which allows Start to suspend its execution (yield).
     */



    void Start()
    {
        SpriteSheetManager spriteSheetManager = new SpriteSheetManager();
        spriteSheetManager.GetImageID(filePath); // finds png, add it to struct list and returns index
        spriteSheetManager.GetImage(0); // returns the sprite struct of the given index
    }

    /* https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
     * Update is called every frame, if the MonoBehaviour is enabled.
     */

    void Update()
    {
        
    }
    /* https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html
     * Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.
     * MonoBehaviour.FixedUpdate has the frequency of the physics system; it is called every fixed frame-rate frame.
     * Use FixedUpdate when using Rigidbody. Set a force to a Rigidbody and it applies each fixed frame.
     */



    private void FixedUpdate()
    {
        
    }
    /* https://docs.unity3d.com/ScriptReference/MonoBehaviour.LateUpdate.html 
     * LateUpdate is called every frame
     * LateUpdate is called after all Update functions have been called. This is useful to order script execution.
     */

    private void LateUpdate()
    {
        
    }
    /* https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html 
     * OnGUI is called for rendering and handling GUI events.
     * OnGUI is the only function that can implement the "Immediate Mode" GUI (IMGUI) system for rendering and handling GUI events.
     */
    private void OnGUI()
    {
        
    }
    /* https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDisable.html
     * This function is called when the behaviour becomes disabled.
     * This is also called when the object is destroyed and can be used for any cleanup code.
     */
    private void OnDisable()
    {
        
    }
    /* https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnEnable.html
     */
    private void OnEnable()
    {
        
    }
    /* https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDestroy.html
     */
    private void OnDestroy()
    {
        
    }
    /* https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationQuit.html
     */
    public void OnApplicationQuit()
    {
        TearDown();
    }

    public void Init()
    {

    }

    public void InitStage1()
    {
        // allocations here
    }

    public void InitStage2()
    {
        // file loading operations here
    }

    public void TearDown()
    {

    }
}
