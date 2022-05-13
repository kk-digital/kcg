using TiledCS;

//Note:
//May want to put enums inside a struct as static consts?
//so it is accessed like PlanetTileMapLayer.Middle
//right now enums/namespace are imported without calling namespace

namespace Enums
{


    enum PlanetTileLayer : byte
    {
        TileLayerError = 0, //TileLayer.Error
        TileLayerBack,
        TileLayerMiddle,
        TileLayerFront, 
        TileLayerFurniture
    }

    enum PlanetTileCategory : byte
    {
        //TODO
        TileCategoryError = 0 //TileCategory.Error
    }

    enum PlanetTileCollisionType : byte
    {
        TileCollisionTypeError = 0
        //TODO
    }
}