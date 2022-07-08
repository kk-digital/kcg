using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum GeneratorBuildResult
{
    None,
    InProgress,
    
    // Warning Messages
    Cancelled,

    // Success Messages
    Success,

    // Error Messages
    NanError,
    MismatchedLayers,
    ZeroMaps,
    NullMaps,
    InvalidCommonality
}

public enum BuildMode
{
    FromScratch,
    FromScratchOverwrite,
    LoadFromGenerator
}

namespace InteliMap
{
    public class InteliMapBuilder : MonoBehaviour
    {
        [Header("General")]
        [Tooltip("How many times to analyze the build maps. Higher values will result in longer build times and more accurate generation.")]
        public int epochs = 1000;
        [Tooltip("Wether to train the machine learning model from scratch, or loading an existing one from a Generator on this gameobject.")]
        public BuildMode buildMode = BuildMode.FromScratch;
        [Tooltip("The size of a tiles 'neighborhood'. A tiles neighborhood is all the nearby tiles that are relevent to deciding what that tile is going to be. Ex. A radius of 1 implies a 3x3 area, a radius of 2 implies a 5x5 area, etc...")]
        [Range(1, 10)] public int neighborhoodRadius = 2;

        [Header("Maps")]
        [Tooltip("The list of Tilemaps to analyze and build the generator from.")]
        public List<GeneratorMap> buildMaps;

        [Serializable]
        public class InteliMapBuilderAdvanced
        {
            [Tooltip("The starting learning rate of the machine learning model. Higher values may result in faster generation, but going too high may result in unexpected behaviour. This value is logarithmically interpolated with the End Learning Rate throughout the build.")]
            public float startLearningRate = 0.1f;
            [Tooltip("The ending learning rate of the machine learning model. In most cases this should be lower than the starting learning rate. This value is logarithmically interpolated with the Start Learning Rate throughout the build.")]
            public float endLearningRate = 0.001f;
            [Tooltip("How to enforce which tiles can connect to which other tiles. Four way connectivity means only the orthogonal connections are enforced, eight way means diagonal connections are also enforced.")]
            public Connectivity connectivity = Connectivity.FourWay;
            [Tooltip("Wether to interpret empty tiles as intentionally empty tiles. If this is true, then empty tiles may be placed during generation; if it is false, then empty tiles will never be placed during generation.")]
            public bool interpretEmptyAsTile = false;
            [Tooltip("What boundaries of the generation bounds to use as an input during training. This will cause generators built with this option set to true to correlate the selected boundaries of the generation with structures that are seen around the selected boundaries.")]
            public DirectionalBools acknowledgeBounds;
            [Tooltip("Wether to enforce what tiles are allowed to be connected to the selected edges of the generation border.")]
            public DirectionalBools enforceBorderConnectivity;
        }

        [Header("Advanced")]
        [Tooltip("A collection of all the advanced settings for the InteliMapBuilder.")]
        public InteliMapBuilderAdvanced advanced;

        [HideInInspector] public int epoch;
        [HideInInspector] public int totalEpochs;
        [HideInInspector] public float lossLastEpoch;
        [HideInInspector] public float avgLossLast20Epochs;
        [HideInInspector] public float currentLearningRate;
        [HideInInspector] public DateTime startTime;
        [HideInInspector] public DateTime endTime;
        [HideInInspector] public GeneratorBuildResult buildResult = GeneratorBuildResult.None;

        [HideInInspector] private Thread trainThread;
        [HideInInspector] private bool shouldSaveAndQuit;

        public void OnEnable()
        {
            buildResult = GeneratorBuildResult.None;
        }

        /**
         * Cancels the current build if there is one running. Note that if a generator was already overwritten it can not be retrieved by canceling the build.
         */
        public void CancelBuild()
        {
            if (trainThread != null)
            {
                trainThread.Abort();
            }

            buildResult = GeneratorBuildResult.Cancelled;
        }

        /**
         * Save and quits the current build if there is one running. It will wait until the end of the current epoch to stop.
         */
        public void SaveAndQuitBuild()
        {
            shouldSaveAndQuit = true;
        }

