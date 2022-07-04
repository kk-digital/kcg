
public sealed class RootSystems : Feature
{
    public RootSystems(Contexts contexts)
    {
        //int
        Add(new CreateGameSystem(contexts));
        Add(new TickingSystem(contexts));
        Add(new CreatPlayerSystem(contexts));
        Add(new CreatSpaceStationSystem(contexts));
        Add(new FillAllElementsSystem(contexts));
        Add(new CreateHoverPointerSystem(contexts));

     //   Add(new GridSystem(contexts));

        //User Input
        Add(new ShipCollisionSystem(contexts));
        Add(new PlayerInput(contexts));
        Add(new UpdateInputPointerSystem(contexts));





        //Update
        Add(new UpdateHoverPointerSystem(contexts));
        Add(new PositionUpdateSystem(contexts));
        Add(new RenderForceSystem(contexts));
        
        Add(new BulletCollisionSystem(contexts));
        Add(new AddViewSystem(contexts));

        Add(new LevelStartingSystem(contexts));

        Add(new ShipControlsSystem(contexts));
        Add(new AddRigidbodySystem(contexts));

        Add(new AgingSystem(contexts));
        Add(new MaxAgeSystem(contexts));
        Add(new AsteroidSplittingSystem(contexts));

        Add(new RenderRotationSystem(contexts));

        Add(new AddSelectionSystem(contexts));

        Add(new GunFiringSystem(contexts));
        Add(new GunCooldownSystem(contexts));

        Add(new LevelEndingSystem(contexts));
        Add(new ShipRespawningSystem(contexts));
        

        Add(new BoundsWrappingSystem(contexts));
        Add(new RenderPositionSystem(contexts));

        Add(new LogHealthSystem(contexts));
        Add(new HealthSystem(contexts));
       




        //View / Render

        //Cleanup
        Add(new InputDestructionSystem(contexts));
        Add(new PointerClickedDestructionSystem(contexts));
        Add(new RemoveViewSystem(contexts));
        Add(new DestroyEntitySystem(contexts));

       // Add(new GunController(contexts));


    }
}
