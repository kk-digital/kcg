using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteliMap
{
    public class Example4Movement : MonoBehaviour
    {
        public Transform mainCamera;
        public InteliMapGenerator bottomSchematic;
        public InteliMapGenerator topSchematic;
        public float speed = 2.0f;
        public int chunkWidth = 10;
        public int chunkOverlap = 2;

        private int lastChunkLocation = 0;
        private float cameraStart;

        private void Start()
        {
            cameraStart = mainCamera.position.x;

            // Set the x size to be equal to the chunk width
            bottomSchematic.boundsToFill.size = new Vector3Int(chunkWidth, bottomSchematic.boundsToFill.size.y, 1);
            topSchematic.boundsToFill.size = new Vector3Int(chunkWidth, topSchematic.boundsToFill.size.y, 1);
        }

        public void Update()
        {
            // Move the main camera forward
            mainCamera.position = new Vector3(mainCamera.position.x + speed * Time.deltaTime, mainCamera.position.y, mainCamera.position.z);

            // If the main camera surpasses some threshold
            if (mainCamera.position.x - cameraStart > lastChunkLocation)
            {
                // Generate the next chunk
                bottomSchematic.StartGeneration();
                topSchematic.StartGeneration();

                // Move the chunks position for the next iterations
                bottomSchematic.boundsToFill.position = new Vector3Int(bottomSchematic.boundsToFill.position.x + chunkWidth - chunkOverlap, bottomSchematic.boundsToFill.position.y);
                topSchematic.boundsToFill.position = new Vector3Int(topSchematic.boundsToFill.position.x + chunkWidth - chunkOverlap, topSchematic.boundsToFill.position.y);
                lastChunkLocation += chunkWidth - chunkOverlap;
            }
        }
    }
}