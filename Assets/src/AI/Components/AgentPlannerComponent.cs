using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;
using System;

namespace AI
{
    public struct AgentPlannerComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int                  AgentPlannerID;

        public Queue<int>    ActionIDs;
        public List<ActionInfo>     ActiveActionIDs;
        public List<int>            GoalIDs;

        public GoapState            CurrentWorldState;
    }

    public struct ActionInfo
    {
        public ActionInfo(int ID)
        { 
            ActionID = ID;
            StartTime = DateTime.Now;
        }

        public int      ActionID; 
        public DateTime StartTime;
    }
}
