
//Note:
//May want to put enums inside a struct as static consts?
//so it is accessed like PlanetTileMapLayer.Middle
//right now enums/namespace are imported without calling namespace

namespace Enums
{


    enum PlanetTileLayer : byte
    {
        TileLayerError = 0, //TileLayer.Error

        TileLayerBack = 1,
        TileLayerMiddle = 2 ,
        TileLayerFront = 3 , 
        TileLayerFurniture = 4
    }

    enum PlanetTileCategory : byte
    {
        //TODO
        TileCategoryError = 0 //TileCategory.Error
    }

    enum PlanetTileCollisionType : byte
    {
        TileCollisionTypeError = 0,
        //TODO
        TileCollisionTypeSolid, //just a normal solid tile
        TileCollisionTypeAir, //no collision, no blocking
        //TileCollisionTypeLiquid,
    }
}