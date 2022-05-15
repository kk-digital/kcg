using Entitas;
using UnityEngine;

namespace src.ecs.Game.Particle.ParticleSpawn
{
    [Game]
    public class GameObjectComponent : IComponent
    {
        public GameObject value;
    }
}