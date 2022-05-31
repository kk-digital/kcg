using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace AI
{
    sealed public class ActionNodeComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int          ActionNodeID;

        [EntityIndex]
        public int          ParentNodeID;

        public int          PlannerID; // Todo: Should be EntityIndex.

        public GoapState    WorldState;
        public int          PathCost;
        public int          HeuristicCost;

    }
}
