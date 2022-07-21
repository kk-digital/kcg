using System;
using UnityEngine;
using Enums.Tile;
using PlanetTileMap;

namespace Action
{
    public class ToolActionPlaceTile : ActionBase
    {
        public struct Data
        {
            public TileMaterialType MaterialType;
            public MapLayerType Layer;
        }

        Data data;

        public ToolActionPlaceTile(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
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
                    planet.TileMap.SetBackTile(x, y, data.MaterialType);
                    break;
                case MapLayerType.Mid:
                    planet.TileMap.SetMidTile(x, y, data.MaterialType);
                    break;
                case MapLayerType.Front:
                    planet.TileMap.SetFrontTile(x, y, data.MaterialType);
                    break;
            }
            }

            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
        }
    }

    // Factory Method
    public class ToolActionPlaceTileCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID)
        {
            return new ToolActionPlaceTile(entitasContext, actionID);
        }
    }
}
