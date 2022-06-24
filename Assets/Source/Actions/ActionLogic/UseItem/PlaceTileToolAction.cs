

using Entitas;
using UnityEngine;
using KMath;
using Enums.Tile;

namespace Action
{
    public class PlaceTileToolAction : ActionBase
    {
        public struct Data
        {
            public TileID TileID;
        }

        Data data;

        public PlaceTileToolAction(int actionID, int agentID) : base(actionID, agentID)
        {
            data = (Data)ActionPropertyEntity.actionPropertyData.Data;
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = (int)worldPosition.x;
            int y = (int)worldPosition.y;
            planet.TileMap.SetTile(x, y, data.TileID, MapLayerType.Front);
            
            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
        }
    }

    // Factory Method
    public class PlaceTileActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(int actionID, int agentID)
        {
            return new PlaceTileToolAction(actionID, agentID);
        }
    }
}
