using System.Collections.Generic;

namespace SystemView
{
    /*
    public class SystemAsteroidBelt
    {
        public List<SystemAsteroid> Asteroids;
        public float BeltWidth = 0.5f;
        public OrbitingObjectDescriptor CentralDescriptor;
        public OrbitingObjectDescriptor[] Descriptor;
        public float[] AverageAltitude;
        public float[] RotationalPos;

        public SystemAsteroidBelt(int Layers, OrbitingObjectDescriptor d)
        {

            Asteroids       = new List<SystemAsteroid>();
            Descriptor      = new OrbitingObjectDescriptor[Layers];
            AverageAltitude = new float[Layers];
            RotationalPos   = new float[Layers];

            CentralDescriptor = new OrbitingObjectDescriptor(d);

            for (int i = 0; i < Layers; i++)
            {
                Descriptor[i] = new OrbitingObjectDescriptor();

                Descriptor[i].CenterX = d.CenterX;
                Descriptor[i].CenterY = d.CenterY;

                Descriptor[i].SemiMinorAxis = d.SemiMinorAxis + (i - Layers / 2) * 0.05f;
                Descriptor[i].SemiMajorAxis = d.SemiMajorAxis + (i - Layers / 2) * 0.05f * d.SemiMajorAxis / d.SemiMinorAxis;

                AverageAltitude[i] = (Descriptor[i].GetDistanceFromCenterAt(0.0f) + Descriptor[i].GetDistanceFromCenterAt(3.1415926f)) / 2.0f;
            }

            BeltWidth = 0.05f * Layers;
        }

        public int NextUpdate = 0;

        public void UpdatePositions(float dt)
        {
            for (int i = 0; i < RotationalPos.Length; i++)
            {
                RotationalPos[i] += dt / AverageAltitude[i] / AverageAltitude[i];
            }

            foreach(SystemAsteroid Asteroid in Asteroids)
            {
                // this would be slow
                // float altitude = Descriptor[Asteroid.Layer].GetDistanceFromCenterAt(Asteroid.RotationalPosition);
                // Asteroid.UpdatePosition(dt / altitude / altitude);

                Asteroid.UpdatePosition(dt / AverageAltitude[Asteroid.Layer] / AverageAltitude[Asteroid.Layer]);
            }
        }
    }*/
}
