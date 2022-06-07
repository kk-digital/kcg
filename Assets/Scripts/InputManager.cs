using UnityEngine;
using Enums;
using Entitas;

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
    private PlayerState playerState = PlayerState.Vehicle;

    // Currently Active Key
    public Key activeKey;

    // Note: This is temporarily
    private Vehicle.ProcessVelocitySystem vehilcePhysics;
    private Contexts contexts;
    private GameEntity vehicleEntity;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
    void Awake()
    {
        //Check if Scene has SceneManager setup
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.Register(this, SceneObjectType.SceneObjectTypeUtilityScript);
        }
        vehilcePhysics = new Vehicle.ProcessVelocitySystem();
        contexts = Contexts.sharedInstance;
    }

    // Event: On Key Pressed
    private void OnKeyPressed()
    {
        if(playerState == PlayerState.Pedestrian)
        {
            // Decrease zoom with -
            if (activeKey.keyName == KeyCode.KeypadMinus.ToString())
            {
                PixelPerfectCameraTestTool pixelCam = Camera.main.GetComponent<PixelPerfectCameraTestTool>();
                if(pixelCam.targetCameraHalfWidth < 15.0f)
                    pixelCam.targetCameraHalfWidth += 1.0f;
                // Update Zoomed ortho pixel perfect calculation
                pixelCam.adjustCameraFOV();
            }

            // Increase Zoom with +
            if (activeKey.keyName == KeyCode.KeypadPlus.ToString())
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
            if(activeKey.keyName == KeyCode.A.ToString())
            { 
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;
                    // Get scale from component
                    vehicle.ReplaceVehiclePhysicsState2D(vehicle.vehiclePhysicsState2D.Position, vehicle.vehiclePhysicsState2D.TempPosition, new Vector2(-vehicle.vehiclePhysicsState2D.Scale.x, vehicle.vehiclePhysicsState2D.Scale.y), vehicle.vehiclePhysicsState2D.Scale, vehicle.vehiclePhysicsState2D.angularVelocity, vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                         vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);
                }

                float velocity = Mathf.SmoothDamp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, 1.0f, ref vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, vehicleEntity.vehiclePhysicsState2D.angularAcceleration);
                vehilcePhysics.ProcessMovementX(velocity, false, contexts);
            }

            if (activeKey.keyName == KeyCode.D.ToString())
            {
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;
                    // Get scale from component
                    vehicle.ReplaceVehiclePhysicsState2D(vehicle.vehiclePhysicsState2D.Position, vehicle.vehiclePhysicsState2D.TempPosition, new Vector2(vehicle.vehiclePhysicsState2D.Scale.x, vehicle.vehiclePhysicsState2D.Scale.y), vehicle.vehiclePhysicsState2D.Scale, vehicle.vehiclePhysicsState2D.angularVelocity, vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                         vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);
                }

                float velocity = Mathf.SmoothDamp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, 1.0f, ref vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, vehicleEntity.vehiclePhysicsState2D.angularAcceleration);
                vehilcePhysics.ProcessMovementX(velocity, true, contexts);
            }
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
            if (activeKey.keyName == KeyCode.A.ToString())
            {
                vehilcePhysics.ProcessMovementX(0.0f, false, contexts);

            }
            if (activeKey.keyName == KeyCode.D.ToString())
            {
                vehilcePhysics.ProcessMovementX(0.0f, true, contexts);
            }
        }
    }

    public void MouseInputs()
    {
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

        MouseInputs();
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

