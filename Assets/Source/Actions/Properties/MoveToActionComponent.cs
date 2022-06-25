using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

namespace Action.Property
{
    /// <summary>
    /// This should be used by Goap. To request movement to movimentSystem. 
    /// </summary>
    [ActionProperties]
    public struct MoveToComponent : IComponent
    {
        public Vector2Int GoalPosition;
    }
}
