using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Enums;

public class InputManager : MonoBehaviour
{
    private pInputDevice inputDevice = pInputDevice.Invalid;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    void Start()
    {
        
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnGUI.html
    private void OnGUI()
    {
        // Detect Input device to understand which device player using.
        DetectInputDevice();
    }

    private pInputDevice DetectInputDevice()
    {
        // If any mouse or keyboard key detected, set input device to keyboard+mouse
        if(Event.current.isKey ||
            Event.current.isMouse)
        {
            return pInputDevice.KeyboardMouse;
        }

        // If any mouse hover event detected, set input device to keyboard+mouse
        if (Input.GetAxis("Mouse X") != 0.0f ||
            Input.GetAxis("Mouse Y") != 0.0f)
        {
            return pInputDevice.KeyboardMouse;
        }

        return pInputDevice.Invalid;
    }
}
