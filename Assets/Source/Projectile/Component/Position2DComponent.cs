using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace Projectile
{
    public class Position2DComponent : IComponent
    {
        public Vector2 Position = Vector2.zero;
        public Vector2 TempPosition = Vector2.zero;
    }
}

