using Physics;
using UnityEngine;

public struct Player
{
    public int PlayerSpriteID;

    public Vector2 Pos
    {
        get => collider.Pos;
        set => collider.Pos = value;
    }
    public Vector2 Size => collider.Size;
    public Vector2 PrevPos;

    public float Speed;
    public Vector2 Vel
    {
        get => collider.Vel;
        set => collider.Vel = value;
    }
    public float Direction;
    
    public bool IgnoresGravity;
    public bool IsOnGround => collider.CollisionInfo.Below;
    
    private RectangleBoundingBoxCollision collider;

    private Player(int playerSpriteID) : this()
    {
        PlayerSpriteID = playerSpriteID;
    }
    
    public Player(int playerSpriteID, Vector2 pos, Vector2 size) : this(playerSpriteID)
    {
        collider = new RectangleBoundingBoxCollision(size, pos);
    }

    public bool MovePlayer(ref PlanetTileMap.PlanetTileMap map, Vector2 newPos)
    {
        return collider.ResolveCollisions(ref map, newPos);
    }
}
