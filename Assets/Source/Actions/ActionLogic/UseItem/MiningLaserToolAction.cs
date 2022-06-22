
using KMath;
using System.Web.UI.WebControls;
using UnityEngine;

namespace Action
{
    public class MiningLaserToolAction : ActionBase
    {
        public MiningLaserToolAction(int actionID, int agentID) : base(actionID, agentID)
        { 
        
        }

        public override void OnEnter()
        {
            Vec2f   agentPosition = AgentEntity.physicsPosition2D.Value;
            Planet.TileMap tileMap = ActionAttributeEntity.actionAttributePlanetState.Planet.TileMap;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int fromX = (int)agentPosition.X;
            int fromY = (int)agentPosition.Y;

            int toX = (int)worldPosition.x;
            int toY = (int)worldPosition.y;


            Cell start = new Cell
            {
                x = (int)fromX,
                y = (int)fromY
            };

            Cell end = new Cell
            {
                x = (int)toX,
                y = (int)toY
            };

            // Log places drawed line go through
            foreach (var cell in start.LineTo(end))
            {
                Debug.Log($"({cell.x},{cell.y})");

                ref var tile = ref tileMap.GetTileRef(cell.x, cell.y, Enums.Tile.MapLayerType.Front);
                if (tile.Type >= 0)
                {
                    tileMap.RemoveTile(cell.x, cell.y, Enums.Tile.MapLayerType.Front);
                    tileMap.RemoveTile(cell.x, cell.y, Enums.Tile.MapLayerType.Ore);
                }
                    Debug.DrawLine(new Vector3(agentPosition.X, agentPosition.Y, 0.0f),
                             new Vector3(worldPosition.x, worldPosition.y, 0.0f), Color.red);
            }
            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
        }
    }

    // Factory Method
    public class MiningLaserActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(int actionID, int agentID)
        {
            return new MiningLaserToolAction(actionID, agentID);
        }
    }
}
