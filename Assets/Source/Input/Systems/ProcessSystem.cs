using UnityEngine;

namespace ECSInput
{
    public class ProcessSystem
    {
        public void Update()
        {
            var AgentsWithXY = Contexts.sharedInstance.game.GetGroup(GameMatcher.AllOf(GameMatcher.ECSInput, GameMatcher.ECSInputXY));

            bool jump = Input.GetKeyDown(KeyCode.UpArrow);
            float x = 0.0f;
            if (Input.GetKey(KeyCode.RightArrow))
            {
                x = 1.0f;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                x = -1.0f;
            }

            foreach (var entity in AgentsWithXY)
            {
                entity.ReplaceECSInputXY(new Vector2(x, 0.0f), jump);
            }
        }
    }
}