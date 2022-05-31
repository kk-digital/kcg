using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace Agent
{
    public class VehicleDrawSystem : IInitializeSystem
    {
        // Entitas Context
        public Contexts _contexts;

        // Streaming Asset File Path
        public string _filePath;

        // Sprite ID
        public int _spriteID;
        
        // Image width
        public int _width;
        
        // Image Height
        public int _height;

        // Sprite Loader for loading image
        TileSpriteLoader.TileSpriteLoader tileSpriteLoader;

        // Constructor, variables setup
        public VehicleDrawSystem(Contexts contexts, string filePath, int width, int height)
        {
            _contexts = contexts;
            _filePath = filePath;
            _width = width;
            _height = height;
        }

        // Initializing image and component
        public void Initialize()
        {
            GameEntity vehicleDraw = _contexts.game.CreateEntity();
            tileSpriteLoader = new TileSpriteLoader.TileSpriteLoader();

            tileSpriteLoader.GetSpriteSheetID(_filePath);

            vehicleDraw.AddVehicleComponentDraw(GetSpriteID(), _width, _height);
        }

        // Get sprite id
        public int GetSpriteID()
        {
            return _spriteID;
        }
    }
}
