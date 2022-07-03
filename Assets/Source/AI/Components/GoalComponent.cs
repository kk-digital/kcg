using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;


namespace AI
{
    [AI]
    public class GoalComponent: IComponent
    {
        [PrimaryEntityIndex]
        public int          GoalID;

        public GoapState    GoalState;
        public int          Priority;
    }
}
