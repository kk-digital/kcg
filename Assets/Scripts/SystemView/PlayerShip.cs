using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class PlayerShip : MonoBehaviour
    {
        public SystemShip Ship;

        public GameObject Object;
        public SystemShipRenderer Renderer;

        private void Start()
        {
            Ship = new SystemShip();

            Object = new GameObject();
            Object.name = "Player ship";

            Ship.UpdatePosition(0.01f);
            Ship.PosX = 10.0f;
            Ship.PosY = 10.0f;
            Ship.Acceleration = 250.0f;

            Renderer = Object.AddComponent<SystemShipRenderer>();
            Renderer.ship = Ship;
            Renderer.shipColor = Color.blue;

            Ship.Health = Ship.MaxHealth = 25000;
            Ship.Shield = Ship.MaxShield = 50000;

            Ship.ShieldRegenerationRate = 2;
        }

        private void Update()
        {
        }
    }
}
