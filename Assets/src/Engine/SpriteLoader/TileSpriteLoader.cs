using UnityEngine;
using System.IO;

//TODO: Dont import unity

//All tiles will be 32x32 pixels, but use a const
public class TileSpriteLoader
{
    public static TileSpriteLoader Instance;
    public TileSpriteLoader()
    {
        Instance = this;
    }
    
    // This script loads a PNG or JPEG image from disk and returns it as a Sprite
    private Sprite newSprite;
    
    public static void InitStage1()
    {
        Instance = new TileSpriteLoader();
    }
    public static void InitStage2()
    {
        
    }
    //TODO: 32 pixels is 1.0f, fix
    /// <summary>
    /// Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite
    /// </summary>
    /// <returns>Reference to a new created sprite</returns>
    public Sprite LoadNewSprite(string filePath, float pixelsPerUnit = 32.0f)
    {
        //Sprite NewSprite = new Sprite();
        var spriteTexture = LoadTexture(filePath);
       
        //ImageFiles are loaded from files
        //Sprite sheets are made by copying assets from texture files to the sheets
        //TODO: use ImageAssetManager, etc
        newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), pixelsPerUnit);

        return newSprite;
    }

    /// <summary>
    /// Load a PNG or JPG file from disk to a Texture2D
    /// </summary>
    /// <returns>Null if load fails</returns>
    public Texture2D LoadTexture(string filePath)
    {
        if (File.Exists(filePath))
        {
            var fileData = File.ReadAllBytes(filePath);
            var tex2D = new Texture2D(2, 2);
            if (tex2D.LoadImage(fileData)) // Load the imagedata into the texture (size is set automatically)
                return tex2D; // If data = readable -> return texture
        }

        return null; // Return null if load failed
    }
}
