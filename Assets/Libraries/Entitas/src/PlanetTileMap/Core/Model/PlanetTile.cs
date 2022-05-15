﻿namespace PlanetTileMap
{
    /// <summary> Contains info about tile, include all layers </summary>
    public struct PlanetTile
    {
        //Back tile property id
        public int BackTileId;

        //Mid tile property id
        public int MidTileId;

        //Front tile property id
        public int FrontTileId;
        
        //Furniture tile property id
        public int FurnitureTileId;

        public sbyte FurnitureOffsetX;
        public sbyte FurnitureOffsetY;

        //Health
        public byte Durability;
    }
}