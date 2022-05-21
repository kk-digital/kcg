/*
 A MANAGE CLASS TO MANAGE SPRITE TILE PROPERTIES
*/

namespace TileProperties
{
    public class TilePropertiesManager
    {
        // Tile properties
        public PlanetTileProperties[] TileProperties;

        //Singleton
        public static TilePropertiesManager Instance;

        public static TilePropertiesManager instance
        {
            get
            {
                if (Instance == null)
                    Instance = new TilePropertiesManager();
                return Instance;
            }
        }

        // First of all, we initial core elements to avoid crashes. Because, if we initialize relatives before core, and because realtives uses core 
        public static void InitCore()
        
        public TileProperties[] TileProperties;

        public static TilePropertiesManager Instance;
        public TilePropertiesManager ()
        {
            Instance = this;
        }
        
        public static void InitStage1()
        {
            //This is where init goes
            //Instance = new TilePropertiesManager();
        Instance = new TilePropertiesManager();
        if(TilePropertiesManager.Instance.TileProperties == null) TilePropertiesManager.Instance.TileProperties = new TileProperties[1];
        }

        // Once we initialize all core elemets, then we can initialize other parts that is use core elements when working
        public static void InitCoreRelatives()
        {
            
        }

        // Reciving information from Tile property with index
        public PlanetTileProperties GetTileProperty(int index) 
        { 
            return TileProperties[index];
        }
    }
}
