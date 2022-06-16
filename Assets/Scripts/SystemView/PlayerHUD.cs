using System;
using UnityEngine;
using UnityEngine.UI;

namespace SystemView
{
    public class PlayerHUD : MonoBehaviour
    {
        public SystemState State;

        public Text SpeedText;
        //public Text DragText;
        public Text AccelerationText;
        public Text GravityText;
        public Text OrbitalPeriodText;
        public Text HealthText;
        public Text ShieldText;

        void Update()
        {
            if (State.Player != null)
            {
                float Speed = (float)Math.Sqrt(State.Player.ship.self.velx * State.Player.ship.self.velx + State.Player.ship.self.vely * State.Player.ship.self.vely);
                SpeedText.text = "  Velocity: " + String.Format("{0:0.00}", Speed) + " m/s";

                /*float Drag = 0.0f;
                if (State.Player.GravitationalStrength < 1.0f)
                {
                    float GravitationalFactor = 1.0f / (1.0f - State.Player.GravitationalStrength);

                    Drag = Speed * (1.0f / (GravitationalFactor + State.Player.DragFactor));
                }

                DragText.text = "Drag: " + String.Format("{0:0.00}", Drag) + " m/s²";*/

                AccelerationText.text = "Acceleration: " + String.Format("{0:0.00}", State.Player.ship.Acceleration) + " m/s²  ";

                float g  = 0.0f;
                float gx = 0.0f;
                float gy = 0.0f;

                foreach (SpaceObject o in State.Objects)
                {
                    float dx = o.posx - State.Player.ship.self.posx;
                    float dy = o.posy - State.Player.ship.self.posy;

                    float d2 = dx * dx + dy * dy;
                    float d  = (float)Math.Sqrt(d2);

                    float curg = 6.67408E-11f * o.mass / d2;

                    gx += curg * dx / d;
                    gy += curg * dy / d;
                }

                g = (float)Math.Sqrt(gx * gx + gy * gy);

                GravityText.text = "  Gravity: " + String.Format("{0:0.00}", g) + " m/s²";
                OrbitalPeriodText.text = "Orbital period: " + (float.IsNaN(State.Player.ship.Descriptor.orbital_period) ? " not orbiting  " : (String.Format("{0:0.00}", State.Player.ship.Descriptor.orbital_period) + " s  "));

                HealthText.text = "  Health: " + State.Player.ship.Health + " / " + State.Player.ship.MaxHealth;
                ShieldText.text = "Shield: " + State.Player.ship.Shield + " / " + State.Player.ship.MaxShield + "  ";
            }
        }
    }
}
