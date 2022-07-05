using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    private Transform _cam;
    public float parallaxEffect;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<MeshRenderer>().bounds.size.x;
        _cam = Camera.main.transform;
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    void Update()
    {
        float temp = (_cam.position.x * (1 - parallaxEffect));
        float dist = (_cam.position.x * parallaxEffect);
        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}
