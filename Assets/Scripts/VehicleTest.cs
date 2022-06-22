using Entitas;
using KMath;
using Physics;
using UnityEngine;

public class VehicleTest : MonoBehaviour
{
    // Vehilce Draw System
    Vehicle.DrawSystem vehicleDrawSystem;

    // Vehicle Collision System
    Vehicle.ProcessCollisionSystem vehicleCollisionSystem;

    // Vehicle Spawner System
    Vehicle.SpawnerSystem vehicleSpawnerSystem;

    // Vehicle Physics
    public Vehicle.ProcessVelocitySystem vehiclePhysics;

    // Rendering Material
    [SerializeField]
    Material Material;

    public bool canUpdateGravity = true;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {
        // Initialize Vehicle Spawner System
        vehicleSpawnerSystem = new Vehicle.SpawnerSystem();

        // Initialize Vehicle Physics System
        vehiclePhysics = new Vehicle.ProcessVelocitySystem();

        // Initialize Vehicle Draw System
        vehicleDrawSystem = new Vehicle.DrawSystem();

        // Initialize Vehicle Collision System
        vehicleCollisionSystem = new Vehicle.ProcessCollisionSystem();

        // Initialize Image
        int image = GameState.SpriteLoader.GetSpriteSheetID("Assets\\StreamingAssets\\assets\\luis\\vehicles\\Speeder_chassis.png", 128, 96);

        // Loading Image
        vehicleSpawnerSystem.SpawnVehicle(Material, image, 128, 96, new Vec2f(-5.0f, 0));
    }
    
    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
        // check if the sprite atlas textures needs to be updated
        for(int type = 0; type < GameState.SpriteAtlasManager.Length; type++)
        {
            GameState.SpriteAtlasManager.UpdateAtlasTexture(type);
        }

        // check if the tile sprite atlas textures needs to be updated
        for(int type = 0; type < GameState.TileSpriteAtlasManager.Length; type++)
        {
            GameState.TileSpriteAtlasManager.UpdateAtlasTexture(type);
        }

        // Clear last frame
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
            if (Application.isPlaying)
                Destroy(mr.gameObject);
            else
                DestroyImmediate(mr.gameObject);

        // Update Gravity
        if (canUpdateGravity)
          vehiclePhysics.UpdateGravity(Contexts.sharedInstance);

        // Update Collision Physics
        vehicleCollisionSystem.Update(tileMap);

        // Draw Vehicle
        vehicleDrawSystem.Draw(Instantiate(Material), transform, 17);

        Controls();
    }

    private GameEntity vehicleEntity;

    void Controls()
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

            vehiclePhysics.ProcessMovement(new Vec2f(velocity, vehicleEntity.vehiclePhysicsState2D.angularVelocity.Y), Contexts.sharedInstance);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            // Get Vehicle Entites
            IGroup<GameEntity> entities =
                Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
            foreach (var vehicle in entities)
            {
                vehicleEntity = vehicle;

                StartCoroutine(vehiclePhysics.Break(true, vehicleEntity.vehiclePhysicsState2D.angularVelocity, Contexts.sharedInstance));
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

            vehiclePhysics.ProcessMovement(new Vec2f(velocity, vehicleEntity.vehiclePhysicsState2D.angularVelocity.Y), Contexts.sharedInstance);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            // Get Vehicle Entites
            IGroup<GameEntity> entities =
                Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
            foreach (var vehicle in entities)
            {
                vehicleEntity = vehicle;

                StartCoroutine(vehiclePhysics.Break(true, vehicleEntity.vehiclePhysicsState2D.angularVelocity, Contexts.sharedInstance));
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            canUpdateGravity = false;
            // Get Vehicle Entites
            IGroup<GameEntity> entities =
                Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
            foreach (var vehicle in entities)
            {
                vehicleEntity = vehicle;
            }

            float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.Y, 1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);
            vehiclePhysics.ProcessMovement(new Vec2f(vehicleEntity.vehiclePhysicsState2D.angularVelocity.X, velocity), Contexts.sharedInstance);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            canUpdateGravity = true;
            // Get Vehicle Entites
            IGroup<GameEntity> entities =
                Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
            foreach (var vehicle in entities)
            {
                vehicleEntity = vehicle;

                StartCoroutine(vehiclePhysics.Break(false, vehicleEntity.vehiclePhysicsState2D.angularVelocity, Contexts.sharedInstance));
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            canUpdateGravity = false;
            // Get Vehicle Entites
            IGroup<GameEntity> entities =
                Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
            foreach (var vehicle in entities)
            {
                vehicleEntity = vehicle;
            }

            float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.Y, -1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);

            vehiclePhysics.ProcessMovement(new Vec2f(vehicleEntity.vehiclePhysicsState2D.angularVelocity.X, velocity), Contexts.sharedInstance);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            canUpdateGravity = true;
            // Get Vehicle Entites
            IGroup<GameEntity> entities =
                Contexts.sharedInstance.game.GetGroup(GameMatcher.VehiclePhysicsState2D);
            foreach (var vehicle in entities)
            {
                vehicleEntity = vehicle;

                StartCoroutine(vehiclePhysics.Break(false, vehicleEntity.vehiclePhysicsState2D.angularVelocity, Contexts.sharedInstance));
            }
        }
    }
}
