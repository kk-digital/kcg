using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;
using System;

namespace Agent
{
    public class AIController : IComponent
    {
        [PrimaryEntityIndex]
        public int                  AgentPlannerID;

        public Queue<int>            ActionIDs;
        public List<int>             GoalIDs;
        public AI.GoapState          CurrentWorldState; // todo: This should probably be a memory component. Probably a blackbord? 
    }
}
