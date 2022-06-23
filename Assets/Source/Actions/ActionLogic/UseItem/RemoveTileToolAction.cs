using Entitas;
using UnityEngine;
using KMath;
using Enums.Tile;

namespace Action
{
    public class RemoveTileToolAction : ActionBase
    {
        public RemoveTileToolAction(int actionID, int agentID) : base(actionID, agentID)
        {
        }

        public override void OnEnter()
        {
            Planet.TileMap tileMap = ActionAttributeEntity.actionAttributePlanetState.Planet.TileMap;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = (int)worldPosition.x;
            int y = (int)worldPosition.y;
            tileMap.RemoveTile(x, y, MapLayerType.Front);

            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
        }
    }

    // Factory Method
    public class RemoveTileActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(int actionID, int agentID)
        {
            return new RemoveTileToolAction(actionID, agentID);
        }
    }
}
