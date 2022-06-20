using Entitas;

namespace Agent
{

    public struct AgentEntity
    {
        public int AgentId;
        public bool IsInitialized;
        public GameEntity Entity;
    }
}