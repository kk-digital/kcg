using System.Collections.Generic;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//Needs Camera Compoent to Work
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraInfo : MonoBehaviour
{
    // Target Camera
    Camera cam;
    
    // Input Informations
    public float OrthoSize;
    public string AspectRatio = string.Format("16:9");

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {
        if(cam == null)
        {
            // Assign camera object
            cam = GetComponent<Camera>();
        }
    }

    private void FixedUpdate()
    {
        // Assigning Info Input values
        OrthoSize = cam.orthographicSize;

        if (cam.aspect >= 1.7f)
            AspectRatio = string.Format("16:9");
        else if (cam.aspect > 1.6f)
            AspectRatio = string.Format("5:3");
        else if (cam.aspect >= 1.5f)
            AspectRatio = string.Format("16:10");
        else
            AspectRatio = string.Format("4:3");
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

    // Get Camera Rotation
    public Vector3 GetCameraRotation()
    {
        if (cam != null)
            return cam.transform.eulerAngles;
        else
            Debug.LogError("Camera object is empty.");
        return Vector3.zero;
    }

    // Get Condition if its ortho or not
    public bool IsOrtho()
    {
        if (cam != null)
            return cam.orthographic;
        else
            Debug.LogError("Camera object is empty.");
        return false;
    }
    
    // Get Ortho Size (only works in Orthographic mode)
    public float GetOrthoSize()
    {
        if (cam != null)
            return cam.orthographicSize;
        else
            Debug.LogError("Camera object is empty.");
        return 0.0f;
    }

    // Get Screen Resolution
    public Vector2 GetResolution()
    {
        if (cam != null)
            return new Vector2(cam.pixelWidth, cam.pixelHeight);
        else
            Debug.LogError("Camera object is empty.");

        return Vector2.zero;
    }

    // Get Aspect
    public float GetAspect()
    {
        if (cam != null)
            return cam.aspect;
        else
            Debug.LogError("Camera object is empty.");
        return 0.0f;
    }

    // Get Field of View (FOV)
    public float GetFieldOfView()
    {
        if (cam != null)
            return cam.fieldOfView;
        else
            Debug.LogError("Camera object is empty.");
        return 0.0f;
    }

}

/*
    //Editor class to write notes or values on to the script inspector
*/
#if UNITY_EDITOR
[CustomEditor(typeof(CameraInfo))]
[CanEditMultipleObjects]
public class CameraInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CameraInfo myCamera = (CameraInfo)target;

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.richText = true;

        string Size = string.Format("{0:0.00}", myCamera.OrthoSize);
        string Width = string.Format("{0:0.00}", myCamera.GetResolution().x);
        string Height = string.Format("{0:0.00}", myCamera.GetResolution().y);

        // ---- Camera Informations ---- 

        style.fontSize = 15;
        EditorGUILayout.LabelField("Camera Information", style);
        EditorGUILayout.Space();
        style.fontSize = 12;

        // If it's ortho, get size info. If not, get fov info.
        if (myCamera.IsOrtho())
            EditorGUILayout.LabelField("Ortho Size", string.Format("{0}", Size), style);
        else
            EditorGUILayout.LabelField("FOV", myCamera.GetFieldOfView().ToString(), style);

        // Camera Resolution (pixelW, pixelH)
        EditorGUILayout.LabelField("Resolution", string.Format("{0} x {1}", Width, Height), style);
        
        // Aspect Ratio
        EditorGUILayout.LabelField("Aspect Ratio", string.Format("{0}", myCamera.AspectRatio), style);

        // Camera World Position
        EditorGUILayout.LabelField("Position", string.Format("X: {0}, Y: {1}, Z: {2}", myCamera.GetCameraPosition().x, myCamera.GetCameraPosition().y, myCamera.GetCameraPosition().z), style);

        // Camera World Rotation
        EditorGUILayout.LabelField("Rotation", string.Format("X: {0}, Y: {1}, Z: {2}", myCamera.GetCameraRotation().x, myCamera.GetCameraRotation().y, myCamera.GetCameraRotation().z), style);

        // ---- Camera Informations End ---- 


        // ---- Screen Informations ---- 

        style.fontSize = 15;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Screen Information", style);
        style.fontSize = 12;

        // Screen Size
        EditorGUILayout.LabelField("Size", string.Format("Width: {0}, Height {1}", Screen.width, Screen.height), style);
        
        // Screen Resoluiton
        EditorGUILayout.LabelField("Resolution", string.Format("{0} x {1}", Screen.width, Screen.height), style);

        // Screen Ratio Calculating
        string ratio = "";
        if (Screen.width / Screen.height >= 1.7f)
            ratio = string.Format("16:9");
        else if (Screen.width / Screen.height > 1.6f)
            ratio = string.Format("5:3");
        else if (Screen.width / Screen.height >= 1.5f)
            ratio = string.Format("16:10");
        else
            ratio = string.Format("4:3");

        // Screen Aspect Ratio
        EditorGUILayout.LabelField("Aspect Ratio ", ratio, style);

        // ---- Screen Informations End ---- 


        // ---- System Informations ---- 

        style.fontSize = 15;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("System Information", style);
        style.fontSize = 12;

        // Local Device Name
        EditorGUILayout.LabelField("Device Name",  SystemInfo.deviceName, style);
        
        // GPU Device Name
        EditorGUILayout.LabelField("GPU", SystemInfo.graphicsDeviceName.ToString(), style);
        
        // GPU Memory Size
        EditorGUILayout.LabelField("GPU Memory Size", SystemInfo.graphicsMemorySize.ToString(), style);
        
        // GPU Driver Version
        EditorGUILayout.LabelField("Driver Version", SystemInfo.graphicsDeviceVersion.ToString(), style);
        
        // Currnet Graphics API
        EditorGUILayout.LabelField("Currnet Graphics API", SystemInfo.graphicsDeviceType.ToString(), style);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif

