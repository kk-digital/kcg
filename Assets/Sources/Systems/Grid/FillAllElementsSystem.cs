using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class FillAllElementsSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    private readonly Contexts _contexts;
    readonly IGroup<GameEntity> _grids;


    public FillAllElementsSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _grids = contexts.game.GetGroup(GameMatcher.Grid); // grid
            

    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.Grid.Added()); //grid
    }

    protected override bool Filter(GameEntity entity)
    {
        return true;
    }

    protected override void Execute(List<GameEntity> entities)
    {
      //  Fill();

        foreach (var entity in entities)
        {
          //  entity.isDestroyed = true;
        }
    }

    public void Initialize()
    {
        Fill();
    }

    private void Fill()
    {

        foreach (var e in _grids.GetEntities())
        {
            var size = e.grid.value;

            var xOffset = (int)(e.position.value.x - (size.x / 2));
            var yOffset = (int)(e.position.value.y - (size.y / 2));


            for (int row = 0 + yOffset; row < size.y + yOffset; row++)
            {
                for (int column = 0 + xOffset; column < size.x + xOffset; column++)
                {
                    Debug.DrawLine(new GridPosition(column, row).ToVector3(), new GridPosition(column, row + 1).ToVector3(), Color.white, 100f);
                    Debug.DrawLine(new GridPosition(column, row).ToVector3(), new GridPosition(column + 1, row).ToVector3(), Color.white, 100f);
                }
            }
            Debug.DrawLine(new GridPosition(0 + xOffset, size.y + yOffset).ToVector3(), new GridPosition(size.x + xOffset, size.y + yOffset).ToVector3(), Color.white, 100f);
            Debug.DrawLine(new GridPosition(size.x + xOffset, 0 + yOffset).ToVector3(), new GridPosition(size.x + xOffset, size.y + yOffset).ToVector3(), Color.white, 100f);
        }

    }
}