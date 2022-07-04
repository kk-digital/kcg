using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Diagnostics.Tracing;

namespace Agent
{
    [Agent]
    public class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ID;
    }
}