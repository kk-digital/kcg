using System;
using UnityEngine;
using System.Collections.Generic;

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

            for (int i = 1; i <= 3; i++)
            {
                SystemPlanet testPlanet = new SystemPlanet();

                testPlanet.Descriptor.CenterX = State.Star.PosX;
                testPlanet.Descriptor.CenterY = State.Star.PosY;

                testPlanet.Descriptor.SemiMinorAxis = 1.0f + 2.0f * (float)rnd.NextDouble() * i;
                testPlanet.Descriptor.SemiMajorAxis = testPlanet.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() / 8.0f;

                testPlanet.Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                testPlanet.Descriptor.RotationalPosition = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                var child = new GameObject();
                child.name = "Planet Renderer " + i;

                testRenderers[i - 1] = child.AddComponent<SystemPlanetRenderer>();
                testRenderers[i - 1].planet = testPlanet;

                State.Planets.Add(testPlanet);
            }

            OrbitingObjectDescriptor testBeltDescriptor = new OrbitingObjectDescriptor();

            testBeltDescriptor.CenterX = State.Star.PosX;
            testBeltDescriptor.CenterY = State.Star.PosY;

            testBeltDescriptor.SemiMinorAxis = State.Planets[2].Descriptor.SemiMajorAxis + 4.0f + (float)rnd.NextDouble();
            testBeltDescriptor.SemiMajorAxis = testBeltDescriptor.SemiMinorAxis + (float)rnd.NextDouble() / 4.0f;

            SystemAsteroidBelt testBelt = new SystemAsteroidBelt(16, testBeltDescriptor);

            for (int Layer = 0; Layer < 16; Layer++)
            {
                for (int i = 0; i < 192 + 8 * Layer; i++)
                {
                    SystemAsteroid testAsteroid = new SystemAsteroid();

                    testAsteroid.RotationalPosition = (float)i * 3.1415926f / (96.0f + 4.0f * Layer);
                    testAsteroid.Layer = Layer;

                    testBelt.Asteroids.Add(testAsteroid);
                }
            }

            State.AsteroidBelts.Add(testBelt);

            var AsteroidBeltObject = new GameObject();
            AsteroidBeltObject.name = "Asteroid Belt Renderer";

            SystemAsteroidBeltRenderer asteroidBeltRenderer = AsteroidBeltObject.AddComponent<SystemAsteroidBeltRenderer>();
            asteroidBeltRenderer.belt = testBelt;

            for (int i = 0; i < 3; i++)
            {
                SystemPlanet testPlanet = new SystemPlanet();

                testPlanet.Descriptor.CenterX = State.Star.PosX;
                testPlanet.Descriptor.CenterY = State.Star.PosY;

                testPlanet.Descriptor.SemiMinorAxis = testBeltDescriptor.SemiMajorAxis + testBelt.BeltWidth + 4.0f * (float)rnd.NextDouble() * (i + 1);
                testPlanet.Descriptor.SemiMajorAxis = testPlanet.Descriptor.SemiMinorAxis + (float)rnd.NextDouble() * (i + 1) / 4.0f;

                testPlanet.Descriptor.Rotation = (float)rnd.NextDouble() * 2.0f * 3.1415926f;

                var child = new GameObject();
                child.name = "Planet Renderer " + (i + 4);

                testRenderers[i + 3] = child.AddComponent<SystemPlanetRenderer>();
                testRenderers[i + 3].planet = testPlanet;

                State.Planets.Add(testPlanet);
            }

            int shipnr = 1;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (j == i) continue;

                    SystemShip testShip = new SystemShip();

                    State.Ships.Add(testShip);

                    var shipRendererObject = new GameObject();
                    shipRendererObject.name = "Ship Renderer " + shipnr++;

                    testShip.Start = State.Planets[i].Descriptor;
                    testShip.Destination = State.Planets[j].Descriptor;

                    SystemShipRenderer shipRenderer = shipRendererObject.AddComponent<SystemShipRenderer>();
                    shipRenderer.ship = testShip;
                }
            }
        }

        void Update()
        {
            foreach (SystemPlanet p in State.Planets)
            {
                p.UpdatePosition(0.2f);
            }
            
            foreach (SystemAsteroidBelt b in State.AsteroidBelts)
            {
                b.UpdatePositions(0.2f);
            }

            foreach (SystemShip b in State.Ships)
            {
                if (!b.PathPlanned)
                    b.PlanPath(b.Start, b.Destination, 0.1f);
                else if (!b.Reached && b.Descriptor.GetDistanceFrom(b.Destination) < 1.0f)
                {
                    b.Descriptor = new OrbitingObjectDescriptor(b.Destination);
                    b.PathPlanned = false;
                    (b.Start, b.Destination) = (b.Destination, b.Start);
                }

                b.UpdatePosition(0.2f);
            }
        }
    }
}
