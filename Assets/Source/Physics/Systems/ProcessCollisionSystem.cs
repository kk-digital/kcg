using Enums;
using Enums.Tile;
using KMath;
using UnityEngine;
using Utility;

namespace Physics
{
    public class ProcessCollisionSystem
    {
        public void Update(Planet.TileMap tileMap)
        {
            float deltaTime = Time.deltaTime;
            var entitiesWithBox = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.PhysicsBox2DCollider, GameMatcher.PhysicsPosition2D));
            var entitiesWithCircle = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.PhysicsSphere2DCollider, GameMatcher.PhysicsPosition2D));

            foreach (var entity in entitiesWithCircle)
            {
                var pos = entity.physicsPosition2D;
                var circleCollider = entity.physicsSphere2DCollider;
                var movable = entity.physicsMovable;



                ref var tile = ref tileMap.GetTileRef(2, 1, MapLayerType.Front);
                if (tile.Type >= 0)
                {
                    var newCircle = new Sphere2D(pos.Value, circleCollider.Radius, circleCollider.Size);
                    var oldCircle = new Sphere2D(pos.PreviousValue, circleCollider.Radius, circleCollider.Size);

                    var direction = oldCircle.GetPointOnEdge(newCircle.Center) - oldCircle.Center;

                    oldCircle.MovingIntersects(tile.Borders, direction, out var time);
                    if (time < 1.0f)
                    {
                        entity.ReplacePhysicsPosition2D(new Vec2f(pos.PreviousValue.Y, pos.PreviousValue.Y), pos.PreviousValue);
                        entity.ReplacePhysicsMovable(movable.Speed, new Vec2f(0.0f, 0.0f), new Vec2f(0.0f, 0.0f));
                    }
                }
            }

            foreach (var entity in entitiesWithBox)
            {
                var pos = entity.physicsPosition2D;
                var entityBoxBorders = new AABB2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y) + entity.physicsBox2DCollider.Offset, entity.physicsBox2DCollider.Size);
                var movable = entity.physicsMovable;
                
                if (entityBoxBorders.IsCollidingBottom(tileMap, movable.Velocity))
                {
                    entity.ReplacePhysicsPosition2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y), pos.PreviousValue);
                    entity.ReplacePhysicsMovable(movable.Speed, new Vec2f(movable.Velocity.X, 0.0f), new Vec2f(movable.Acceleration.X, 0.0f));
                }
                else if (entityBoxBorders.IsCollidingTop(tileMap, movable.Velocity))
                {
                    entity.ReplacePhysicsPosition2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y), pos.PreviousValue);
                    entity.ReplacePhysicsMovable(movable.Speed, new Vec2f(movable.Velocity.X, 0.0f), new Vec2f(movable.Acceleration.X, 0.0f));
                }
                
                pos = entity.physicsPosition2D;
                entityBoxBorders = new AABB2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y) + entity.physicsBox2DCollider.Offset, entity.physicsBox2DCollider.Size);
                movable = entity.physicsMovable;
                
                if (entityBoxBorders.IsCollidingLeft(tileMap, movable.Velocity))
                {
                    entity.ReplacePhysicsPosition2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y), pos.PreviousValue);
                    entity.ReplacePhysicsMovable(movable.Speed, new Vec2f(0.0f, movable.Velocity.Y), new Vec2f(0.0f, movable.Acceleration.Y));
                }
                else if (entityBoxBorders.IsCollidingRight(tileMap, movable.Velocity))
                {
                    entity.ReplacePhysicsPosition2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y), pos.PreviousValue);
                    entity.ReplacePhysicsMovable(movable.Speed, new Vec2f(0.0f, movable.Velocity.Y), new Vec2f(0.0f, movable.Acceleration.Y));
                }
                
                entityBoxBorders.DrawBox();
            }
        }
    }
}
