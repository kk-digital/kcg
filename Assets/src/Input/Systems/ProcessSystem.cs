using UnityEngine;

namespace ECSInput
{
    public class ProcessSystem
    {
        Contexts EntitasContext;
        
        public ProcessSystem(Contexts entitasContext)
        {
            EntitasContext = entitasContext;
        }

        public void ProcessAgentInput(ref Agent.List list)
        {
            foreach (var entity in list.AgentsWithXY)
            {
                entity.ReplaceECSInputXY(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            }
        }
    }
}