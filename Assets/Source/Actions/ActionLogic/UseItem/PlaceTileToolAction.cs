using System;
using UnityEngine;
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

        public PlaceTileToolAction(Contexts entitasContext, int actionID, int agentID) : base(entitasContext, actionID, agentID)
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
            switch (data.Layer)
            {
                case MapLayerType.Back:
                    planet.TileMap.SetBackTile(x, y, data.TileID);
                    break;
                case MapLayerType.Mid:
                    planet.TileMap.SetMidTile(x, y, data.TileID);
                    break;
                case MapLayerType.Front:
                    planet.TileMap.SetFrontTile(x, y, data.TileID);
                    break;
            }
            }

            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
        }
    }

    // Factory Method
    public class PlaceTileActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID, int agentID)
        {
            return new PlaceTileToolAction(entitasContext, actionID, agentID);
        }
    }
}
