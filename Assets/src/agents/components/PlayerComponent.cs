using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Agent
{
    [Agent]
    public class PlayerComponent : IComponent
    {
        [PrimaryEntityIndex]
        public int ID;

        public Transform ParentGameObject;
    }
}

