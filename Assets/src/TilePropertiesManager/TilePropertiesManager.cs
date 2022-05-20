using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TileProperties
{
    public class TilePropertiesManager
    {
        public PlanetTileProperties[] TileProperties;
        public static TilePropertiesManager Instance;
        public TilePropertiesManager ()
        {
            Instance = this;
        }
        
        public static void InitStage1()
        {
            //This is where init goes
            //Instance = new TilePropertiesManager();
        }

        public static void InitStage2()
        {

        }
    }
}
    //TODO: add a function to get pointer to TileProperty struct from index
