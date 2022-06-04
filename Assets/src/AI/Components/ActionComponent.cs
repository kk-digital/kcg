using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace AI
{
    public struct ActionComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ActionID;

        public GoapState PreConditions;
        public GoapState Effects;

        //public int Duration;  // How long it takes to execute the action in miliseconds.
        public int      Cost;
    }
}
