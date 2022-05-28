using UnityEngine;
using Enums;
#if UNITY_EDITOR
using UnityEditor;
#endif

//Needs Camera Compoent to Work
[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraInfo : MonoBehaviour
{
    // Target Camera
    public Camera cam;

    //Camera Struct
    public struct CameraProperties
    {
        public int cullingMask;
        public bool isOrtho;
        public float nearZ;
        public float farZ;
        public float depth;
        public Vector3 Position;
        public Vector3 Rotation;
        public float fieldOfView;
        public float orthographicSize;
        public float aspect;
    }

    // Struct Creation
    CameraProperties camProp;

    // Input Informations
    private float tempZoom;
    private bool canUpdate = true;
    private float minOrthoZoom = 0.5f;
    private float minFOVZoom = 10;
    private float maxOrthoZoom = 50;
    private float maxFOVZoom = 120;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
    private void Awake()
    {
        //Check if Scene has SceneManager setup
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.Register(this, SceneObjectType.SceneObjectTypeUtilityScript);
        }
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {
        if (cam == null)
        {
            // Assign camera object
            cam = GetComponent<Camera>();

            // Assigning struct values to camera values. Reciving all the info of the camera to our struct. So, we can save later on.
            camProp.depth = cam.depth;
            camProp.isOrtho = cam.orthographic;
            camProp.cullingMask = cam.cullingMask;
            camProp.farZ = cam.farClipPlane;
            camProp.nearZ = cam.nearClipPlane;
            camProp.Position = cam.transform.position;
            camProp.Rotation = cam.transform.eulerAngles;
            camProp.fieldOfView = cam.fieldOfView;
            camProp.orthographicSize = cam.orthographicSize;
            camProp.aspect = cam.aspect;
            tempZoom = cam.orthographicSize;
        }
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html
    private void OnGUI()
    {
        // Assigning struct values to camera values. Reciving all the info of the camera to our struct. So, we can save later on.
        if (canUpdate)
        {
            camProp.depth = cam.depth;
            camProp.isOrtho = cam.orthographic;
            camProp.cullingMask = cam.cullingMask;
            camProp.farZ = cam.farClipPlane;
            camProp.nearZ = cam.nearClipPlane;
            camProp.Position = cam.transform.position;
            camProp.Rotation = cam.transform.eulerAngles;
            camProp.fieldOfView = cam.fieldOfView;
            camProp.orthographicSize = cam.orthographicSize;
            camProp.aspect = cam.aspect;
        }
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDrawGizmos.html
    private void OnDrawGizmos()
    {
        // Assigning struct values to camera values. Reciving all the info of the camera to our struct. So, we can save later on.
        if(canUpdate)
        {
            camProp.depth = cam.depth;
            camProp.isOrtho = cam.orthographic;
            camProp.cullingMask = cam.cullingMask;
            camProp.farZ = cam.farClipPlane;
            camProp.nearZ = cam.nearClipPlane;
            camProp.Position = cam.transform.position;
            camProp.Rotation = cam.transform.eulerAngles;
            camProp.fieldOfView = cam.fieldOfView;
            camProp.orthographicSize = cam.orthographicSize;
            camProp.aspect = cam.aspect;
        }
    }

    // Set Camera's world position
    public void SetPosition(Vector3 newPosition)
    {
        canUpdate = false;
        if (cam != null)
            camProp.Position = newPosition;
        else
            Debug.LogError("Camera object is empty.");
        UpdateCamera();
    }

    // Set Camera Depth (Default: 1)
    public void SetDepth(int newDepth)
    {
        canUpdate = false;
        if (cam != null)
            camProp.depth = newDepth;
        else
            Debug.LogError("Camera object is empty.");
        UpdateCamera();
    }

    // Set Near and Far Clip Values
    public void SetAspect(float NearZ, float farZ)
    {
        if (cam != null)
        {
            canUpdate = false;
            camProp.nearZ = NearZ;
            camProp.farZ = farZ;
        }
        else
        {
            Debug.LogError("Camera object is empty.");
        }
        UpdateCamera();
    }

    // Set culling mask
    public void SetCullingMask(int newMask)
    {
        canUpdate = false;
        if (cam != null)
            camProp.cullingMask = newMask;
        else
            Debug.LogError("Camera object is empty.");
        UpdateCamera();
    }

    // Set Camera projection to orthographic
    public void SetCameraToOrtho()
    {
        canUpdate = false;
        if (cam != null)
            camProp.isOrtho = true;
        else
            Debug.LogError("Camera object is empty.");
        UpdateCamera();
    } 

    // Set Camera projection to perspective
    public void SetCameraToPerspective()
    {
        canUpdate = false;
        if (cam != null)
            camProp.isOrtho = false;
        else
            Debug.LogError("Camera object is empty.");
        UpdateCamera();
    }

    // Set FOV Axis (Only works in perspective mode)
    public void SetCameraFOVAxis(float newFOV)
    {
        if (cam != null)
        {
            canUpdate = false;
            if (!camProp.isOrtho)
                camProp.fieldOfView = newFOV;
        }
        else
        {
            Debug.LogError("Camera object is empty.");
        }
        UpdateCamera();
    }
 
    // Set new Camera object
    public void SetCamera(Camera newCamera)
    {
        canUpdate = false;
        if (cam != null)
            cam = newCamera;
        else
            Debug.LogError("Camera object is empty.");
        UpdateCamera();
    }

    // Zoom the camera (Works in both projections) 
    public void IncreaseZoom()
    {
        if(cam != null)
        {
            canUpdate = false;
            if(!camProp.isOrtho)
            {
                camProp.fieldOfView = Mathf.MoveTowards(camProp.fieldOfView, minFOVZoom, 0.5f * Time.deltaTime);
            }
            else
            {
                if(camProp.orthographicSize > 0.0f)
                {
                    tempZoom = cam.orthographicSize;
                    camProp.orthographicSize = Mathf.MoveTowards(camProp.orthographicSize, minOrthoZoom, 0.5f * Time.deltaTime);
                }
            }
        }
        else
        {
            Debug.LogError("Camera object is empty.");
        }
        UpdateCamera();
    }

    // Zoom the camera (Works in both projections) 
    public void DecreaseZoom()
    {
        if (cam != null)
        {
            canUpdate = false;
            if (!camProp.isOrtho)
            {
                camProp.fieldOfView = Mathf.MoveTowards(camProp.fieldOfView, maxFOVZoom, 0.5f * Time.deltaTime);
            }
            else
            {
                if (camProp.orthographicSize > 0.0f)
                {
                    tempZoom = cam.orthographicSize;
                    camProp.orthographicSize = Mathf.MoveTowards(camProp.orthographicSize, maxOrthoZoom, 0.5f * Time.deltaTime);
                }
            }
        }
        else
        {
            Debug.LogError("Camera object is empty.");
        }
        UpdateCamera();
    }

    // Reset Camera Zoom 
    public void ResetZoom()
    {
        if(cam)
        {
            if (!camProp.isOrtho)
            {
                camProp.fieldOfView = 90;
            }
            else
            {
                cam.orthographicSize = tempZoom;
            }
        }
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
            return camProp.Position;
        else
            Debug.LogError("Camera object is empty.");
        return Vector3.zero;
    }

    // Get Camera Rotation
    public Vector3 GetCameraRotation()
    {
        if (cam != null)
            return camProp.Rotation;
        else
            Debug.LogError("Camera object is empty.");
        return Vector3.zero;
    }

    // Get Condition if its ortho or not
    public bool IsOrtho()
    {
        if (cam != null)
            return camProp.isOrtho;
        else
            Debug.LogError("Camera object is empty.");
        return false;
    }
    
    // Get Ortho Size (only works in Orthographic mode)
    public float GetOrthoSize()
    {
        if (cam != null)
            return camProp.orthographicSize;
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
            return camProp.aspect;
        else
            Debug.LogError("Camera object is empty.");
        return 0.0f;
    }

    // Get Field of View (FOV)
    public float GetFieldOfView()
    {
        if (cam != null)
            return camProp.fieldOfView;
        else
            Debug.LogError("Camera object is empty.");
        return 0.0f;
    }

    // Get Culling Mask
    public int GetCullingMask()
    {
        if (cam != null)
            return camProp.cullingMask;
        else
            Debug.LogError("Camera object is empty.");
        return -1;
    }

    // Calculate and Get Aspect Ratio
    public string GetAspectRatio()
    {
        string AspectRatio = string.Format("16:9");
        // Aspect Ratio Calculation
        if (camProp.aspect >= 1.7f)
            AspectRatio = string.Format("16:9");
        else if (cam.aspect > 1.6f)
            AspectRatio = string.Format("5:3");
        else if (cam.aspect >= 1.5f)
            AspectRatio = string.Format("16:10");
        else
            AspectRatio = string.Format("4:3");

        return AspectRatio;
    }

    // Update Camera Properties
    public void UpdateCamera()
    {
        // Assign camera values to strcut values
        cam.aspect = GetAspect();
        cam.fieldOfView = GetFieldOfView();
        cam.orthographicSize = GetOrthoSize();
        cam.orthographic = IsOrtho();
        cam.cullingMask = GetCullingMask();
        cam.depth = camProp.depth;
        cam.farClipPlane = camProp.farZ;
        cam.nearClipPlane = camProp.nearZ;
        cam.transform.position = camProp.Position;
        cam.transform.eulerAngles = camProp.Rotation;
        canUpdate = true;
    }
}

/*
    // Editor class to write notes or values on to the script inspector
    // See: https://docs.unity3d.com/ScriptReference/SystemInfo.html
*/
#if UNITY_EDITOR
[CustomEditor(typeof(CameraInfo))]
[CanEditMultipleObjects]
public class CameraInfoEditor : Editor
{
    // Doc: https://docs.unity3d.com/ScriptReference/Editor.OnInspectorGUI.html
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CameraInfo myCamera = (CameraInfo)target;

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.richText = true;

        string Size = string.Format("{0:0.00}", myCamera.GetOrthoSize());
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
        EditorGUILayout.LabelField("Aspect Ratio", string.Format("{0}", myCamera.GetAspectRatio()), style);

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
        
        // Current Operating System
        EditorGUILayout.LabelField("Operating System", SystemInfo.operatingSystem.ToString(), style);

        // GPU Device Name
        EditorGUILayout.LabelField("GPU", SystemInfo.graphicsDeviceName.ToString(), style);
        
        // GPU Memory Size
        EditorGUILayout.LabelField("GPU Memory Size", SystemInfo.graphicsMemorySize.ToString() + " MB", style);
        
        // GPU Driver Version
        EditorGUILayout.LabelField("Driver Version", SystemInfo.graphicsDeviceVersion.ToString(), style);
        
        // Current Graphics API
        EditorGUILayout.LabelField("Graphics API", SystemInfo.graphicsDeviceType.ToString(), style);

        // Processor Type Name
        EditorGUILayout.LabelField("CPU", SystemInfo.processorType.ToString(), style);

        // System Memory
        EditorGUILayout.LabelField("System Memory", SystemInfo.systemMemorySize.ToString() + " MB",  style);

        // Apply Changes
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