        /**
         * Builds a generator according to the attributes of this builder.
         */
        public void Build()
        {
            shouldSaveAndQuit = false;
            startTime = DateTime.Now;

            if (buildMaps.Count == 0)
            {
                buildResult = GeneratorBuildResult.ZeroMaps;
                return;
            }

            int layerCount = 0;
            float totalCommonality = 0.0f;
            for (int i = 0; i < buildMaps.Count; i++)
            {
                if (buildMaps[i].mapLayers == null || buildMaps[i].mapLayers.Count == 0)
                {
                    buildResult = GeneratorBuildResult.NullMaps;
                    return;
                }
                if (buildMaps[i].commonality < 0.0f)
                {
                    buildResult = GeneratorBuildResult.InvalidCommonality;
                    return;
                }

                if (buildMaps[i].bounds.size.z == 0)
                {
                    buildMaps[i].bounds = new BoundsInt(buildMaps[i].bounds.position, new Vector3Int(buildMaps[i].bounds.size.x, buildMaps[i].bounds.size.y, 1));
                }

                if (i == 0)
                {
                    layerCount = buildMaps[i].mapLayers.Count;
                }
                else
                {
                    if (buildMaps[i].mapLayers.Count != layerCount)
                    {
                        buildResult = GeneratorBuildResult.MismatchedLayers;
                        return;
                    }
                }

                totalCommonality += buildMaps[i].commonality;
            }
            if (totalCommonality <= 0.0f)
            {
                buildResult = GeneratorBuildResult.InvalidCommonality;
                return;
            }

            // Collect all the tile data
            BoundsInt[] bounds = new BoundsInt[buildMaps.Count];
            int[][] tileIndicies = new int[buildMaps.Count][];

            for (int i = 0; i < buildMaps.Count; i++)
            {
                bounds[i] = GetBoundsOfBuildMap(buildMaps[i]);
                tileIndicies[i] = new int[bounds[i].size.x * bounds[i].size.y];
            }

            CSPConnectivity connectivity;
            LayeredTile[] uniqueTiles = GetUniqueTilesAndIndicies(bounds, tileIndicies, layerCount, out connectivity);

            InteliMapGenerator ss = gameObject.GetComponent<InteliMapGenerator>();
            if (ss == null || buildMode == BuildMode.FromScratch)
            {
                ss = gameObject.AddComponent<InteliMapGenerator>();
            }

            // Indexed by tileToPlace, x, y, tileAtLocation
            GeneratorWeights weights;
            if (buildMode == BuildMode.FromScratch || buildMode == BuildMode.FromScratchOverwrite)
            {
                weights = new GeneratorWeights(uniqueTiles.Length, neighborhoodRadius, advanced.acknowledgeBounds);
                totalEpochs = 0;
            }
            else
            {
                weights = ss.weights;
                neighborhoodRadius = ss.GetNeighborhoodRadius();
                totalEpochs = weights.epochsTrained;
            }

            ss.Build(buildMaps[0].mapLayers, layerCount, uniqueTiles, connectivity, weights, neighborhoodRadius);
            buildResult = GeneratorBuildResult.InProgress;

            trainThread = new Thread(() =>
            {
                TrainWeights(bounds, tileIndicies, totalCommonality, uniqueTiles.Length, connectivity, weights, ss);
            });
            trainThread.Priority = System.Threading.ThreadPriority.Highest;
            trainThread.Start();
        }

