using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class CameraMove : MonoBehaviour
{
    public float CameraSpeed = 6.0f;

    void Awake()
    {
        //Check if Scene has SceneManager setup
        if(SceneManager.Instance != null)
        {
            SceneManager.Instance.Register(this, SceneObjectType.SceneObjectTypeUtilityScript);
        }
    }

    void Update()
    {
        var x = 0f;
        var y = 0f;

        if (Input.GetKey(KeyCode.A)) x = -1;
        if (Input.GetKey(KeyCode.D)) x = 1;
        if (Input.GetKey(KeyCode.W)) y = 1;
        if (Input.GetKey(KeyCode.S)) y = -1;

        transform.position += Vector3.right * x * Time.deltaTime * CameraSpeed;
        transform.position += Vector3.up * y * Time.deltaTime * CameraSpeed;
    }
}
