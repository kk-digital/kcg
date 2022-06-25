using Entitas;
using KMath;
using UnityEngine;

namespace Item.Property
{
    [ItemProperties]
    public struct SizeComponent : IComponent
    {
        /// <summary>
        /// .x = 1.0f = tile Size.
        /// </summary>
        public Vec2f Size;
    }
}
