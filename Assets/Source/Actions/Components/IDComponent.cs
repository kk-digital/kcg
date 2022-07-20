using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Action
{
    [Action]
    public class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int                  ID;
        [EntityIndex]
        public Enums.ActionType     TypeID;
    }
}