        // Returns an array of all the unique tiles. Also fills the tileIndicies with the indicies of each tile in the unique list.
        // Also generates an array of how many times each tile occured. And generates CSP Connectivity.
        LayeredTile[] GetUniqueTilesAndIndicies(BoundsInt[] bounds, int[][] tileIndicies, int layerCount, out CSPConnectivity connectivity)
        {
            // Maps each tile to its corresponding index in the eventually returned unique array
            Dictionary<LayeredTile, int> uniqueTiles = new Dictionary<LayeredTile, int>(new LayeredTileComparer());

            for (int i = 0; i < buildMaps.Count; i++)
            {
                TileBase[][] tiles = new TileBase[layerCount][];
                for (int layer = 0; layer < layerCount; layer++)
                {
                    tiles[layer] = buildMaps[i].mapLayers[layer].GetTilesBlock(bounds[i]);
                }

                for (int x = 0; x < bounds[i].size.x; x++)
                {
                    for (int y = 0; y < bounds[i].size.y; y++)
                    {
                        TileBase[] tileArr = new TileBase[layerCount];
                        for (int layer = 0; layer < layerCount; layer++)
                        {
                            tileArr[layer] = tiles[layer][x + y * bounds[i].size.x];
                        }
                        LayeredTile tile = new LayeredTile(tileArr);

                        if (!tile.IsEmpty())
                        {
                            int outIndex;
                            if (uniqueTiles.TryGetValue(tile, out outIndex))
                            {
                                tileIndicies[i][x + y * bounds[i].size.x] = outIndex;
                            }
                            else
                            {
                                tileIndicies[i][x + y * bounds[i].size.x] = uniqueTiles.Count;

                                uniqueTiles.Add(tile, uniqueTiles.Count);
                            }
                        }
                        else
                        {
                            tileIndicies[i][x + y * bounds[i].size.x] = -1;
                        }
                    }
                }
            }

            LayeredTile[] uniques = new LayeredTile[advanced.interpretEmptyAsTile ? uniqueTiles.Count + 1 : uniqueTiles.Count];

            foreach (var pair in uniqueTiles)
            {
                uniques[pair.Value] = pair.Key;
            }

            if (advanced.interpretEmptyAsTile)
            {
                uniques[uniqueTiles.Count] = new LayeredTile(layerCount);

                for (int i = 0; i < buildMaps.Count; i++)
                {
                    for (int x = 0; x < bounds[i].size.x; x++)
                    {
                        for (int y = 0; y < bounds[i].size.y; y++)
                        {
                            if (tileIndicies[i][x + y * bounds[i].size.x] == -1)
                            {
                                tileIndicies[i][x + y * bounds[i].size.x] = uniqueTiles.Count;
                            }
                        }
                    }
                }
            }

            // Generate connectivitity
            connectivity = new CSPConnectivity(advanced.connectivity, uniques.Length, advanced.enforceBorderConnectivity);

            for (int i = 0; i < buildMaps.Count; i++)
            {
                for (int x = 0; x < bounds[i].size.x; x++)
                {
                    for (int y = 0; y < bounds[i].size.y; y++)
                    {
                        int tileIdx = tileIndicies[i][x + y * bounds[i].size.x];
                        if (tileIdx != -1)
                        {
                            AddToConnectivity(connectivity, x, y, tileIdx, bounds[i], tileIndicies[i], uniqueTiles.Count);
                        }
                        else if (advanced.interpretEmptyAsTile)
                        {
                            AddToConnectivity(connectivity, x, y, uniqueTiles.Count, bounds[i], tileIndicies[i], uniqueTiles.Count);
                        }
                    }
                }
            }

            return uniques;
        }

