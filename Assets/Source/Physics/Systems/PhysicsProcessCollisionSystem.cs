using Collisions;
using KMath;
using PlanetTileMap;
using UnityEngine;
using Utility;
using Enums.Tile;

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
        private void Update(ref TileMap tileMap, Position2DComponent pos, MovableComponent movable, Box2DColliderComponent box2DCollider, float deltaTime)
        {       
            var entityBoxBorders = new AABox2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y) + box2DCollider.Offset, box2DCollider.Size);

            if (entityBoxBorders.IsCollidingBottom(tileMap, movable.Velocity))
            {
                pos.Value = new Vec2f(pos.Value.X, pos.PreviousValue.Y);
                movable.Velocity.Y = 0.0f;
                movable.Acceleration.Y = 0.0f;
                movable.Landed = true;
            }
            if (entityBoxBorders.IsCollidingTop(tileMap, movable.Velocity))
            {   
                pos.Value = new Vec2f(pos.Value.X, pos.PreviousValue.Y);
                movable.Velocity.Y = 0.0f;
                movable.Acceleration.Y = 0.0f;
            }

            entityBoxBorders = new AABox2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y) + box2DCollider.Offset, box2DCollider.Size);

            if (entityBoxBorders.IsCollidingLeft(tileMap, movable.Velocity))
            {
                pos.Value = new Vec2f(pos.PreviousValue.X, pos.Value.Y);
                movable.Velocity.X = 0.0f;
                movable.Acceleration.X = 0.0f;
                movable.SlidingLeft = true;
            }
            else if (entityBoxBorders.IsCollidingRight(tileMap, movable.Velocity))
            {
                pos.Value = new Vec2f(pos.PreviousValue.X, pos.Value.Y);
                movable.Velocity.X = 0.0f;
                movable.Acceleration.X = 0.0f;
                movable.SlidingRight = true;
            }

            Vec2f position = pos.Value;
            position.X += box2DCollider.Size.X / 2.0f;
            //position.Y -= box2DCollider.Size.Y / 2.0f;

            if ((int)position.X > 0 && (int)position.X + 1 < tileMap.MapSize.X &&
            (int)position.Y > 0 && (int)position.Y < tileMap.MapSize.Y)
            {
                if (tileMap.GetFrontTileID((int)position.X + 1, (int)position.Y)== TileID.Air)
                {
                    movable.SlidingRight = false;
                }
            }

            if ((int)position.X > 0 && (int)position.X - 1 < tileMap.MapSize.X &&
            (int)position.Y > 0 && (int)position.Y < tileMap.MapSize.Y)
            {
                if (tileMap.GetFrontTileID((int)position.X - 1, (int)position.Y) == TileID.Air)
                {
                    movable.SlidingLeft = false;
                }
            }

            entityBoxBorders.DrawBox();
        }

        public void Update(AgentContext agentContext, ref PlanetTileMap.TileMap tileMap)
        {
            float deltaTime = Time.deltaTime;
            var agentEntitiesWithBox = agentContext.GetGroup(AgentMatcher.AllOf(AgentMatcher.PhysicsBox2DCollider, AgentMatcher.PhysicsPosition2D));

            foreach (var agentEntity in agentEntitiesWithBox)
            {
                var pos = agentEntity.physicsPosition2D;
                var movable = agentEntity.physicsMovable;
                var box2DCollider = agentEntity.physicsBox2DCollider;

                Update(ref tileMap, pos, movable, box2DCollider, deltaTime); 
            }

        }

        public void Update(ItemParticleContext itemContext, ref PlanetTileMap.TileMap tileMap)
        {
            float deltaTime = Time.deltaTime;
            var entitiesWithBox = itemContext.GetGroup(ItemParticleMatcher.AllOf(ItemParticleMatcher.PhysicsBox2DCollider, ItemParticleMatcher.PhysicsPosition2D));

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
