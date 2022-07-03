using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using KMath;

namespace FloatingText
{
    [FloatingText]
    public class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int Index;
    }
}
