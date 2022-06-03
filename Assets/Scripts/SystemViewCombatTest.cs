using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SystemView
{
    public class SystemViewCombatTest : MonoBehaviour
    {
        public PlayerShip        Player;
        public List<SystemEnemy> Enemies;

        public SystemState State;

        public int LastTime;

        void Start()
        {
            LastTime = (int)(Time.time * 1000.0f);

            GameLoop gl = GetComponent<GameLoop>();

            State = gl.CurrentSystemState;

            SystemEnemy Enemy = gameObject.AddComponent<SystemEnemy>();

            Enemies.Add(Enemy);

            Player = gameObject.AddComponent<PlayerShip>();

            State.Star = new SystemStar();
            State.Star.PosX = 0.0f;
            State.Star.PosY = 0.0f;

            var StarObject = new GameObject();
            StarObject.name = "Star Renderer";

            SystemStarRenderer starRenderer = StarObject.AddComponent<SystemStarRenderer>();
            starRenderer.Star = State.Star;
        }

        void Update()
        {

        }
    }
}
