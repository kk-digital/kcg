using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InteliMap
{
    [Serializable]
    public class GeneratorMap
    {
        [Tooltip("The Tilemap object(s) to open and analyze for the purposes of building the generator. Having multiple entries in this list allows you to create multi-layered tilemaps.")]
        public List<Tilemap> mapLayers = new List<Tilemap>();

        [Tooltip("How 'common' this map should be considered. I.e., make this value low if the map includes rare structures.\n" +
            "These values are normalized, meaning the effective commonality of a map is the commonality of that map, divided by the total commonality of all build maps.")]
        public float commonality = 1.0f;

        [Tooltip("Wether to use manually inputted boundaries, or to just use the entire tilemap.")]
        public bool manualBounds = false;

        [Tooltip("The boundaries of the map to analyze for building the schematic.")]
        public BoundsInt bounds = new BoundsInt(Vector3Int.zero, new Vector3Int(25, 25, 1));

        public GeneratorMap(List<Tilemap> mapLayers, BoundsInt bounds)
        {
            this.mapLayers = new List<Tilemap>(mapLayers);
            commonality = 1.0f;
            manualBounds = true;
            this.bounds = bounds;
        }
    }
}