using Entitas;
using UnityEngine;

namespace src.ecs.Game.Particle.ParticleSpawn
{
    [Game]
    public class PositionComponent : IComponent
    {
        public Vector3 value;
    }
}