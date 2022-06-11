using Entitas;
using UnityEngine;

namespace Agent
{
    public class StatsComponent : IComponent
    {
        public float Health;

        public float AttackCooldown;
    }
}