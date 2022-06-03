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
            foreach (var entity in list.AgentsWithXY)
            {
                entity.ReplaceECSInputXY(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            }
        }
    }
}