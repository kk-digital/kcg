using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] float Speed= 0.1f;

    void Start()
    {
        
    }


    void Update()
    {
        var x = 0f;
        var y = 0f;

        if (Input.GetKey(KeyCode.A)) x = -1;
        if (Input.GetKey(KeyCode.D)) x = 1;
        if (Input.GetKey(KeyCode.W)) y = 1;
        if (Input.GetKey(KeyCode.S)) y = -1;

        transform.position += Vector3.right * x * Time.deltaTime;
        transform.position += Vector3.up * y * Time.deltaTime;
    }
}
