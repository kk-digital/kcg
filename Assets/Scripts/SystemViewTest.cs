using System;
using UnityEngine;

namespace SystemView
{
    public class SystemViewTest : MonoBehaviour
    {
        public SystemState State;

        private void Start()
        {
            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            System.Random rnd = new System.Random();

            SystemPlanetRenderer[] testRenderers = new SystemPlanetRenderer[6];

            State.Star = new SystemStar();
            State.Star.PosX = (float)rnd.NextDouble() * 8.0f - 4.0f;
            State.Star.PosY = (float)rnd.NextDouble() * 8.0f - 4.0f;

            var StarObject = new GameObject();
            StarObject.name = "Star Renderer";

            SystemStarRenderer starRenderer = StarObject.AddComponent<SystemStarRenderer>();
            starRenderer.Star = State.Star;

            for (int i = 0; i < 6; i++)
            {
                SystemPlanet testPlanet = new SystemPlanet();

                testPlanet.Descriptor.CenterX = State.Star.PosX;
                testPlanet.Descriptor.CenterY = State.Star.PosY;

                testPlanet.Descriptor.SemiMinorAxis = 2.0f + i + 2.0f * (float)rnd.NextDouble();
                testPlanet.Descriptor.SemiMajorAxis = testPlanet.Descriptor.SemiMinorAxis + 4.0f * (float)rnd.NextDouble();

                testPlanet.Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                var child = new GameObject();
                child.name = "Planet Renderer " + (i + 1);

                testRenderers[i] = child.AddComponent<SystemPlanetRenderer>();
                testRenderers[i].planet = testPlanet;

                State.Planets.Add(testPlanet);
            }
        }

        void Update()
        {
            foreach(SystemPlanet p in State.Planets)
            {
                p.Descriptor.RotationalPosition += 0.05f / (p.Descriptor.GetDistanceFromCenter());
            }
        }
    }
}
