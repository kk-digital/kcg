using Entitas;
using KMath;
using System.IO;

namespace Action
{
    public class MoveAction : ActionBase
    {
        private ItemParticleEntity ItemParticle;
        Vec2f [] path;
        int pathLength;

        public MoveAction(Contexts entitasContext, int actionID) : base(entitasContext, actionID)
        {
        }

        public override void OnEnter(ref Planet.PlanetState planet)
        {
            Vec2f goalPosition = ActionEntity.actionMoveTo.GoalPosition;

            AI.Movement.PathFinding pathFinding = new AI.Movement.PathFinding();
            pathFinding.Initialize();
            path = pathFinding.getPath(ref planet.TileMap, AgentEntity.physicsPosition2D.Value, goalPosition);

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

            Vec2f direction = AgentEntity.physicsPosition2D.Value - targetPos;

            if (direction.Magnitude < 0.1f)
            {
                if (--pathLength == 0)
                {
                    AgentEntity.physicsMovable.Acceleration = Vec2f.Zero;
                    ActionEntity.actionExecution.State = Enums.ActionState.Success;
                    return;
                }
            }

            // Todo: deals with flying agents.
            direction.Y = 0;
            direction.Normalize();
            AgentEntity.physicsMovable.Acceleration = direction * AgentEntity.physicsMovable.Speed * 25.0f;
            // Todo deals with jumping.
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