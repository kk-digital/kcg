using Entitas;
using UnityEngine;

namespace Agent
{
    [Agent]
    public class EnemyComponent : IComponent
    {
        public int Behaviour;

        public float DetectionRadius;
    }

        
}
