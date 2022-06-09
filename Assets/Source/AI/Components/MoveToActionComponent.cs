using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace AI
{
    public struct MoveToActionComponent : IComponent
    {
        public Vector2Int GoalPosition;
    }
}
