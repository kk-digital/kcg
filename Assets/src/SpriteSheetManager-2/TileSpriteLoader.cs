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
    private Sprite newTexture;
    
    public static void InitStage1()
    {
        Instance = new TileSpriteLoader();
    }
    public static void InitStage2()
    {
        
    }

    //TODO: COMPLETELY WRONG
    public Sprite LoadNewSprite(string filePath, float pixelsPerUnit = 32.0f)
    {
        //TODO: COMPLETELY WRONG
        //LOADING IMAGE IS ONE STEP
        //COPYING SPRITES FROM IMAGE TO ATLAS IS ANOTHER STEP (blitting)
        //MAKING TEXTURE FROM ATLAS IS A THIRD STEP
        var spriteTexture = LoadImageAndCreateTexture(filePath);
        newTexture = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), pixelsPerUnit);
        return newTexture;
    }

    /// <summary>
    /// Load a PNG or JPG file from disk to a Texture2D
    /// </summary>
    /// <returns>Null if load fails</returns>
    public Texture2D LoadImageAndCreateTexture(string filePath)
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
