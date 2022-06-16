using Enums;
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
            var entitiesWithCircle = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.PhysicsCircle2DCollider, GameMatcher.PhysicsPosition2D));

            foreach (var entity in entitiesWithCircle)
            {
                var pos = entity.physicsPosition2D;
                var radius = entity.physicsCircle2DCollider.Radius;
                var movable = entity.physicsMovable;

                var previousCircle = Circle.Create(pos.PreviousValue, radius, entity.agentSprite2D.Size);
                var directionType = Circle.GetQuarterType(pos.Value - pos.PreviousValue);
                {
                    var newCircle = Circle.Create(new Vec2f(pos.Value.X, pos.PreviousValue.Y), radius, entity.agentSprite2D.Size);
                    var quarters = previousCircle.GetTileCollisionQuarters(newCircle, tileMap);

                    if (quarters != CircleQuarter.Error)
                    {
                        Flag.UnsetFlag(ref directionType, quarters);

                        var positionProcess = new Vec2f(pos.PreviousValue.X, pos.Value.Y);
                        var velocityProcess = new Vec2f(0.0f, movable.Velocity.Y);
                        var accelerationProcess = new Vec2f(0.0f, movable.Acceleration.Y);

                        if (directionType.HasFlag(CircleQuarter.Left) && !quarters.HasFlag(CircleQuarter.LeftBottom) && !quarters.HasFlag(CircleQuarter.LeftTop) || 
                            directionType.HasFlag(CircleQuarter.Right) && !quarters.HasFlag(CircleQuarter.RightBottom) && !quarters.HasFlag(CircleQuarter.RightTop))
                        {
                            positionProcess.X = pos.Value.X;
                            velocityProcess.X = movable.Velocity.X;
                            accelerationProcess.X = movable.Acceleration.X;
                        }

                        if (entity.eCSInputXY.Value.X != 0f)
                        {
                            if (quarters.HasFlag(CircleQuarter.RightBottom) || quarters.HasFlag(CircleQuarter.LeftBottom))
                            {
                                if (!quarters.HasFlag(CircleQuarter.Right) && !quarters.HasFlag(CircleQuarter.Left))
                                {
                                    accelerationProcess.Y = KMath.KMath.MakePositive(movable.Acceleration.X);
                                    velocityProcess.Y = KMath.KMath.MakePositive(movable.Velocity.X);
                                    Flag.Set(ref directionType, CircleQuarter.Top);
                                }
                            }
                            else if(quarters.HasFlag(CircleQuarter.RightTop) || quarters.HasFlag(CircleQuarter.LeftTop))
                            {
                                if (movable.Acceleration.Y < 0f && movable.Velocity.Y < 0f)
                                {
                                    accelerationProcess.Y = -KMath.KMath.MakePositive(movable.Acceleration.X);
                                    velocityProcess.Y = -KMath.KMath.MakePositive(movable.Velocity.X);
                                    Flag.Set(ref directionType, CircleQuarter.Bottom);
                                }
                                else if (movable.Acceleration.Y > 0f && movable.Velocity.Y > 0f)
                                {
                                    if (quarters.HasFlag(CircleQuarter.RightTop))
                                    {
                                        accelerationProcess.X = -KMath.KMath.MakePositive(movable.Acceleration.Y);
                                        velocityProcess.X = -KMath.KMath.MakePositive(movable.Velocity.Y);
                                        Flag.Set(ref directionType, CircleQuarter.Bottom);
                                    }
                                    else if (quarters.HasFlag(CircleQuarter.LeftTop))
                                    {
                                        accelerationProcess.X = KMath.KMath.MakePositive(movable.Acceleration.Y);
                                        velocityProcess.X = KMath.KMath.MakePositive(movable.Velocity.Y);
                                        Flag.Set(ref directionType, CircleQuarter.Bottom);
                                    }
                                }
                            }
                        }


                        entity.ReplacePhysicsPosition2D(positionProcess, pos.PreviousValue);
                        entity.ReplacePhysicsMovable(movable.Speed, velocityProcess, accelerationProcess);
                    }
                }
                
                pos = entity.physicsPosition2D;
                radius = entity.physicsCircle2DCollider.Radius;
                movable = entity.physicsMovable;
                
                {
                    var newCircle = Circle.Create(new Vec2f(pos.PreviousValue.X, pos.Value.Y), radius, entity.agentSprite2D.Size);
                    var quarters = previousCircle.GetTileCollisionQuarters(newCircle, tileMap);

                    if (quarters != CircleQuarter.Error)
                    {
                        Flag.UnsetFlag(ref directionType, quarters);

                        var positionProcess = new Vec2f(pos.Value.X, pos.PreviousValue.Y);
                        var velocityProcess = new Vec2f(movable.Velocity.X, 0.0f);
                        var accelerationProcess = new Vec2f(movable.Acceleration.X, 0.0f);

                        if (directionType.HasFlag(CircleQuarter.Top) && !quarters.HasFlag(CircleQuarter.LeftTop) && !quarters.HasFlag(CircleQuarter.RightTop) || 
                            directionType.HasFlag(CircleQuarter.Bottom) && !quarters.HasFlag(CircleQuarter.LeftBottom) && !quarters.HasFlag(CircleQuarter.RightBottom))
                        {
                            positionProcess.Y = pos.Value.Y;
                            velocityProcess.Y = movable.Velocity.Y;
                            accelerationProcess.Y = movable.Acceleration.Y;
                        }
                        
                        if (entity.eCSInputXY.Value.X == 0f && !quarters.HasFlag(CircleQuarter.Bottom))
                        {
                            if (quarters.HasFlag(CircleQuarter.RightBottom))
                            {
                                accelerationProcess.X = movable.Acceleration.Y;
                                velocityProcess.X = movable.Velocity.Y;
                            }
                            else if (quarters.HasFlag(CircleQuarter.LeftBottom))
                            {
                                accelerationProcess.X = -movable.Acceleration.Y;
                                velocityProcess.X = -movable.Velocity.Y;
                            }
                        }

                        entity.ReplacePhysicsPosition2D(positionProcess, pos.PreviousValue);
                        entity.ReplacePhysicsMovable(movable.Speed, velocityProcess, accelerationProcess);
                    }
                }
            }

            foreach (var entity in entitiesWithBox)
            {
                var pos = entity.physicsPosition2D;
                var entityBoxBorders = new AABB(new Vec2f(pos.PreviousValue.X, pos.Value.Y) + entity.physicsBox2DCollider.Offset, entity.physicsBox2DCollider.Size);
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
                entityBoxBorders = new AABB(new Vec2f(pos.Value.X, pos.PreviousValue.Y) + entity.physicsBox2DCollider.Offset, entity.physicsBox2DCollider.Size);
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
            }
        }
    }
}
