using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    
    public string filePath = "/SimpleSpriteSheet/Table1.png";
    
    // Start is called before the first frame update
    void Start()
    {
        SpriteSheetManager spriteSheetManager = new SpriteSheetManager();
        spriteSheetManager.GetImageID(filePath); // finds png and add it to struct list
        spriteSheetManager.GetImage(0); // returns the sprite struct of the given index
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
        
    
}
