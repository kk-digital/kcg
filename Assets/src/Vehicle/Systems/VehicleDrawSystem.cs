using Entitas;

namespace Systems
{
    public class VehicleDrawSystem
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

            _spriteID = GameState.SpriteLoader.GetSpriteSheetID(_filePath, _width, _height);

            vehicleDraw.AddVehicleComponentDraw(GetSpriteID(), GetWidth(), GetHeight());
        }

        // Get Sprite ID
        public int GetSpriteID()
        {
            return _spriteID;
        }

        // Get Contextx Object
        public Contexts GetContexts()
        {
            return _contexts;
        }

        // Get Width
        public int GetWidth()
        {
            return _width;
        }

        // Get Height
        public int GetHeight()
        {
            return _height;
        }
    }
}
