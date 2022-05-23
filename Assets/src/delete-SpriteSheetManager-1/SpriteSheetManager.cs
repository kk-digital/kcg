using System.Collections.Generic;
using UnityEngine; //Why is unity imported?

//TODO: Dont import unity

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
        Sprite sprite = TileSpriteLoader.Instance.LoadNewSprite(BaseDir + filePath, 32f);
        loadedSprite.id = spriteList.Count;
        //TODO: All tile sprites are 32x32 or scaled to 32x32, dont use rects
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
