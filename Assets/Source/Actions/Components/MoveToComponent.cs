using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using KMath;

namespace Action
{
    /// <summary>
    /// This should be used by Goap. To request movement to movimentSystem. 
    /// </summary>
    [Action]
    public class MoveToComponent : IComponent
    {
        public Vec2f GoalPosition;
    }
}
