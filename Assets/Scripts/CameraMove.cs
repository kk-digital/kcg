using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float CameraSpeed= 0.3f;

    void Awake()
    {
        SceneManager.Instance.Register(this.GetType().Name);
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
