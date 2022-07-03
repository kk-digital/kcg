using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using Planet;

namespace KGUI.Statistics
{
    public static class StatisticsDisplay
    {
        public static Rect PositionInfo1609 = new Rect(Camera.main.pixelWidth - 150, 0, 200, 200);
        public static Rect PositionInfo1610 = new Rect(Camera.main.pixelWidth - 250, 0, 200, 200);
        public static Rect PositionInfo0403 = new Rect(Camera.main.pixelWidth - 170, 0, 200, 200);
        private static KMath.Vec2f playerPosition;
        private static GUIStyle TextStyle = new GUIStyle();

        public static Rect WorldSizeInfo1609 = new Rect(Camera.main.pixelWidth - 150, 20, 200, 200);
        public static Rect WorldSizeInfo1610 = new Rect(Camera.main.pixelWidth - 250, 20, 200, 200);
        public static Rect WorldSizeInfo0403 = new Rect(Camera.main.pixelWidth - 170, 20, 200, 200);

        public static Rect MSInfoSizeInfo1609 = new Rect(Camera.main.pixelWidth - 150, 40, 200, 200);
        public static Rect MSInfoSizeInfo1610 = new Rect(Camera.main.pixelWidth - 250, 40, 200, 200);
        public static Rect MSInfoSizeInfo0403 = new Rect(Camera.main.pixelWidth - 170, 40, 200, 200);

        public static Rect FPSSizeInfo1609 = new Rect(Camera.main.pixelWidth - 150, 60, 200, 200);
        public static Rect FPSSizeInfo1610 = new Rect(Camera.main.pixelWidth - 250, 60, 200, 200);
        public static Rect FPSSizeInfo0403 = new Rect(Camera.main.pixelWidth - 170, 60, 200, 200);

        static float frameCount = 0;
        static double dt = 0.0;
        static double fps = 0.0;
        static double updateRate = 4.0;  // 4 updates per sec.

        public static bool canDraw = true;

        public static void DrawStatistics(PlanetState planet)
        {
            if(canDraw)
            {
                if(TextStyle.normal.textColor != Color.red)
                TextStyle.normal.textColor = Color.red;

                frameCount++;
                dt += Time.deltaTime;
                if (dt > 1.0 / updateRate)
                {
                    fps = frameCount / dt;
                    frameCount = 0;
                    dt -= 1.0 / updateRate;
                }

                // Get Player Position Info
                IGroup<AgentEntity> Playerentities = planet.EntitasContext.agent.GetGroup(AgentMatcher.AgentPlayer);
                foreach (var entity in Playerentities)
                {
                    playerPosition = entity.physicsPosition2D.Value;
                }

                if (Camera.main.aspect >= 1.7f)
                {
                    // Render Position Info in 16:09
                    GUI.Label(PositionInfo1609, "Position: X: " + playerPosition.X + ", Y: " + playerPosition.Y, TextStyle);

                    // Render World Size Info in 16:09
                    GUI.Label(WorldSizeInfo1609, "World Size: X: " + planet.TileMap.MapSize.X + ", Y: " + planet.TileMap.MapSize.Y, TextStyle);

                    // Render MS Per Frame in 16:09
                    GUI.Label(MSInfoSizeInfo1609, "MS: " + 1 / Time.deltaTime, TextStyle);

                    // Render FPS in 16:09
                    GUI.Label(FPSSizeInfo1609, "FPS: " + (int)fps, TextStyle);
                }
                else if (Camera.main.aspect >= 1.5f)
                {
                    // Render Position Info in 16:10
                    GUI.Label(PositionInfo1610, "Position: X: " + playerPosition.X + ", Y: " + playerPosition.Y, TextStyle);

                    // Render World Size Info in 16:10
                    GUI.Label(WorldSizeInfo1610, "World Size: X: " + planet.TileMap.MapSize.X + ", Y: " + planet.TileMap.MapSize.Y, TextStyle);

                    // Render MS Per Frame in 16:10
                    GUI.Label(MSInfoSizeInfo1610, "MS: " + 1 / Time.deltaTime, TextStyle);

                    // Render FPS in 16:10
                    GUI.Label(FPSSizeInfo1610, "FPS: " + (int)fps, TextStyle);
                }
                else
                {
                    // Render Position Info in 4:3
                    GUI.Label(PositionInfo0403, "Position: X: " + playerPosition.X + ", Y: " + playerPosition.Y, TextStyle);

                    // Render World Size Info in 4:3
                    GUI.Label(WorldSizeInfo0403, "World Size: X: " + planet.TileMap.MapSize.X + ", Y: " + planet.TileMap.MapSize.Y, TextStyle);

                    // Render MS Per Frame in 4:3
                    GUI.Label(MSInfoSizeInfo0403, "MS: " + 1 / Time.deltaTime, TextStyle);

                    // Render FPS in 4:3
                    GUI.Label(FPSSizeInfo0403, "FPS: " + (int)fps, TextStyle);
                }
            }
        }
    }
}
