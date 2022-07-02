using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KMath.PerlinNoise;

public class PerlinNoiseTest : MonoBehaviour
{
    // Unlit
    [SerializeField]
    private Material Unlit;

    public int width = 256;
    public int height = 256;

    public float scale = 20;

    private PerlinField2D perlin2D;

    private Texture2D MathFGeneratedPerlin;
    private Texture2D KMathGeneratedPerlin;

    private static bool Init;

    private Sprites.Sprite MathfNoiseTex;
    private Sprites.Sprite KMathNoiseTex;

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
    private void Start()
    {
        perlin2D = new PerlinField2D();

        perlin2D.init(width, height);
        MathFGeneratedPerlin = MathfGenerateTexture();
        KMathGeneratedPerlin = KMathGenerateTexture();

        MathfNoiseTex = new Sprites.Sprite
        {
            Texture = MathFGeneratedPerlin,
            TextureCoords = new Vector4(0, 0, 1, 1)
        };

        KMathNoiseTex = new Sprites.Sprite
        {
            Texture = KMathGeneratedPerlin,
            TextureCoords = new Vector4(0, 0, 1, 1)
        };

        Init = true;
    }

    // Doc: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
    private void OnGUI()
    {
        if(Init)
        {
            // Clear last frame
            foreach (var mr in GetComponentsInChildren<MeshRenderer>())
                if (Application.isPlaying)
                    Destroy(mr.gameObject);
                else
                    DestroyImmediate(mr.gameObject);

            Utility.Render.DrawSprite(-7, 1, 5, 5, MathfNoiseTex, Unlit, transform, 5000);


            Utility.Render.DrawSprite(1.5f, 1, 5, 5, KMathNoiseTex, Unlit, transform, 5000);
        }
    }

    Texture2D MathfGenerateTexture()
    {
        Texture2D newTex = new Texture2D(width, height);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Color color = MathfCalculateColor(x, y);
                newTex.SetPixel(x, y, color);
            }
        }
        newTex.Apply();
        return newTex;
    }

    Texture2D KMathGenerateTexture()
    {
        Texture2D newTex = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = KMathCalculateColor(x, y);
                newTex.SetPixel(x, y, color);
            }
        }
        newTex.Apply();
        return newTex;
    }

    Color MathfCalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }

    Color KMathCalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;

        float sample = perlin2D.noise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