        void AddToConnectivity(CSPConnectivity connectivity, int x, int y, int tileIdx, BoundsInt bounds, int[] indicies, int emptyIndex)
        {
            if (y > 0) // bottom
            {
                int tb = indicies[x + (y - 1) * bounds.size.x];
                if (tb != -1)
                {
                    connectivity.SetBottomConnectivity(tileIdx, tb, true);
                }
                else if (advanced.interpretEmptyAsTile)
                {
                    connectivity.SetBottomConnectivity(tileIdx, emptyIndex, true);
                }
            }
            else if (advanced.enforceBorderConnectivity.bottom)
            {
                connectivity.SetBottomBorderConnectivity(tileIdx, true);
            }

            if (y < bounds.size.y - 1) // top
            {
                int tt = indicies[x + (y + 1) * bounds.size.x];
                if (tt != -1)
                {
                    connectivity.SetTopConnectivity(tileIdx, tt, true);
                }
                else if (advanced.interpretEmptyAsTile)
                {
                    connectivity.SetTopConnectivity(tileIdx, emptyIndex, true);
                }
            }
            else if (advanced.enforceBorderConnectivity.top)
            {
                connectivity.SetTopBorderConnectivity(tileIdx, true);
            }

            if (x > 0) // left
            {
                int tl = indicies[x - 1 + y * bounds.size.x];
                if (tl != -1)
                {
                    connectivity.SetLeftConnectivity(tileIdx, tl, true);
                }
                else if (advanced.interpretEmptyAsTile)
                {
                    connectivity.SetLeftConnectivity(tileIdx, emptyIndex, true);
                }
            }
            else if (advanced.enforceBorderConnectivity.left)
            {
                connectivity.SetLeftBorderConnectivity(tileIdx, true);
            }

            if (x < bounds.size.x - 1) // right
            {
                int tr = indicies[x + 1 + y * bounds.size.x];
                if (tr != -1)
                {
                    connectivity.SetRightConnectivity(tileIdx, tr, true);
                }
                else if (advanced.interpretEmptyAsTile)
                {
                    connectivity.SetRightConnectivity(tileIdx, emptyIndex, true);
                }
            }
            else if (advanced.enforceBorderConnectivity.right)
            {
                connectivity.SetRightBorderConnectivity(tileIdx, true);
            }

            if (advanced.connectivity == Connectivity.EightWay)
            {
                if (y > 0 && x > 0) // bottom left
                {
                    int tbl = indicies[(x - 1) + (y - 1) * bounds.size.x];
                    if (tbl != -1)
                    {
                        connectivity.SetBottomLeftConnectivity(tileIdx, tbl, true);
                    }
                    else if (advanced.interpretEmptyAsTile)
                    {
                        connectivity.SetBottomLeftConnectivity(tileIdx, emptyIndex, true);
                    }
                }
                if (y > 0 && x < bounds.size.x - 1) // bottom right
                {
                    int tbr = indicies[(x + 1) + (y - 1) * bounds.size.x];
                    if (tbr != -1)
                    {
                        connectivity.SetBottomRightConnectivity(tileIdx, tbr, true);
                    }
                    else if (advanced.interpretEmptyAsTile)
                    {
                        connectivity.SetBottomRightConnectivity(tileIdx, emptyIndex, true);
                    }
                }

                if (y < bounds.size.y - 1 && x > 0) // top left
                {
                    int ttl = indicies[(x - 1) + (y + 1) * bounds.size.x];
                    if (ttl != -1)
                    {
                        connectivity.SetTopLeftConnectivity(tileIdx, ttl, true);
                    }
                    else if (advanced.interpretEmptyAsTile)
                    {
                        connectivity.SetTopLeftConnectivity(tileIdx, emptyIndex, true);
                    }
                }
                if (y < bounds.size.y - 1 && x < bounds.size.x - 1) // top right
                {
                    int ttr = indicies[(x + 1) + (y + 1) * bounds.size.x];
                    if (ttr != -1)
                    {
                        connectivity.SetTopRightConnectivity(tileIdx, ttr, true);
                    }
                    else if (advanced.interpretEmptyAsTile)
                    {
                        connectivity.SetTopRightConnectivity(tileIdx, emptyIndex, true);
                    }
                }
            }
        }

        float Logerp(float a, float b, float t)
        {
            return a * Mathf.Pow(b / a, t);
        }

