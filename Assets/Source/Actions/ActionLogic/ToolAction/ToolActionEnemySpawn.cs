using Entitas;
using UnityEngine;
using KMath;
using Enums.Tile;
using Action;

namespace Action
{
    public class ToolActionEnemySpawn : ActionBase
    {
        // Todo create methods to instatiate Agents.
        // Data should only have something like:
        // struct Data
        // {
        //      Enums.enemyType type
        // }

        public struct Data
        {
            public int CharacterSpriteId;
        }

        Data data;

        public ToolActionEnemySpawn(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
        {
            data = (Data)ActionPropertyEntity.actionPropertyData.Data;
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = worldPosition.x;
            float y = worldPosition.y;
            planet.AddEnemy(new Vec2f(x, y));

            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
        }
    }

    // Factory Method
    public class ToolActionEnemySpawnCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID)
        {
            return new ToolActionEnemySpawn(entitasContext, actionID);
        }
    }
}
