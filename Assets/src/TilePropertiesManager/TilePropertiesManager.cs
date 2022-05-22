using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TileProperties
{
    public class TilePropertiesManager
    {
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
        public static void InitStage2()
        {
            
        }
        public TileProperties GetTileProperty(int index) 
        { 
            return TileProperties[index];
        }
        
    }
}
    //TODO: add a function to get pointer to TileProperty struct from index
