using System.Collections.Generic;

namespace SystemView
{
    public class SystemAsteroidBelt
    {
        public List<SystemAsteroid> Asteroids;
        public float BeltWidth = 0.5f;
        public OrbitingObjectDescriptor Descriptor;

        public SystemAsteroidBelt()
        {
            Asteroids = new List<SystemAsteroid>();
            Descriptor = new OrbitingObjectDescriptor();
        }

        public int NextUpdate = 0;

        public void UpdatePositions(float dt)
        {
            // todo: Don't update all asteroids each tick
            foreach(SystemAsteroid Asteroid in Asteroids)
            {
                Asteroid.UpdatePosition(dt);
            }
        }
    }
}
