using System;
using System.IO;
using System.Text;
using Enums.Tile;
using KMath;

namespace PlanetTileMap
{



    public class TileMapManager
    {


        public static TileMap Load(string filePath, int playerPositionX, int playerPositionY)
        {
            TileMap tileMap;
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    int width = reader.ReadInt32();
                    int height = reader.ReadInt32();
                    tileMap = new TileMap(new Vec2i(width, height));

                    for(int y = 0; y < height; y++)
                    {
                        for(int x = 0; x < width; x++)
                        {
                            tileMap.GetBackTile(x, y).MaterialType = (TileMaterialType)reader.ReadInt32();
                        }
                    }

                    for(int y = 0; y < height; y++)
                    {
                        for(int x = 0; x < width; x++)
                        {
                            tileMap.GetMidTile(x, y).MaterialType = (TileMaterialType)reader.ReadInt32();
                        }
                    }

                    for(int y = 0; y < height; y++)
                    {
                        for(int x = 0; x < width; x++)
                        {
                            tileMap.GetFrontTile(x, y).MaterialType = (TileMaterialType)reader.ReadInt32();
                        }
                    }


                    tileMap.UpdateBackTileMapPositions(playerPositionX, playerPositionY);
                    tileMap.UpdateMidTileMapPositions(playerPositionX, playerPositionY);
                    tileMap.UpdateFrontTileMapPositions(playerPositionX, playerPositionY);
                }
            }

            return tileMap;
        }


        public static void Save(TileMap tileMap, string filePath)
        {
            int width = tileMap.MapSize.X;
            int height = tileMap.MapSize.Y;

            

            using (BinaryWriter binWriter =  
                new BinaryWriter(File.Open(filePath, FileMode.Create)))  
            {  
                binWriter.Write(width);
                binWriter.Write(height);

                for(int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        binWriter.Write((int)tileMap.GetBackTile(x, y).MaterialType);
                    }
                }
                for(int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        binWriter.Write((int)tileMap.GetMidTile(x, y).MaterialType);
                    }
                }
                for(int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        binWriter.Write((int)tileMap.GetFrontTile(x, y).MaterialType);
                    }
                }
            }  
        }
    }
}