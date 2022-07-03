using System;
using UnityEngine;
using UnityEngine.UI;
using Source.SystemView;

namespace Scripts {
    namespace SystemView {
        public class PlayerHUD : MonoBehaviour {
            public SystemState State;

            public Text SpeedText;
            //public Text DragText;
            public Text AccelerationText;
            public Text GravityText;
            public Text OrbitalPeriodText;
            public Text HealthText;
            public Text ShieldText;
            public Text MainGravityText;
            public Text ResidualGravityText;

            void Update() {
                if(State.player != null && State.player.ship.descriptor.central_body != null) {
                    float dvx = State.player.ship.descriptor.central_body.velx - State.player.ship.self.velx;
                    float dvy = State.player.ship.descriptor.central_body.vely - State.player.ship.self.vely;

                    float Speed = Tools.magnitude(dvx, dvy);
                    SpeedText.text = "  Velocity: " + String.Format("{0:0.00}", Speed) + " m/s";

                    /*float Drag = 0.0f;
                    if (State.Player.GravitationalStrength < 1.0f)
                    {
                        float GravitationalFactor = 1.0f / (1.0f - State.Player.GravitationalStrength);

                        Drag = Speed * (1.0f / (GravitationalFactor + State.Player.DragFactor));
                    }

                    DragText.text = "Drag: " + String.Format("{0:0.00}", Drag) + " m/s�";*/

                    AccelerationText.text = "Acceleration: " + String.Format("{0:0.00}", State.player.ship.acceleration) + " m/s�  ";

                    float g     = 0.0f;
                    float gx    = 0.0f;
                    float gy    = 0.0f;
                    float maing = 0.0f;

                    foreach(SpaceObject o in State.objects) {
                        float dx = o.posx - State.player.ship.self.posx;
                        float dy = o.posy - State.player.ship.self.posy;

                        float d2 = dx * dx + dy * dy;
                        float d  = (float)Math.Sqrt(d2);

                        float curg = 6.67408E-11f * o.mass / d2;

                        gx += curg * dx / d;
                        gy += curg * dy / d;

                        if(curg > maing) maing = curg;
                    }

                    g = Tools.magnitude(gx, gy);

                    GravityText.text = "  Gravity: " + String.Format("{0:0.00}", g) + " m/s�";

                    MainGravityText.text = "  Main gravity: " + String.Format("{0:0.00}", maing) + " m/s�";
                    ResidualGravityText.text = "Residual gravity: " + String.Format("{0:0.00}", Math.Abs(g - maing)) + " m/s�  ";

                    OrbitalPeriodText.text = "Orbital period: " + (float.IsNaN(State.player.ship.descriptor.orbital_period) ? " hyperbolic  " : (String.Format("{0:0.00}", State.player.ship.descriptor.orbital_period) + " s  "));

                    HealthText.text = "  Health: " + State.player.ship.health + " / " + State.player.ship.max_health;
                    ShieldText.text = "Shield: " + State.player.ship.shield + " / " + State.player.ship.max_shield + "  ";
                }
            }
        }
    }
}
