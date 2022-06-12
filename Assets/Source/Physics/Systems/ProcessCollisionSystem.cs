using UnityEngine;

namespace Physics
{
    public class ProcessCollisionSystem
    {
        public void Update(Planet.TileMap tileMap)
        {
            float deltaTime = Time.deltaTime;
            var entities = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.PhysicsBox2DCollider, GameMatcher.PhysicsPosition2D));


            foreach (var entity in entities)
            {
                var pos = entity.physicsPosition2D;
                var entityBoxBorders = entity.physicsBox2DCollider.CreateEntityBoxBorders(new Vector2(pos.PreviousValue.x, pos.Value.y) + entity.physicsBox2DCollider.Offset);
                var movable = entity.physicsMovable;
                
                if (entityBoxBorders.IsCollidingBottom(tileMap, movable.Velocity))
                {
                    entity.ReplacePhysicsPosition2D(new Vector2(pos.Value.x, pos.PreviousValue.y), pos.PreviousValue);
                    entity.ReplacePhysicsMovable(movable.Speed, new Vector2(movable.Velocity.x, 0.0f), new Vector2(movable.Acceleration.x, 0.0f), movable.AccelerationTime);
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, movable.Velocity))
                {
                    entity.ReplacePhysicsPosition2D(new Vector2(pos.Value.x, pos.PreviousValue.y), pos.PreviousValue);
                    entity.ReplacePhysicsMovable(movable.Speed, new Vector2(movable.Velocity.x, 0.0f), new Vector2(movable.Acceleration.x, 0.0f), movable.AccelerationTime);
                }
                
                pos = entity.physicsPosition2D;
                entityBoxBorders = entity.physicsBox2DCollider.CreateEntityBoxBorders(new Vector2(pos.Value.x, pos.PreviousValue.y) + entity.physicsBox2DCollider.Offset);
                movable = entity.physicsMovable;
                
                if (entityBoxBorders.IsCollidingLeft(tileMap, movable.Velocity))
                {
                    entity.ReplacePhysicsPosition2D(new Vector2(pos.PreviousValue.x, pos.Value.y), pos.PreviousValue);
                    entity.ReplacePhysicsMovable(movable.Speed, new Vector2(0.0f, movable.Velocity.y), new Vector2(0.0f, movable.Acceleration.y), movable.AccelerationTime);
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, movable.Velocity))
                {
                    entity.ReplacePhysicsPosition2D(new Vector2(pos.PreviousValue.x, pos.Value.y), pos.PreviousValue);
                    entity.ReplacePhysicsMovable(movable.Speed, new Vector2(0.0f, movable.Velocity.y), new Vector2(0.0f, movable.Acceleration.y), movable.AccelerationTime);
                }
            }
        }
    }
}
