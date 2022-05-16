using UnityEngine;
using System.IO;
using Enums;

// load tile sprite from streaming assets
// only accepts square tile image, ex. 32x32.
// only accepts png
// if image is 16x16, it will scale up to 32x32
public static class TileSpriteLoader
{
    // load image files from path and output it as sprite
    public static Sprite LoadSprite(string filename)
    {
        if (!filename.Contains(".png"))
        {
            return null;
        }
        else
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, $"{TileSpriteInfo.Path}/{filename}");
            var spriteTexture = LoadTexture(filePath);

            if (spriteTexture != null)
            {
                if (spriteTexture.width != spriteTexture.height)
                {
                    return null;
                }
                else
                {
                    Vector2 pivot = new Vector2(TileSpriteInfo.HorizontalPivot, TileSpriteInfo.VerticalPivot); // centered-pivot

                    float pixelPerUnit = spriteTexture.width / TileSpriteInfo.Size; // divide texture width (or height) by 32, to get the pixel per unit
                    var sprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), pivot, pixelPerUnit);

                    return sprite;
                }
            }
            else
            {
                return null;
            }
        }
    }

    // load image file as texture from streaming assets
    private static Texture2D LoadTexture(string filePath)
    {
        if (File.Exists(filePath))
        {
            Texture2D texture = new Texture2D(TileSpriteInfo.Size, TileSpriteInfo.Size); // 32 x 32 same as tile size
            byte[] fileData;

            fileData = File.ReadAllBytes(filePath);
            texture.LoadImage(fileData);
            return texture;
        }
        else
        {
            return null;
        }
    }
}