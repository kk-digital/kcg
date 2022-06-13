using Entitas;
using UnityEngine;

namespace Item.Attribute
{
    public struct SizeComponent : IComponent
    {
        /// <summary>
        /// .x = 1.0f = tile Size.
        /// </summary>
        public Vector2 Size;
    }
}
