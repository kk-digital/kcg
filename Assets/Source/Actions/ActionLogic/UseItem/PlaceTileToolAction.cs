

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
            public MapLayerType Layer;
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
            
            if (x >= 0 && x < planet.TileMap.MapSize.X &&
               y >= 0 && y < planet.TileMap.MapSize.Y)
            {
                planet.TileMap.SetTile(x, y, data.TileID, data.Layer);
            }
            
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
