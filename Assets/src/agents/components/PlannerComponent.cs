using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;

// Todo: Should this really be a AgentComponent?
namespace Agent
{
    [Agent]
    public struct PlannerComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int AgentPlannerID;

        public Queue<int> ActionIDs;
        public List<int>  GoalIDs;

        //public int RunningActionID; 
        //public int ActionStartTime;   // Start Time of an Action in miliseconds.
        public AI.GoapState CurrentWorldState;
    }
}
