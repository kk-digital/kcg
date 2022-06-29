
using KMath;
using System.Web.UI.WebControls;
using Enums.Tile;
using UnityEngine;

namespace Action
{
    public class MiningLaserToolAction : ActionBase
    {
        public MiningLaserToolAction(int actionID, int agentID) : base(actionID, agentID)
        { 
        
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            Vec2f   agentPosition = AgentEntity.physicsPosition2D.Value;
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
                if (cell.x >= 0 && cell.x < planet.TileMap.MapSize.X &&
                    cell.y >= 0 && cell.y < planet.TileMap.MapSize.Y)
                {
                    planet.TileMap.RemoveTile(cell.x, cell.y, MapLayerType.Front);
                    Debug.DrawLine(new Vector3(agentPosition.X, agentPosition.Y, 0.0f), new Vector3(worldPosition.x, worldPosition.y, 0.0f), Color.red);
                }
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
