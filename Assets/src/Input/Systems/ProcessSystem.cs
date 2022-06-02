using UnityEngine;

namespace ECSInput
{
    public sealed class ProcessSystem
    {
        public static readonly ProcessSystem Instance;

        static ProcessSystem()
        {
            Instance = new ProcessSystem();
        }

        public void Update(ref Agent.List list)
        {
            foreach (var entity in list.AgentsWithInput)
            {
                entity.agentVelocity.Value = Vector2.zero;
                
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    entity.agentVelocity.Value.y = 1f;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    entity.agentVelocity.Value.y = -1f;
                }

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    entity.agentVelocity.Value.x = 1f;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    entity.agentVelocity.Value.x = -1f;
                }
            }
        }
    }
}