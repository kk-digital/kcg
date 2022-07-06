using System;
using Entitas;
using Entitas.CodeGeneration.Attributes;

namespace Action.Property
{
    [ActionProperties]
    public class Component : IComponent
    {
        [PrimaryEntityIndex]
        public Enums.ActionType   TypeID;
    }
}
