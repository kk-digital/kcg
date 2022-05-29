using System.Collections.Generic;

namespace SystemView
{
    public class SystemAsteroidBelt
    {
        public List<SystemAsteroid> Asteroids;
        public float BeltWidth = 0.5f;
        public OrbitingObjectDescriptor CentralDescriptor;
        public OrbitingObjectDescriptor[] Descriptor;

        public SystemAsteroidBelt(int Layers, OrbitingObjectDescriptor d)
        {
            Asteroids = new List<SystemAsteroid>();
            Descriptor = new OrbitingObjectDescriptor[Layers];

            CentralDescriptor = new OrbitingObjectDescriptor(d);

            for (int i = 0; i < Layers; i++)
            {
                Descriptor[i] = new OrbitingObjectDescriptor();

                Descriptor[i].CenterX = d.CenterX;
                Descriptor[i].CenterY = d.CenterY;

                Descriptor[i].SemiMinorAxis = d.SemiMinorAxis + (i - Layers / 2) * 0.2f;
                Descriptor[i].SemiMajorAxis = d.SemiMajorAxis + (i - Layers / 2) * 0.2f * d.SemiMajorAxis / d.SemiMinorAxis;
            }

            BeltWidth = 0.2f * Layers;
        }

        public int NextUpdate = 0;

        public void UpdatePositions(float dt)
        {
            // todo: Don't update all asteroids each tick
            foreach(SystemAsteroid Asteroid in Asteroids)
            {
                float altitude = Descriptor[Asteroid.Layer].GetDistanceFromCenterAt(Asteroid.RotationalPosition);
                Asteroid.UpdatePosition(dt / altitude / altitude);
            }
        }
    }
}
