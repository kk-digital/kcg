using UnityEngine;
using Entitas;
using KMath;
using Physics;

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

    // Planet Tile Map
    private Planet.TileMap tileMap;

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

        // Initialize Planet Tile Map
        tileMap = GameObject.Find("TilesTest").GetComponent<Planet.Unity.MapLoaderTestScript>().TileMap;
    }
    
    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void Update()
    {
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
                vehicle.ReplaceVehiclePhysicsState2D(vehicle.vehiclePhysicsState2D.Position, vehicle.vehiclePhysicsState2D.TempPosition, new Vector2(-vehicle.vehiclePhysicsState2D.Scale.x, vehicle.vehiclePhysicsState2D.Scale.y), vehicle.vehiclePhysicsState2D.Scale, vehicle.vehiclePhysicsState2D.angularVelocity, vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                     vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);
            }

            float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, -1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);

            vehiclePhysics.ProcessMovement(new Vector2(velocity, vehicleEntity.vehiclePhysicsState2D.angularVelocity.y), Contexts.sharedInstance);
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
                vehicle.ReplaceVehiclePhysicsState2D(vehicle.vehiclePhysicsState2D.Position, vehicle.vehiclePhysicsState2D.TempPosition, new Vector2(vehicle.vehiclePhysicsState2D.Scale.x, vehicle.vehiclePhysicsState2D.Scale.y), vehicle.vehiclePhysicsState2D.Scale, vehicle.vehiclePhysicsState2D.angularVelocity, vehicle.vehiclePhysicsState2D.angularMass, vehicle.vehiclePhysicsState2D.angularAcceleration,
                     vehicle.vehiclePhysicsState2D.centerOfGravity, vehicle.vehiclePhysicsState2D.centerOfRotation);
            }

            float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, 1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);

            vehiclePhysics.ProcessMovement(new Vector2(velocity, vehicleEntity.vehiclePhysicsState2D.angularVelocity.y), Contexts.sharedInstance);
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

            float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.y, 1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);
            vehiclePhysics.ProcessMovement(new Vector2(vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, velocity), Contexts.sharedInstance);
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

            float velocity = Mathf.Lerp(vehicleEntity.vehiclePhysicsState2D.angularVelocity.y, -1.0f, vehicleEntity.vehiclePhysicsState2D.angularAcceleration * Time.deltaTime);

            vehiclePhysics.ProcessMovement(new Vector2(vehicleEntity.vehiclePhysicsState2D.angularVelocity.x, velocity), Contexts.sharedInstance);
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

    // Draw Gizmos of collider (works only in editor mode)
#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        var group = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.VehiclePhysicsState2D));

        Gizmos.color = Color.green;

        foreach (var entity in group)
        {
            var pos = entity.vehiclePhysicsState2D;
            var boxCollider = entity.physicsBox2DCollider;
            var boxBorders = Box.Create(pos.Position + boxCollider.Offset, boxCollider.Size);
            var center = new UnityEngine.Vector3(boxBorders.Center.X, boxBorders.Center.Y, 0.0f);

            Gizmos.DrawWireCube(center, new Vector3(boxCollider.Size.X, boxCollider.Size.Y, 0.0f));
        }
    }
#endif
}
