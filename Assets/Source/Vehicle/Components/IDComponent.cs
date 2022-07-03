using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Vehicle
{
    [Vehicle]
    public class IDComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ID;
    }
}