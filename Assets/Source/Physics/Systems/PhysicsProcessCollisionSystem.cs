using Entitas;
using Enums;
using Enums.Tile;
using KMath;
using UnityEngine;
using Utility;

namespace Physics
{
    //TODO: Collision calculation should internally cache the chunks around player
    //TODO: (up left, up right, bottom left, bottom right) instead of doing GetTile for each tile.
    //TODO: Implement Prediction Movement Collision
    //TODO: Create broad-phase for getting tiles
    // https://www.flipcode.com/archives/Raytracing_Topics_Techniques-Part_4_Spatial_Subdivisions.shtml
    // http://www.cs.yorku.ca/~amana/research/grid.pdf
    public class PhysicsProcessCollisionSystem
    {
        private void Update(ref PlanetTileMap.TileMap tileMap, Position2DComponent pos, MovableComponent movable, Box2DColliderComponent box2DColider, float deltaTime)
{
            var entityBoxBorders = new AABB2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y) + box2DColider.Offset, box2DColider.Size);

            if (entityBoxBorders.IsCollidingBottom(tileMap, movable.Velocity) || entityBoxBorders.IsCollidingTop(tileMap, movable.Velocity))
            {   
                pos.Value = new Vec2f(pos.Value.X, pos.PreviousValue.Y);
                movable.Velocity.Y = 0.0f;
                movable.Acceleration.Y = 0.0f;
            }

            entityBoxBorders = new AABB2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y) + box2DColider.Offset, box2DColider.Size);

            if (entityBoxBorders.IsCollidingLeft(tileMap, movable.Velocity) || entityBoxBorders.IsCollidingRight(tileMap, movable.Velocity))
            {
                pos.Value = new Vec2f(pos.PreviousValue.X, pos.Value.Y);
                movable.Velocity.X = 0.0f;
                movable.Acceleration.X = 0.0f;
            }

            entityBoxBorders.DrawBox();
        }

        public void Update(AgentContext agentContext, ref PlanetTileMap.TileMap tileMap)
        {
            float deltaTime = Time.deltaTime;
            var entitiesWithBox = agentContext.GetGroup(AgentMatcher.AllOf(AgentMatcher.PhysicsBox2DCollider, AgentMatcher.PhysicsPosition2D));

            foreach (var entity in entitiesWithBox)
            {
                var pos = entity.physicsPosition2D;
                var movable = entity.physicsMovable;
                var box2DColider = entity.physicsBox2DCollider;

                Update(ref tileMap, pos, movable, box2DColider, deltaTime); 
            }

        }

        public void Update(ItemContext itemContext, ref PlanetTileMap.TileMap tileMap)
        {
            float deltaTime = Time.deltaTime;
            var entitiesWithBox = itemContext.GetGroup(ItemMatcher.AllOf(ItemMatcher.PhysicsBox2DCollider, ItemMatcher.PhysicsPosition2D));

            foreach (var entity in entitiesWithBox)
            {
                var pos = entity.physicsPosition2D;
                var movable = entity.physicsMovable;
                var box2DColider = entity.physicsBox2DCollider;

                Update(ref tileMap, pos, movable, box2DColider, deltaTime);
            }
        }
    }
}
