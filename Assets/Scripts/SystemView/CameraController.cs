using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float scale = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.position += Vector3.right * Input.GetAxis("Mouse X") * -0.28f / scale;
            transform.position += Vector3.up    * Input.GetAxis("Mouse Y") * -0.28f / scale;
        }

        scale += Input.GetAxis("Mouse ScrollWheel") * 0.3f * scale;
        GetComponent<Camera>().orthographicSize = 20.0f / scale;
    }
}
