using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action.Attribute
{
    public struct Component : IComponent
    {
        [PrimaryEntityIndex]
        public int   TypeID;
    }
}
