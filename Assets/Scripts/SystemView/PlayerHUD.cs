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
                float Speed = (float)Math.Sqrt(State.Player.Ship.Self.VelX * State.Player.Ship.Self.VelX + State.Player.Ship.Self.VelY * State.Player.Ship.Self.VelY);
                SpeedText.text = "  Velocity: " + String.Format("{0:0.00}", Speed) + " m/s";

                /*float Drag = 0.0f;
                if (State.Player.GravitationalStrength < 1.0f)
                {
                    float GravitationalFactor = 1.0f / (1.0f - State.Player.GravitationalStrength);

                    Drag = Speed * (1.0f / (GravitationalFactor + State.Player.DragFactor));
                }

                DragText.text = "Drag: " + String.Format("{0:0.00}", Drag) + " m/s²";*/

                AccelerationText.text = "Acceleration: " + String.Format("{0:0.00}", State.Player.Ship.Acceleration) + " m/s²  ";

                float g  = 0.0f;
                float gx = 0.0f;
                float gy = 0.0f;

                foreach (SystemViewBody Body in State.Bodies)
                {
                    float dx = Body.PosX - State.Player.Ship.Self.PosX;
                    float dy = Body.PosY - State.Player.Ship.Self.PosY;

                    float d2 = dx * dx + dy * dy;
                    float d  = (float)Math.Sqrt(d2);

                    float curg = 6.67408E-11f * Body.Mass / d2;

                    gx += curg * dx / d;
                    gy += curg * dy / d;
                }

                g = (float)Math.Sqrt(gx * gx + gy * gy);

                GravityText.text = "  Gravity: " + String.Format("{0:0.00}", g) + " m/s²";
                OrbitalPeriodText.text = "Orbital period: " + String.Format("{0:0.00}", State.Player.Ship.Descriptor.OrbitalPeriod) + " s  ";

                HealthText.text = "  Health: " + State.Player.Ship.Health + " / " + State.Player.Ship.MaxHealth;
                ShieldText.text = "Shield: " + State.Player.Ship.Shield + " / " + State.Player.Ship.MaxShield + "  ";
            }
        }
    }
}
