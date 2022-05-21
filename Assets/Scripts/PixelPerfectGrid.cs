using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

/*
GRID CONTROL SCRIPT TO TEST PIXEL PERFECT CAMERA AND PIXEL SNAP
 */

// NEEDS "PIXEL SNAP" COMPONENT TO WORK
[ExecuteInEditMode]
[RequireComponent(typeof(PixelSnap))]
public class PixelPerfectGrid : MonoBehaviour
{
    //Cursor posisiton
    private Vector3 mousePosition;

    // Setting cursor move speed to drag grid
    public float CursorMoveSpeed = 0.1f;

    // Grid's X Offset
    public float XOffset = 0.0f;

    // Grid's Y Offset
    public float YOffset = 0.0f;
    
    // Sprite Renderer
    SpriteRenderer spriteRenderer;

    // Can grid follow mouse condition
    public bool followCursor = false;
    
    // Grid's Alpha Color
    [Range(0f, 1f)]
    public float targetAlpha = 1.0f;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Awake()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.Register(this, SceneObjectType.SceneObjectTypeUtilityScript);
        }
    }
    
    void Start()
    {
        // When we off the cursor following, grid go backs to his offset
        if (!followCursor)
            transform.position = new Vector3(XOffset, YOffset, transform.position.z);

        // Assign Sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(spriteRenderer)
        {
            // Assign grid's alpha
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetAlpha);
        }
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html
    private void OnGUI()
    {
        // When we off the cursor following, grid go backs to his offset
        if (!followCursor)
            transform.position = new Vector3(XOffset, YOffset, transform.position.z);

        if(spriteRenderer)
        {
            // Assign grid's alpha everytime on gui event
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, targetAlpha);
        }
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html
    private void FixedUpdate()
    {
        // If follow cursor is true, grid follows cursor (in Screen space)
        if (followCursor)
        {
            // Getting cursor pos
            mousePosition = Input.mousePosition;
            // Converting to screen space
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            // Then sign grid's transform to cursor
            transform.position = Vector2.Lerp(transform.position, mousePosition, CursorMoveSpeed);
        }
    }

   
}
