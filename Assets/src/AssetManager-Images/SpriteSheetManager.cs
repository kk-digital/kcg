using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SpriteStruct
{
    public int id;
    //public string name;
    //public Color32 rgba;
    public Vector2 size;
    public Sprite sprite;
}
public class SpriteSheetManager
{
    public List<SpriteStruct> spriteList = new List<SpriteStruct>();
    public static string BaseDir => Application.streamingAssetsPath;

    public int GetImageID(string filePath)
    {
       
        SpriteStruct loadedSprite = new SpriteStruct();
        Sprite sprite = SpriteLoader.instance.LoadNewSprite(BaseDir + filePath, 32f);
        loadedSprite.id = spriteList.Count;
        loadedSprite.size = sprite.textureRect.size;
        loadedSprite.sprite = sprite;

        spriteList.Add(loadedSprite);

        return loadedSprite.id;
        
    }

    public SpriteStruct GetImage(int id)
    {

        return spriteList[id];
    }
}
