using Entitas;
using UnityEngine;
using KMath;
using Enums.Tile;

namespace Action
{
    public class RemoveTileToolAction : ActionBase
    {
        public RemoveTileToolAction(Contexts entitasContext, int actionID, int agentID) : base(entitasContext, actionID, agentID)
        {
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = (int)worldPosition.x;
            int y = (int)worldPosition.y;

            if (x >= 0 && x < planet.TileMap.MapSize.X &&
            y >= 0 && y < planet.TileMap.MapSize.Y)
            {
                planet.TileMap.RemoveTile(x, y, MapLayerType.Front);
            }

            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
        }
    }

    // Factory Method
    public class RemoveTileActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID, int agentID)
        {
            return new RemoveTileToolAction(entitasContext, actionID, agentID);
        }
    }
}
