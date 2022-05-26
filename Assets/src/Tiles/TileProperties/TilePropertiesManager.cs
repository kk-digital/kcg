namespace TileProperties
{
    public class TilePropertiesManager
    {
        public TilePropertiesData[] TileProperties;
        
        // First of all, we initial core elements to avoid crashes. Because, if we initialize relatives before core, and because realtives uses core 
        public void InitStage1()
        {
            //This is where init goes
            //Instance = new TilePropertiesManager();
            TileProperties ??= new TilePropertiesData[1];
        }
        
        // Once we initialize all core elemets, then we can initialize other parts that is use core elements when working
        public void InitStage2()
        {

        }
        
        public TilePropertiesData GetTileProperty(int index) 
        { 
            return TileProperties[index];
        }
    }
}
    //TODO: add a function to get pointer to TileProperty struct from index
