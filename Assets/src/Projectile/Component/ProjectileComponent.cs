using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using Enums;

namespace Components
{
    public class ProjectileComponent : IComponent
    {
        public ProjectileType projectileType;
        public ProjectileDrawType projectileDrawType;
    }
}
