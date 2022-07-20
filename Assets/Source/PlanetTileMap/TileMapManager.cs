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
                            tileMap.SetBackTile(x, y, (TileID)reader.ReadInt32());
                        }
                    }

                    for(int y = 0; y < height; y++)
                    {
                        for(int x = 0; x < width; x++)
                        {
                            tileMap.SetMidTile(x, y, (TileID)reader.ReadInt32());
                        }
                    }

                    for(int y = 0; y < height; y++)
                    {
                        for(int x = 0; x < width; x++)
                        {
                            tileMap.SetFrontTile(x, y, (TileID)reader.ReadInt32());
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
                        binWriter.Write((int)tileMap.GetBackTileID(x, y));
                    }
                }
                for(int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        binWriter.Write((int)tileMap.GetMidTileID(x, y));
                    }
                }
                for(int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        binWriter.Write((int)tileMap.GetFrontTileID(x, y));
                    }
                }
            }  
        }
    }
}