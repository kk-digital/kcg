using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetTileMap
{
    class TilePropertiesManager
    {
        public PlanetTileProperties[] TileProperties;
        private static TilePropertiesManager instance;
        public static TilePropertiesManager Instance
        {
            get {
                if(instance == null)
                {
                    instance = new TilePropertiesManager();
                }
                return instance; 
            }
        }
    }

}