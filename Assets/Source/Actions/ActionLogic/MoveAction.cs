using Entitas;
using KMath;
using System.IO;
using UnityEngine;

namespace Action
{
    public class MoveAction : ActionBase
    {
        private ItemParticleEntity ItemParticle;
        Vec2f [] path;
        int pathLength;
        bool availableJump = true;

        public MoveAction(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
        {
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
#if DEBUG
            float deltaTime = Time.realtimeSinceStartup;
#endif
            Vec2f goalPosition = ActionEntity.actionMoveTo.GoalPosition;

            AI.Movement.PathFinding pathFinding = new AI.Movement.PathFinding();
            pathFinding.Initialize();
            path = pathFinding.getPath(ref planet.TileMap, AgentEntity.physicsPosition2D.Value, goalPosition);
#if DEBUG
            deltaTime = (Time.realtimeSinceStartup - deltaTime) * 1000f; // get time and transform to ms.
            Debug.Log("Found time in " + deltaTime.ToString() + "ms");
#endif

            if (path == null)
            {
                ActionEntity.actionExecution.State = Enums.ActionState.Fail;
                return;
            }

            pathLength = path.Length;
            ActionEntity.actionExecution.State = Enums.ActionState.Running;
        }

        public override void OnUpdate(float deltaTime, ref Planet.PlanetState planet)
        {
            Vec2f targetPos = path[pathLength - 1];

            Vec2f direction = targetPos - AgentEntity.physicsPosition2D.Value;

            if (direction.Magnitude < 0.1f)
            {
                if (--pathLength == 0)
                {
                    AgentEntity.physicsMovable.Acceleration = Vec2f.Zero;
                    ActionEntity.actionExecution.State = Enums.ActionState.Success;
                    return;
                }
                // Reset jump if landed.
                if (!availableJump)
                {
                    if (path[pathLength - 1].Y == path[pathLength].Y)
                        availableJump = true;
                }
            }

            // Jumping is just an increase in velocity.
            if (direction.Y > 0 && availableJump)
            {
                availableJump = false;
                AgentEntity.physicsMovable.Acceleration.Y = 0.0f;
                AgentEntity.physicsMovable.Velocity.Y = 7.5f;
            }

            // Todo: deals with flying agents.
            direction.Y = 0;
            direction.Normalize();
            AgentEntity.physicsMovable.Acceleration = direction * AgentEntity.physicsMovable.Speed * 25.0f;
        }

        public override void OnExit(ref Planet.PlanetState planet)
        {
            base.OnExit(ref planet);
        }
    }
    // Factory Method
    public class MoveActionCreator : ActionCreator
    {
        public override ActionBase CreateAction(Contexts entitasContext, int actionID)
        {
            return new MoveAction(entitasContext, actionID);
        }
    }
}