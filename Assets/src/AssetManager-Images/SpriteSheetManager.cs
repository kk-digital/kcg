using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetManager : MonoBehaviour
{
    public static string BaseDir => Application.streamingAssetsPath;
    public string dir = "/SimpleSpriteSheet";
    [System.Serializable]
    public struct spriteStruct
    {
        public int id;
        //public Color32 rgba;
        public Vector2 size;
        public Sprite sprite;
    }

    public List<spriteStruct> spriteList;
    // Start is called before the first frame update
    void Start()
    {
        // foreach(File in folder)?
        spriteStruct loadedSprite = new spriteStruct();
        Sprite sprite = SpriteLoader.instance.LoadNewSprite(BaseDir + dir + "/Tiles_stone_bulkheads.png", 32f);
        loadedSprite.id = 0;
        
        loadedSprite.size = sprite.textureRect.size;
        loadedSprite.sprite = sprite;

        spriteList.Add(loadedSprite);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
