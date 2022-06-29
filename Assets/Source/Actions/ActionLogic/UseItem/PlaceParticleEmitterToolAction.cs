using Entitas;
using UnityEngine;
using KMath;
using Enums.Tile;
using Action;

namespace Action
{
    public class PlaceParticleEmitterToolAction : ActionBase
    {
        // Todo create methods to instatiate Agents.
        // Data should only have something like:
        // struct Data
        // {
        //      Enums.enemyType type
        // }

        public struct Data
        {
            public Material Material;
        }

        Data data;

        public PlaceParticleEmitterToolAction(int actionID, int agentID) : base(actionID, agentID)
        {
            data = (Data)ActionPropertyEntity.actionPropertyData.Data;
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float x = worldPosition.x;
            float y = worldPosition.y;
            int t = System.Math.Abs((int)KMath.Random.Mt19937.genrand_int32() % System.Enum.GetNames(typeof(Particle.ParticleType)).Length);

            Debug.Log((Particle.ParticleEmitterType)t);
            planet.AddParticleEmitter(new Vec2f(x, y), (Particle.ParticleEmitterType)t);

            ActionEntity.ReplaceActionExecution(this, Enums.ActionState.Success);
        }
    }

    // Factory Method
    public class PlaceParticleEmitterActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(int actionID, int agentID)
        {
            return new PlaceParticleEmitterToolAction(actionID, agentID);
        }
    }
}
