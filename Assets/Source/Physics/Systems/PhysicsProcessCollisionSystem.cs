using System;
using Entitas;
using Enums;
using Enums.Tile;
using KMath;
using PlanetTileMap;
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
        private void PlotLineLow(Vec2i start, Vec2i end)
        {
            Vec2i delta = end - start;

            int y_inc = 1;

            if (delta.Y < 0)
            {
                y_inc = -1;

                delta.Y = -delta.Y;
            }

            var D = 2 * delta.Y - delta.X;
            var y = start.Y;

            for (int x = start.X; x <= end.X; x++)
            {
                // plot(x, y); Output for the position at Line
                if (D > 0)
                {
                    y += y_inc;
                    D += 2 * (delta.Y - delta.X);
                }
                else
                {
                    D += 2 * delta.Y;
                }
            }
        }

        private void PlotLineHigh(Vec2i start, Vec2i end)
        {
            Vec2i delta = end - start;
            
            int x_inc = 1;
            if (delta.X < 0)
            {
                x_inc = -1;
                delta.X = -delta.X;
            }

            var D = 2 * delta.X - delta.Y;
            var x = start.X;

            for (int y = start.Y; y <= end.Y; y++)
            {
                // plot(x, y); Output for the position at Line
                if (D > 0)
                {
                    x += x_inc;
                    D += 2 * (delta.X - delta.Y);

                }
                else
                {
                    D += 2 * delta.X;
                }
            }
        }

        public void PlotLine(Vec2i start, Vec2i end)
        {
            if (System.Math.Abs(end.Y - start.Y) < System.Math.Abs(end.X - start.X))
            {
                if (start.X > end.X)
                {
                    PlotLineLow(end, start);
                }
                else
                {
                    PlotLineLow(start, end);
                }
            }
            else
            {
                if (start.Y > end.Y)
                {
                    PlotLineHigh(end, start);
                }
                else
                {
                    PlotLineHigh(start, end);
                }
            }
        }

        public void IsColliding(ref TileMap tileMap, Vec2f previousCenter, Vec2f currentCenter, Vec2f size, Vec2f velocity)
        {
            var halfSize = size / 2f;

            var previousRightX  = (int)Math.Ceiling(previousCenter.X + halfSize.X); //round up
            var currentRightX   = (int)Math.Ceiling(currentCenter.X + halfSize.X);  //round up
            
            var previousLeftX   = (int)Math.Floor(previousCenter.X - halfSize.X);   //round down
            var currentLeftX    = (int)Math.Floor(currentCenter.X - halfSize.X);    //round down
            
            var previousTopY    = (int)Math.Ceiling(previousCenter.Y + halfSize.Y); //round up
            var currentTopY     = (int)Math.Ceiling(currentCenter.Y + halfSize.Y);  //round up
            
            var previousBottomY = (int)Math.Floor(previousCenter.Y - halfSize.Y);   //round down
            var currentBottomY  = (int)Math.Floor(currentCenter.Y - halfSize.Y);    //round down

            //verify, that no tile is checked twice
            //do a count to verify
            //NOTE: Do asserts, that min<=max and swap if not

            int xMin1 = velocity.X > 0f ? previousRightX : currentLeftX;
            int xMax1 = velocity.X > 0f ? currentRightX  : previousLeftX;
            
            for (int y = currentBottomY; y <= currentTopY; y++)
            {
                for (int x = xMin1; x <= xMax1; x++)
                {
                    var tile = tileMap.GetFrontTile(x, y);

                    if (tile.ID != TileID.Air)
                    {
                        // collision on X
                    }
                }
            }
            
            int yMin1 = velocity.Y > 0f ? previousTopY   : currentBottomY;
            int yMax1 = velocity.Y > 0f ? currentTopY    : previousBottomY;
            
            for (int y = yMin1; y <= yMax1; y++)
            {
                for (int x = currentLeftX; x <= currentRightX; x++)
                {
                    var tile = tileMap.GetFrontTile(x, y);

                    if (tile.ID != TileID.Air)
                    {
                        // collision on Y
                    }
                }
            }
            
            int xMin2 = Math.Min(currentLeftX, previousLeftX);
            int xMax2 = Math.Max(previousRightX, currentRightX);
            
            for (int y = currentBottomY; y <= currentTopY; y++)
            {
                for (int x = xMin2; x <= xMax2; x++)
                {
                    var tile = tileMap.GetFrontTile(x, y);

                    if (tile.ID != TileID.Air)
                    {
                        // collision on X
                    }
                }
            }
            

            int yMin2 = Math.Min(previousBottomY, currentBottomY);
            int yMax2 = Math.Max(previousTopY, currentTopY);
            
            for (int y = yMin2; y <= yMax2; y++)
            {
                for (int x = currentLeftX; x <= currentRightX; x++)
                {
                    var tile = tileMap.GetFrontTile(x, y);

                    if (tile.ID != TileID.Air)
                    {
                        // collision on Y
                    }
                }
            }
        }

        private void Update(ref TileMap tileMap, Position2DComponent pos, MovableComponent movable, Box2DColliderComponent box2DCollider, float deltaTime)
        {       
            var entityBoxBorders = new AABB2D(new Vec2f(pos.PreviousValue.X, pos.Value.Y) + box2DCollider.Offset, box2DCollider.Size);

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

            entityBoxBorders = new AABB2D(new Vec2f(pos.Value.X, pos.PreviousValue.Y) + box2DCollider.Offset, box2DCollider.Size);

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
            var agentEntitiesWithBox = agentContext.GetGroup(AgentMatcher.AllOf(AgentMatcher.PhysicsBox2DCollider, AgentMatcher.PhysicsPosition2D));

            foreach (var agentEntity in agentEntitiesWithBox)
            {
                var pos = agentEntity.physicsPosition2D;
                var movable = agentEntity.physicsMovable;
                var box2DCollider = agentEntity.physicsBox2DCollider;

                Update(ref tileMap, pos, movable, box2DCollider, deltaTime); 
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
