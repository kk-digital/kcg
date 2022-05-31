using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

namespace Components
{
    // Vehicle Draw Component
    public class VehicleComponentDraw : IComponent
    {
        // Sprite ID
        public int spriteId;
        
        // Image Width
        public int width;

        // Image Height
        public int height;
    }
}
