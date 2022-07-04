using Entitas;
using UnityEngine;

public sealed class UpdateHoverPointerSystem : IExecuteSystem
{
    readonly IGroup<GameEntity> _HoverBuildposition;
    readonly IGroup<GameEntity> _grids;


    public UpdateHoverPointerSystem(Contexts contexts)
    {
        _HoverBuildposition = contexts.game.GetGroup(GameMatcher.AllOf(GameMatcher.View, GameMatcher.Pointer, GameMatcher.HoverPointer, GameMatcher.Position));
        _grids = contexts.game.GetGroup(GameMatcher.Grid); // Grid
    }

    public void Execute()
    {
        var hoverPointer = _HoverBuildposition.GetSingleEntity();
        if (hoverPointer == null)
            return;

        var mousePos = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        var objectOffset = hoverPointer.view.gameObject.transform.localScale / 2;
        var hoverObjectPosition = mousePos.ToGridPosition();
        var isInGrid = false;

        foreach (var grid in _grids)
        {
            var gridSize = grid.grid.value;

            var xOffset = (int)(grid.position.value.x - (gridSize.x / 2));
            var yOffset = (int)(grid.position.value.y - (gridSize.y / 2));

            var horizontalBounded = hoverObjectPosition.x >= 0 + xOffset && hoverObjectPosition.x < gridSize.x + xOffset;
            var verticalBounded = hoverObjectPosition.y >= 0 + yOffset && hoverObjectPosition.y < gridSize.y + yOffset;

            if (horizontalBounded && verticalBounded)
            {
                isInGrid = true;
                break;
            }
        }


        if (isInGrid)
        {
            hoverPointer.view.gameObject.transform.position = hoverObjectPosition.ToVector3();
        }
        else
        {
            hoverPointer.view.gameObject.transform.position = new Vector3(mousePos.x - objectOffset.x, mousePos.y - objectOffset.y);
        }

        var x = hoverPointer.view.gameObject.transform.position.x;
        var y = hoverPointer.view.gameObject.transform.position.y;
        if (x != hoverPointer.position.value.x || y != hoverPointer.position.value.y)
        {
            hoverPointer.ReplacePosition(mousePos);
        }


    }
}
