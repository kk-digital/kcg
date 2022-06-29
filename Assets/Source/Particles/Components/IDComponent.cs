using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Particle
{
    [Particle]
    public struct IDComponent : IComponent
    {
        public int ID;
    }
}