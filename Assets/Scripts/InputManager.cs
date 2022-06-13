using UnityEngine;
using Entitas;
using KMath;

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
        }
        else if (playerState == Enums.PlayerState.Vehicle)
        {
            if (Input.GetKey(KeyCode.A))
            {
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                    Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;

                    // Get scale from component
                    vehicle.ReplaceVehiclePhysicsState2D(vehicle.vehiclePhysicsState2D.Position, vehicle.vehiclePhysicsState2D.TempPosition, new Vec2f(-vehicle.vehiclePhysicsState2D.Scale.X, vehicle.vehiclePhysicsState2D.Scale.Y), vehicle.vehiclePhysicsState2D.Scale, vehicle.vehiclePhysicsState2D.angularVelocity, vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                         vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);
                }

                float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.X, -1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);

                vehilcePhysics.ProcessMovement(new Vec2f(velocity, vehicleEntity.vehiclePhysicsState2D.angularVelocity.Y), Contexts.sharedInstance);
            }
            else if(Input.GetKeyUp(KeyCode.A))
            {
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                    Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;

                    StartCoroutine(vehilcePhysics.Break(true, vehicleEntity.vehiclePhysicsState2D.angularVelocity, Contexts.sharedInstance));
                }
            }

            if (Input.GetKey(KeyCode.D))
            {
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                    Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;
                    // Get scale from component
                    vehicle.ReplaceVehiclePhysicsState2D(vehicle.vehiclePhysicsState2D.Position, vehicle.vehiclePhysicsState2D.TempPosition, new Vec2f(vehicle.vehiclePhysicsState2D.Scale.X, vehicle.vehiclePhysicsState2D.Scale.Y), vehicle.vehiclePhysicsState2D.Scale, vehicle.vehiclePhysicsState2D.angularVelocity, vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                         vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);
                }

                float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.X, 1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);

                vehilcePhysics.ProcessMovement(new Vec2f(velocity, vehicleEntity.vehiclePhysicsState2D.angularVelocity.Y), Contexts.sharedInstance);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                    Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;

                    StartCoroutine(vehilcePhysics.Break(true, vehicleEntity.vehiclePhysicsState2D.angularVelocity, Contexts.sharedInstance));
                }
            }

            if (Input.GetKey(KeyCode.W))
            {
                vehicleTest.canUpdateGravity = false;
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                    Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;
                }

                float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.Y, 1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);
                vehilcePhysics.ProcessMovement(new Vec2f(vehicleEntity.vehiclePhysicsState2D.angularVelocity.X, velocity), Contexts.sharedInstance);
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                vehicleTest.canUpdateGravity = true;
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                    Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;

                    StartCoroutine(vehilcePhysics.Break(false, vehicleEntity.vehiclePhysicsState2D.angularVelocity, Contexts.sharedInstance));
                }
            }

            if (Input.GetKey(KeyCode.S))
            {
                vehicleTest.canUpdateGravity = false;
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                    Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;
                }

                float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.Y, -1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);

                vehilcePhysics.ProcessMovement(new Vec2f(vehicleEntity.vehiclePhysicsState2D.angularVelocity.X, velocity), Contexts.sharedInstance);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                vehicleTest.canUpdateGravity = true;
                // Get Vehicle Entites
                IGroup<GameEntity> entities =
                    Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
                foreach (var vehicle in entities)
                {
                    vehicleEntity = vehicle;

                    StartCoroutine(vehilcePhysics.Break(false, vehicleEntity.vehiclePhysicsState2D.angularVelocity, Contexts.sharedInstance));
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

