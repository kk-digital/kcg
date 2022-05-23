using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TileProperties
{
    public class TilePropertiesManager
    {
        public TileProperties[] TileProperties;

        //TODO: Move singleton state to src/GameLoop/GameState.cs struct
        public static TilePropertiesManager Instance;
        public TilePropertiesManager ()
        {
            Instance = this;
        }
        // First of all, we initial core elements to avoid crashes. Because, if we initialize relatives before core, and because realtives uses core 
        //TODO: Rename InitStage1, InitState2
        public static void InitCore()
        {
            //This is where init goes
            //Instance = new TilePropertiesManager();
        Instance = new TilePropertiesManager();
        if(TilePropertiesManager.Instance.TileProperties == null) TilePropertiesManager.Instance.TileProperties = new TileProperties[1];
        }
        // Once we initialize all core elemets, then we can initialize other parts that is use core elements when working
        public static void InitCoreRelatives() //WRONG
        {

        }
        public TileProperties GetTileProperty(int index) 
        { 
            return TileProperties[index];
        }
    }
}
    //TODO: add a function to get pointer to TileProperty struct from index
