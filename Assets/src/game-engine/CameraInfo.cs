using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
#endif

//Needs Camera Compoent to Work
[CustomEditor(typeof(CameraInfo))]
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraInfo : MonoBehaviour
{
    // Target Camera
    Camera cam;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {
        if(cam == null)
        {
            // Assign camera object
            cam = GetComponent<Camera>();
        }
    }

    // Set Camera's world position
    public void SetPosition(Vector3 newPosition)
    {
        if (cam != null)
            cam.transform.position = newPosition;
        else
            Debug.LogError("Camera object is empty.");
    }

    // Set Camera Depth (Default: 1)
    public void SetDepth(int newDepth)
    {
        cam.depth = newDepth;
    }

    // Set Near and Far Clip Values
    public void SetAspect(float NearZ, float farZ)
    {
        if (cam != null)
        {
            cam.nearClipPlane = NearZ;
            cam.farClipPlane = farZ;
        }
        else
        {
            Debug.LogError("Camera object is empty.");
        }
    }

    // Set culling mask
    public void SetCullingMask(int newMask)
    {
        if (cam != null)
            cam.cullingMask = newMask;
        else
            Debug.LogError("Camera object is empty.");
    }

    // Set Projection matrix
    public void SetProjectionMatrix(Matrix4x4 newProjMatrix)
    {
        if (cam != null)
            cam.projectionMatrix = newProjMatrix;
        else
            Debug.LogError("Camera object is empty.");
    }

    // Set Camera projection to orthographic
    public void SetCameraToOrtho()
    {
        if (cam != null)
            cam.orthographic = true;
        else
            Debug.LogError("Camera object is empty.");
    } 

    // Set Camera projection to perspective
    public void SetCameraToPerspective()
    {
        if (cam != null)
            cam.orthographic = false;
        else
            Debug.LogError("Camera object is empty.");
    }

    // Set FOV Axis (Only works in perspective mode)
    public void SetCameraFOVAxis(float newFOV)
    {
        if (cam != null)
        {
            if(!cam.orthographic)
            cam.fieldOfView = newFOV;
        }
        else
            Debug.LogError("Camera object is empty.");
    }
 
    // Set new Camera object
    public void SetCamera(Camera newCamera)
    {
        if (cam != null)
            cam = newCamera;
        else
            Debug.LogError("Camera object is empty.");
    }

    // Get Current Camera Object
    public Camera GetCurrentCamera()
    {
        if (cam != null)
            return cam;
        else
            Debug.LogError("Camera object is empty.");
        return null;
    }

    // Get Camera Position
    public Vector3 GetCameraPosition()
    {
        if (cam != null)
            return cam.transform.position;
        else
            Debug.LogError("Camera object is empty.");
        return Vector3.zero;
    }


}
