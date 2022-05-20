using UnityEngine;
using System.Collections;
using System.IO;

//TODO: Dont import unity

//TODO: Should not be mono-behavior
//TODO: Monobehavior for testing can be in Assets/script


//TODO: Rename SpriteLoader -> TileSpriteLoader
//All tiles will be 32x32 pixels, but use a const
public class SpriteLoaderTest : MonoBehaviour
{
    // This script loads a PNG or JPEG image from disk and returns it as a Sprite
    

    private Sprite NewSprite;


    /// TODO, dont do private/getters like this, too much code, no one will ever assign this by accident
    private static SpriteLoaderTest _instance;

    public static SpriteLoaderTest instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.

            //Dont use GameObject/unity
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<SpriteLoaderTest>();
            return _instance;
        }
    }

    //TODO: 32 pixels is 1.0f, fix
    public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f)
    {

        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

        //Sprite NewSprite = new Sprite();
        Texture2D SpriteTexture = LoadTexture(FilePath);
       
        //ImageFiles are loaded from files
        //Sprite sheets are made by copying assets from texture files to the sheets
        //TODO: use ImageAssetManager, etc
        NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit);

        return NewSprite;
    }

    public Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
}
