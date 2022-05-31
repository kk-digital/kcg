using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace Systems
{
    public class VehicleDrawSystem
    {
        public Contexts _contexts;
        public string _filePath;
        public int _spriteID;
        public int _width;
        public int _height;

        public VehicleDrawSystem(Contexts contexts)
        {
            _contexts = contexts;
        }

        public void Initialize(string filePath, int spriteID, int width, int height)
        {
            _filePath = filePath;
            _spriteID = spriteID;
            _width = width;
            _height = height;

            GameEntity vehicleDraw = _contexts.game.CreateEntity();

            spriteID = GameState.SpriteLoader.GetSpriteSheetID(_filePath, _width, _height);

            vehicleDraw.AddVehicleComponentDraw(_spriteID, _width, _height);
        }
    }
}
