using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action.Property
{
    [ActionProperties]
    public struct Component : IComponent
    {
        [PrimaryEntityIndex]
        public int   TypeID;
    }
}
