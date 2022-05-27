using System;
using UnityEngine;

namespace SystemView
{
    public class SystemViewTest : MonoBehaviour
    {
        private SystemPlanet[] testPlanets;
        private SystemPlanetRenderer[] testRenderers;

        void Start()
        {
            System.Random rnd = new System.Random();

            testPlanets = new SystemPlanet[6];
            testRenderers = new SystemPlanetRenderer[6];

            for (int i = 0; i < 6; i++)
            {
                testPlanets[i] = new SystemPlanet();

                testPlanets[i].Descriptor.CenterX = 0;
                testPlanets[i].Descriptor.CenterY = 0;

                testPlanets[i].Descriptor.SemiMinorAxis = 2.0f + i + 2.0f * (float)rnd.NextDouble();
                testPlanets[i].Descriptor.SemiMajorAxis = testPlanets[i].Descriptor.SemiMinorAxis + 4.0f * (float)rnd.NextDouble();

                testPlanets[i].Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                var child = new GameObject();
                child.name = "Planet Renderer " + (i + 1);

                testRenderers[i] = child.AddComponent<SystemPlanetRenderer>();
                testRenderers[i].planet = testPlanets[i];
                testRenderers[i].orbitColor = new Color((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 1.0f);
                testRenderers[i].planetColor = new Color((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 1.0f);
            }
        }

        void Update()
        {
            for (int i = 0; i < 6; i++)
            {
                testPlanets[i].Descriptor.RotationalPosition += 0.05f / (testPlanets[i].Descriptor.GetDistanceFromCenter());
            }
        }
    }
}
