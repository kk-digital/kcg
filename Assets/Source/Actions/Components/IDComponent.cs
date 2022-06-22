using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Action
{
    public class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int                  ID;
        [EntityIndex]
        public int                  TypeID;
    }
}
