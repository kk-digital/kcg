using System;
using System.IO;
using Enums.Tile;

namespace PlanetTileMap
{



    public class TileMapManager
    {


        public static TileMap Load(string filePath)
        {
            TileMap tileMap;
            using (var stream = File.Open(fileName, FileMode.Open))
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
                            tileMap.GetBackTile(x, y).ID = rader.ReadInt32();
                        }
                    }

                    for(int y = 0; y < height; y++)
                    {
                        for(int x = 0; x < width; x++)
                        {
                            tileMap.GetMidTile(x, y).ID = rader.ReadInt32();
                        }
                    }

                    for(int y = 0; y < height; y++)
                    {
                        for(int x = 0; x < width; x++)
                        {
                            tileMap.GetFrontTile(x, y).ID = rader.ReadInt32();
                        }
                    }
                }
            }
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
                        binWriter.Write((int)tileMap.GetBackTile(x, y).ID);
                    }
                }
                for(int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        binWriter.Write((int)tileMap.GetMidTile(x, y).ID);
                    }
                }
                for(int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        binWriter.Write((int)tileMap.GetFrontTile(x, y).ID);
                    }
                }
            }  
        }
    }
}