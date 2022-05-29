using UnityEngine;
using Enums;

public class InputManager : MonoBehaviour
{
    // Key struct to keep key settings
    public struct Key
    {
        public KeyCode keyCode;
        public eKeyEvent keyEvent;
        public string keyName;
    }

    // Input Device
    private eInputDevice inputDevice;

    // Player State
    private PlayerState playerState;

    // Currently Active Key
    public Key activeKey;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
    void Awake()
    {
        //Check if Scene has SceneManager setup
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.Register(this, SceneObjectType.SceneObjectTypeUtilityScript);
        }
    }

    // Event: On Key Pressed
    private void OnKeyPressed()
    {
        if(playerState == PlayerState.Pedestrian)
        {
            // Increase Zoom with +
            if (activeKey.keyName == KeyCode.KeypadPlus.ToString())
            {
                PixelPerfectCameraTestTool pixelCam = Camera.main.GetComponent<PixelPerfectCameraTestTool>();
                if(pixelCam.targetCameraHalfWidth < 15.0f)
                    pixelCam.targetCameraHalfWidth += 1.0f;
                // Update Zoomed ortho pixel perfect calculation
                pixelCam.adjustCameraFOV();
            }

            // Decrease zoom with -
            if (activeKey.keyName == KeyCode.KeypadMinus.ToString())
            {
                PixelPerfectCameraTestTool pixelCam = Camera.main.GetComponent<PixelPerfectCameraTestTool>();
                if(pixelCam.targetCameraHalfWidth > 1.5f)
                    pixelCam.targetCameraHalfWidth -= 1.0f;
                // Update Zoomed ortho pixel perfect calculation
                pixelCam.adjustCameraFOV();
            }
        }
        else if(playerState == PlayerState.Vehicle)
        {

        }
    }

    // Event: On Key Released
    private void OnKeyReleased()
    {
        if(playerState == PlayerState.Pedestrian)
        {

        }
        else if(playerState == PlayerState.Vehicle)
        {

        }
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html
    private void FixedUpdate()
    {
        // Detect Key Call
        DetectKey();
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

    // Detect which key player pressing
    private void DetectKey()
    {
        // Check input device is empty
        if (inputDevice == eInputDevice.Invalid)
            return;

        // Getting active keycode from unity system.enum
        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            // Assign active key to local activeKey and key event to local key event
            if (Input.GetKey(vKey))
            {
                activeKey.keyCode = vKey;
                activeKey.keyName = vKey.ToString();
                activeKey.keyEvent = eKeyEvent.Press;
                OnKeyPressed();
            }
            else if (Input.GetKeyUp(vKey))
            {
                activeKey.keyEvent = eKeyEvent.Release;
                OnKeyReleased();
                activeKey.keyName = "";
                activeKey.keyCode = KeyCode.None;
            }
        }
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html
    private void OnGUI()
    {
        // Detect Input device to understand which device player using.
        DetectInputDevice();
    }

    // Detecting Input device from input actions
    private eInputDevice DetectInputDevice()
    {
        // If any mouse or keyboard key detected, set input device to keyboard+mouse
        if(Event.current.isKey ||
            Event.current.isMouse)
        {
            return eInputDevice.KeyboardMouse;
        }

        // If any mouse hover event detected, set input device to keyboard+mouse
        if (Input.GetAxis("Mouse X") != 0.0f ||
            Input.GetAxis("Mouse Y") != 0.0f)
        {
            return eInputDevice.KeyboardMouse;
        }

        // Else, return none device.
        return eInputDevice.Invalid;
    }
}

