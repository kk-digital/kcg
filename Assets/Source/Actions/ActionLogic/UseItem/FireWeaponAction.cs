
using KMath;
using System.Web.UI.WebControls;
using UnityEngine;

namespace Action
{
    public class FireWeaponAction : ActionBase
    {
        public FireWeaponAction(int actionID, int agentID) : base(actionID, agentID)
        {

        }

        public override void OnEnter()
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = worldPosition.x;
            float y = worldPosition.y;

            // Start positiom
            Vec2f startPos = AgentEntity.physicsPosition2D.Value + AgentEntity.agentSprite2D.Size / 2f;

            //GameState.ProjectileSpawnerSystem.SpawnProjectile(GameResources.OreSpriteSheet, 0.2, 0.2, startPos);
        }
    }

    // Factory Method
    public class FireWeaponActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(int actionID, int agentID)
        {
            return new MiningLaserToolAction(actionID, agentID);
        }
    }
}

