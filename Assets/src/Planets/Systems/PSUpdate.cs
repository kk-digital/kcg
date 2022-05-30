using Tiles.PlanetMap;
using TileProperties;

namespace Planets.Systems
{
    public class PSUpdate
    {
        private static PSUpdate instance;
        public static PSUpdate Instance => instance ??= new PSUpdate();
        
        public void UpdateTileMap(ref TilesPlanetMap tilesMap, ref UnityEngine.Transform tileParent, ref UnityEngine.Material tileAtlas)
        {
            var TileSize = tilesMap.TileSize;
            
            var visibleRect = CalcVisibleRect();

            byte[] bytes = new byte[32 * 32* 4];

            for(int layerIndex = 0; layerIndex < 1; layerIndex++)
            {
                for(int j = 0; j < tilesMap.Size.y; j++)
                {
                    for(int i = 0; i < tilesMap.Size.x; i++)
                    {
                        ref PlanetTile tile = ref tilesMap.getTile(i, j);
                        int tilePropertiesIndex = tile.TileIdPerLayer[layerIndex];

                        if (tilePropertiesIndex >= 0)
                        {
                            TilePropertiesData tileProperties =
                                GameState.TileCreationApi.GetTileProperties(tilePropertiesIndex);

                            GameState.TileSpriteAtlasManager.GetSpriteBytes(tileProperties.SpriteId, bytes);

                            var x = (i * tilesMap.TileSize);
                            var y = (j * TileSize);

                            if (x + TileSize >= visibleRect.X && x <= visibleRect.X + visibleRect.W &&
                                y + TileSize >= visibleRect.Y && y <= visibleRect.Y + visibleRect.H)
                            {
                                TPMMeshBuilder.Instance.DrawTile(ref tileParent, ref tileAtlas, x, y, TileSize, TileSize, bytes, 32, 32);
                            }
                        }

                    }
                }
            }
        }

        // TODO: Move out from here
        private struct CameraRect
        {
            public float X;
            public float Y;
            public float W;
            public float H;

            public CameraRect(float x, float y, float w, float h)
            {
                X = x;
                Y = y;
                W = w;
                H = h;
            }
        }
        
        // TODO: Move out CalcVisibleRect from here
        CameraRect CalcVisibleRect()
        {
            var cam = UnityEngine.Camera.main;
            var pos = cam.transform.position;
            var height = 2f * cam.orthographicSize;
            var width = height * cam.aspect;
            var visibleRect = new CameraRect(pos.x - width / 2, pos.y - height / 2, width, height);
            return visibleRect;
        }
    }
}
