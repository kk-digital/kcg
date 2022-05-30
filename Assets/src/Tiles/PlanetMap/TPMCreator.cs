using UnityEngine;
using Systems.Planets;

namespace Tiles.PlanetMap
{
    //Note: TileMap should be mostly controlled by GameManager


    //Note(Mahdi): we are just testing and making sure everything is working
    // before we move things out of here
    // there will be things like rendering, collision, TileMap
    // that are not supposed to be here.

    public class TPMCreator
    {
        private static TPMCreator instance;
        public static TPMCreator Instance => instance ??= new TPMCreator();
        
        public TilesPlanetMap PlanetTilesMap;
        
        //All memory allocations/setups go here
        //File loading should not occur at this stage
        public void InitStage1()
        {
            PlanetTilesMap = new TilesPlanetMap(new Vector2Int(128, 128));
            SPTileMemoryReserve.Instance.ReserveTiles();
            SPTileCreator.Instance.CreateTiles(ref PlanetTilesMap);
        }

        //Load settings from files and other init, that requires systems to be intialized
        public void InitStage2()
        {
            
        }      


        

        public void ReloadMap()
        {
            InitStage1();
            InitStage2();
        }
    }
}
