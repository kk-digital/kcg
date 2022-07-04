using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AddSelectionSystem : ReactiveSystem<GameEntity>
{
    private readonly Contexts _contexts;
    readonly IGroup<GameEntity> _grids;
    GameEntity entityUnderPointer;

    public AddSelectionSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _grids = contexts.game.GetGroup(GameMatcher.Grid); // grid
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.PointerClicked.Added());
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameEntity> entities)
    {

        
        foreach (var e in entities)
        {
            foreach (var grid in _grids)
            {
                entityUnderPointer = null;
                var position = e.pointerPosition.value.ToGridPosition();

                var gridSize = grid.grid.value;

                var xOffset = (int)(grid.position.value.x - (gridSize.x / 2));
                var yOffset = (int)(grid.position.value.y - (gridSize.y / 2));

                var horizontalBounded = position.x >= 0 + xOffset && position.x < gridSize.x + xOffset;
                var verticalBounded = position.y >= 0 + yOffset && position.y < gridSize.y + yOffset;

                if (horizontalBounded && verticalBounded)
                {

                    foreach (var entitesPosition in _contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.Position, GameMatcher.SpaceStation)).GetEntities())
                    {
                        if (entitesPosition.position.value == position.ToVector3())
                        {
                            entityUnderPointer = entitesPosition;
                        }
                    }

                    if (entityUnderPointer == null)
                    {
                        var newbuilding = _contexts.game.CreateEntity();

                        newbuilding.AddPosition(position.ToVector3());
                        newbuilding.isSpaceStation = true;
                        newbuilding.AddResource("Prefabs/SpaceStation");
                        newbuilding.AddHealth(100);
                    }
                    else
                    {
                        Debug.Log("can't build here");
                    }


                }
            }

        }
            
    }

        

       

     
}