using UnityEngine;
using Entitas;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Range(0, 1)]
    private float smoothTime = 0.25f;

    private Vector3 velocity;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    void Update()
    {
        // Get Player Position Info
        IGroup<AgentEntity> Playerentities = Contexts.sharedInstance.agent.GetGroup(AgentMatcher.AgentPlayer);
        foreach (var entity in Playerentities)
        {
            Vector3 targetPosition = new Vector3(entity.physicsPosition2D.Value.X, entity.physicsPosition2D.Value.Y, 0.0f) + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
