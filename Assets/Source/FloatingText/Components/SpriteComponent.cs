using Entitas;
using UnityEngine;
using KMath;

namespace FloatingText
{
    [FloatingText]
    public struct SpriteComponent : IComponent
    {
        public GameObject GameObject; // used for unity rendering
    }
}
