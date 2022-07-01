using UnityEngine;
using Entitas;

public class InputManager : MonoBehaviour
{
    // Key struct to keep key settings
    public struct Key
    {
        public KeyCode keyCode;
        public Enums.eKeyEvent keyEvent;
        public string keyName;
    }

    // Input Device
    private Enums.eInputDevice inputDevice;

    // Player State
    private Enums.PlayerState playerState = Enums.PlayerState.Pedestrian;

    // Currently Active Key
    public Key activeKey;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
    void Awake()
    {
        //Check if Scene has SceneManager setup
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.Register(this, Enums.SceneObjectType.SceneObjectTypeUtilityScript);
        }

        if (Camera.main.gameObject.GetComponent<CameraMove>().enabled == true)
        {
            if (Camera.main.gameObject.GetComponent<CameraFollow>() != null)
                Camera.main.gameObject.GetComponent<CameraFollow>().enabled = false;
            else
                Camera.main.gameObject.AddComponent<CameraFollow>();
        }
    }

    public void Controls()
    {
        if (playerState == Enums.PlayerState.Pedestrian)
        {
            // Decrease zoom with -
            if (Input.GetKey(KeyCode.KeypadMinus))
            {
                PixelPerfectCameraTestTool pixelCam = Camera.main.GetComponent<PixelPerfectCameraTestTool>();
                if (pixelCam.targetCameraHalfWidth < 15.0f)
                    pixelCam.targetCameraHalfWidth += 1.0f;
                // Update Zoomed ortho pixel perfect calculation
                pixelCam.adjustCameraFOV();
            }

            // Increase Zoom with +
            if (Input.GetKey(KeyCode.KeypadPlus))
            {
                PixelPerfectCameraTestTool pixelCam = Camera.main.GetComponent<PixelPerfectCameraTestTool>();
                if (pixelCam.targetCameraHalfWidth > 1.5f)
                    pixelCam.targetCameraHalfWidth -= 1.0f;
                // Update Zoomed ortho pixel perfect calculation
                pixelCam.adjustCameraFOV();
            }

            if (Input.mouseScrollDelta.y > 0)
            {
                PixelPerfectCameraTestTool pixelCam = Camera.main.GetComponent<PixelPerfectCameraTestTool>();
                if (pixelCam.targetCameraHalfWidth > 1.5f)
                    pixelCam.targetCameraHalfWidth -= 1.0f;
                // Update Zoomed ortho pixel perfect calculation
                pixelCam.adjustCameraFOV();
            }
            else if (Input.mouseScrollDelta.y < -0.5f)
            {
                PixelPerfectCameraTestTool pixelCam = Camera.main.GetComponent<PixelPerfectCameraTestTool>();
                if (pixelCam.targetCameraHalfWidth < 15.0f)
                    pixelCam.targetCameraHalfWidth += 1.0f;
                // Update Zoomed ortho pixel perfect calculation
                pixelCam.adjustCameraFOV();
            }

            // Open or close statistics
            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (KGUI.Statistics.StatisticsDisplay.canDraw)
                    KGUI.Statistics.StatisticsDisplay.canDraw = false;
                else if (!KGUI.Statistics.StatisticsDisplay.canDraw)
                    KGUI.Statistics.StatisticsDisplay.canDraw = true;

            }

            if(Input.GetKeyDown(KeyCode.LeftAlt))
            {
                if(Camera.main.gameObject.GetComponent<CameraFollow>().enabled == true)
                {
                    Camera.main.gameObject.GetComponent<CameraFollow>().enabled = false;
                    Camera.main.gameObject.GetComponent<CameraMove>().enabled = true;
                }
                else
                {
                    Camera.main.gameObject.GetComponent<CameraFollow>().enabled = true;
                    Camera.main.gameObject.GetComponent<CameraMove>().enabled = false;
                }
            }
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            PixelPerfectCameraTestTool pixelCam = Camera.main.GetComponent<PixelPerfectCameraTestTool>();
            if (pixelCam.targetCameraHalfWidth > 1.5f)
                pixelCam.targetCameraHalfWidth -= 1.0f;
            // Update Zoomed ortho pixel perfect calculation
            pixelCam.adjustCameraFOV();
        }
        else if (Input.mouseScrollDelta.y < -0.5f)
        {
            PixelPerfectCameraTestTool pixelCam = Camera.main.GetComponent<PixelPerfectCameraTestTool>();
            if (pixelCam.targetCameraHalfWidth < 15.0f)
                pixelCam.targetCameraHalfWidth += 1.0f;
            // Update Zoomed ortho pixel perfect calculation
            pixelCam.adjustCameraFOV();
        }
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        // Detect Key Call
        //DetectKey();

        Controls();
    }

    // Returns if referenced key pressed or not
    public bool IsKeyPressed(KeyCode key)
    {
        if(activeKey.ToString() == key.ToString())
        {
            return true;
        }
        return false;
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html
    private void OnGUI()
    {
        // Detect Input device to understand which device player using.
        DetectInputDevice();

        if (GameObject.Find("Test") != null)
            KGUI.Statistics.StatisticsDisplay.DrawStatistics(GameObject.Find("Test").GetComponent<Planet.Unity.PlanetTest>().Planet);
    }

    // Detecting Input device from input actions
    private Enums.eInputDevice DetectInputDevice()
    {
        // If any mouse or keyboard key detected, set input device to keyboard+mouse
        if(Event.current.isKey ||
            Event.current.isMouse)
        {
            return Enums.eInputDevice.KeyboardMouse;
        }

        // If any mouse hover event detected, set input device to keyboard+mouse
        if (Input.GetAxis("Mouse X") != 0.0f ||
            Input.GetAxis("Mouse Y") != 0.0f)
        {
            return Enums.eInputDevice.KeyboardMouse;
        }

        // Else, return none device.
        return Enums.eInputDevice.Invalid;
    }
}