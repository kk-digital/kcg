using UnityEngine;

// test usage for TileSpriteLoader
// just call TileSpriteLoader.LoadSprite and input file name
public class TileSpriteLoaderTest : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer.sprite = TileSpriteLoader.LoadSprite("bedrock.png");
    }

}
