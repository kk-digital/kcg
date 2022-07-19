using UnityEngine;
using Entitas;
using KMath;

public class CameraFollow
{
    private Vec2f offset = new Vec2f(0f, 0f);

    [Range(0, 10)]
    public float followSpeed = 3.0f;

    KMath.Vec2f PlayerPos;

    public bool canFollow = false;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    public void Update(ref Planet.PlanetState planetState)
    {
        if(canFollow)
        {
            // Check if projectile has hit a enemy.
            var entities = planetState.EntitasContext.agent.GetGroup(AgentMatcher.AllOf(AgentMatcher.AgentID));
            foreach (var entity in entities)
            {
                if(entity.isAgentPlayer)
                {
                    // Set Player Position from Player Entity
                    PlayerPos = new KMath.Vec2f(entity.physicsPosition2D.Value.X, entity.physicsPosition2D.Value.Y) + offset;
                }
            }

            // Follow Player Position
            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, new Vector3(PlayerPos.X, PlayerPos.Y, -10.0f), followSpeed * Time.deltaTime);
        }
        
    }
}
