using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

[ExecuteInEditMode]
[RequireComponent(typeof(PixelSnap))]
public class PixelPerfectGrid : MonoBehaviour
{

    private Vector3 mousePosition;
    public float CursorMoveSpeed = 0.1f;
    public float XOffset = 0.0f;
    public float YOffset = 0.0f;

    void Awake()
    {
        SceneManager.Instance.Register(this, SceneObjectType.SceneObjectTypeUtilityScript);
    }

    void Start()
    {
        if(!followCursor)
            transform.position = new Vector3(XOffset, YOffset, transform.position.z);
    }

    private void OnGUI()
    {
        if (!followCursor)
            transform.position = new Vector3(XOffset, YOffset, transform.position.z);
    }

    public bool followCursor = false;

    private void FixedUpdate()
    {
        if (followCursor)
        {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, CursorMoveSpeed);
        }
    }

   
}
