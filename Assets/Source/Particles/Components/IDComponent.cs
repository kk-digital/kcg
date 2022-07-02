using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Particle
{
    [Particle]
    public class IDComponent : IComponent
    {
        public int ID;
    }
}