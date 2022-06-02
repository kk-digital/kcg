using UnityEngine;

namespace Agent
{
    public sealed class ProcessVelocitySystem
    {
        public static readonly ProcessVelocitySystem Instance;
        private GameContext gameContext;

        static ProcessVelocitySystem()
        {
            Instance = new ProcessVelocitySystem();
        }

        private ProcessVelocitySystem()
        {
            gameContext = Contexts.sharedInstance.game;
        }

        public void Process(ref List list)
        {
            foreach (var agent in list.AgentsWithVelocity)
            {
                var position = agent.agentPosition2D;
                position.PreviousValue = position.Value;

                position.Value += agent.agentVelocity.Value * Time.deltaTime;
                
                agent.ReplaceAgentPosition2D(position.Value, position.PreviousValue);
            }
        }
    }
}

