using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace Projectile
{
    public class ColliderComponent : IComponent
    {
        public bool isCollidingLeft = false;
        public bool isCollidingRight = false;
        public bool isCollidingTop = false;
        public bool isCollidingBottom = false;
    }
}