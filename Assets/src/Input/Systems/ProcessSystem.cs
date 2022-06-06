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

        public void Update()
        {
            var AgentsWithXY = EntitasContext.game.GetGroup(GameMatcher.AllOf(GameMatcher.ECSInput, GameMatcher.ECSInputXY));

            bool jump = Input.GetKeyDown(KeyCode.UpArrow);

            foreach (var entity in AgentsWithXY)
            {
                entity.ReplaceECSInputXY(new Vector2(Input.GetAxisRaw("Horizontal"), 0.0f), jump);
            }
        }
    }
}