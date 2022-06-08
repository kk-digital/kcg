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
    private VehicleTest vehicleTest;
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

        // Set Vehicle Physics
        vehilcePhysics = new Vehicle.ProcessVelocitySystem();

        // Set Vehicle Test Obj
        vehicleTest = GameObject.Find("VehicleTest").GetComponent<VehicleTest>();

        // Set Contexts obj
        contexts = Contexts.sharedInstance;
    }

    public void Controls()
    {
        if (playerState == PlayerState.Pedestrian)
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
        }
        else if (playerState == PlayerState.Vehicle)
        {
            if (Input.GetKey(KeyCode.A))
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

                float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, -1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);

                vehilcePhysics.ProcessMovement(new Vector2(velocity, vehicleEntity.vehiclePhysicsState2D.angularVelocity.y), contexts);
            }
            else if(Input.GetKeyUp(KeyCode.A))
            {
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;

                    StartCoroutine(vehilcePhysics.Break(false, vehicleEntity.vehiclePhysicsState2D.angularVelocity, contexts));
                }
            }

            if (Input.GetKey(KeyCode.D))
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

                float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, 1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);

                vehilcePhysics.ProcessMovement(new Vector2(velocity, vehicleEntity.vehiclePhysicsState2D.angularVelocity.y), contexts);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;

                    StartCoroutine(vehilcePhysics.Break(true, vehicleEntity.vehiclePhysicsState2D.angularVelocity, contexts));
                }
            }

            if (Input.GetKey(KeyCode.W))
            {
                vehicleTest.canUpdateGravity = false;
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;
                }

                float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.y, 1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);
                vehilcePhysics.ProcessMovement(new Vector2(vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, velocity), contexts);
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                vehicleTest.canUpdateGravity = true;
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;

                    StartCoroutine(vehilcePhysics.Break(false, vehicleEntity.vehiclePhysicsState2D.angularVelocity, contexts));
                }
            }

            if (Input.GetKey(KeyCode.S))
            {
                vehicleTest.canUpdateGravity = false;
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;
                }

                float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.y, -1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);

                vehilcePhysics.ProcessMovement(new Vector2(vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, velocity), contexts);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                vehicleTest.canUpdateGravity = true;
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                contexts.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;

                    StartCoroutine(vehilcePhysics.Break(false, vehicleEntity.vehiclePhysicsState2D.angularVelocity, contexts));
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

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html
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

