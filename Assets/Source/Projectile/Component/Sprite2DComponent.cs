using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace Projectile
{
    public struct Sprite2DComponent : IComponent
    {
        public int SpriteID;
        public string SpritePath;

        public Vector2 Size;
        public Vector2Int PngSize;

        public Material Material;
        public Mesh Mesh;
    }
}