        void TrainWeights(BoundsInt[] bounds, int[][] tileIndicies, float totalCommonality, int uniqueCount, CSPConnectivity connectivity, GeneratorWeights weights, InteliMapGenerator ss)
        {
            float[] lastTwenty = new float[20];

            lossLastEpoch = 0.0f;
            avgLossLast20Epochs = 0.0f;

            GeneratorEngine engine = null;

            int previouslyChosenMap = -1;
            System.Random threadRand = new System.Random();

            for (epoch = 0; epoch < epochs && !shouldSaveAndQuit; epoch++)
            {
                float epochT = (float)epoch / (float)epochs;
                currentLearningRate = Logerp(advanced.startLearningRate, advanced.endLearningRate, epochT);

                // Pick random map based on commonality
                float val = (float)threadRand.NextDouble() * totalCommonality;
                int chosenMap = buildMaps.Count - 1;
                for (int i = 0; i < buildMaps.Count; i++)
                {
                    val -= buildMaps[i].commonality;
                    if (val < 0.0f)
                    {
                        chosenMap = i;
                        break;
                    }
                }

                // Initialize the engine to train the weights
                if (chosenMap != previouslyChosenMap)
                {
                    engine = new GeneratorEngine(weights, neighborhoodRadius, threadRand, bounds[chosenMap], uniqueCount);

                    previouslyChosenMap = chosenMap;
                }

                int[] chosenIndicies = new int[bounds[chosenMap].size.x * bounds[chosenMap].size.y];
                for (int i = 0; i < chosenIndicies.Length; i++)
                {
                    chosenIndicies[i] = -1;
                }

                engine.Reset(threadRand, 0.1f, chosenIndicies);

                float totalLoss = 0.0f;

                // Train the main mode
                while (!engine.IsDone())
                {
                    Vector2Int pos = engine.NextPos();

                    int idx = tileIndicies[chosenMap][pos.x + pos.y * bounds[chosenMap].size.x];
                    if (idx != -1)
                    {
                        totalLoss += engine.Train(pos, chosenIndicies, idx, currentLearningRate);
                        chosenIndicies[pos.x + pos.y * bounds[chosenMap].size.x] = idx;
                    }
                }

                totalLoss /= bounds[chosenMap].size.x * bounds[chosenMap].size.y;
                if (float.IsNaN(totalLoss)) // returned a null loss value, therefore the training has been broken and should terminate
                {
                    buildResult = GeneratorBuildResult.NanError;
                    return;
                }

                lastTwenty[epoch % 20] = totalLoss;
                lossLastEpoch = totalLoss;

                int cap = Mathf.Min(20, epoch + 1);
                float sum = 0.0f;
                for (int i = 0; i < cap; i++)
                {
                    sum += lastTwenty[i];
                }

                avgLossLast20Epochs = sum / cap;

                weights.epochsTrained++;
                totalEpochs++;
            }

            buildResult = GeneratorBuildResult.Success;

            endTime = DateTime.Now;
        }

        private BoundsInt GetBoundsOfBuildMap(GeneratorMap map)
        {
            if (map.manualBounds)
            {
                return map.bounds;
            }

            // otherwise get the maximum extent of all the layers
            BoundsInt firstBounds = map.mapLayers[0].cellBounds;
            int furthestLeft = firstBounds.xMin;
            int furthestRight = firstBounds.xMax;
            int furthestDown = firstBounds.yMin;
            int furthestUp = firstBounds.yMax;
            for (int i = 1; i < map.mapLayers.Count; i++)
            {
                BoundsInt bounds = map.mapLayers[i].cellBounds;

                if (bounds.xMin < furthestLeft)
                {
                    furthestLeft = bounds.xMin;
                }
                if (bounds.xMax > furthestRight)
                {
                    furthestRight = bounds.xMax;
                }
                if (bounds.yMin < furthestDown)
                {
                    furthestDown = bounds.yMin;
                }
                if (bounds.yMax > furthestUp)
                {
                    furthestUp = bounds.yMax;
                }
            }

            return new BoundsInt(furthestLeft, furthestDown, 0, furthestRight - furthestLeft + 1, furthestUp - furthestDown + 1, 1);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (buildMaps != null)
            {
                foreach (GeneratorMap map in buildMaps)
                {
                    BoundsInt bounds = GetBoundsOfBuildMap(map);
                    foreach (Tilemap tilemap in map.mapLayers)
                    {
                        TileSelectionGizmos.DrawBounds(tilemap, bounds);
                    }
                }
            }
        }
    }
}