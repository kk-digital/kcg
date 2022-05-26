using UnityEngine;
using Enums;

public class InputManager : MonoBehaviour
{
    // Key struct to keep key settings
    struct Key
    {
        public KeyCode keyCode;
        public eKeyEvent keyEvent;
        public string keyName;
    }

    // Input Device
    private eInputDevice inputDevice;

    // Currently Active Key
    private Key activeKey;

    void Awake()
    {
        //Check if Scene has SceneManager setup
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.Register(this, SceneObjectType.SceneObjectTypeUtilityScript);
        }
    }
    private void KeyEvents()
    {
        // All key events goes here

        if(activeKey.keyName == KeyCode.KeypadPlus.ToString())
        {
            CameraInfo camInfo = Camera.main.GetComponent<CameraInfo>();
            camInfo.IncreaseZoom();
        }

        if(activeKey.keyName == KeyCode.KeypadMinus.ToString())
        {
            CameraInfo camInfo = Camera.main.GetComponent<CameraInfo>();
            camInfo.DecreaseZoom();
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
                KeyEvents();
            }
            else
            {
                activeKey.keyEvent = eKeyEvent.Release;
                activeKey.keyName = "";
                activeKey.keyCode = KeyCode.None;
                KeyEvents();
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

        return eInputDevice.Invalid;
    }
}
