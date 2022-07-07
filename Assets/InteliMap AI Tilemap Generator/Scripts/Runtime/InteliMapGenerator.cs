using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InteliMap
{
    public class InteliMapGenerator : MonoBehaviour
    {
        [Header("General")]
        [Tooltip("The tilemap to fill by generating. Each seperate entry into the list should be a different layer of the map to fill.")]
        public List<Tilemap> mapToFill;
        [Tooltip("The bounds of the given tilemap to fill by generating. Already placed tiles will be incorporated into the new generation in-place.")]
        public BoundsInt boundsToFill = new BoundsInt(new Vector3Int(0, 0), new Vector3Int(25, 25, 1));
        [Tooltip("Wether to start generation upon the scene starting.")]
        public bool generateOnStart = true;
        [Tooltip("When this is set to true, if the generator encounters an area impossible to generate in, instead of throw an exception, it will forcefully generate by changing some of the previously set tiles.")]
        public bool forceful = true;
        [Tooltip("Positive values of this will likely result in more random generation, while negative values may result in more consistent generation.")]
        public float temperature = 0;

        [HideInInspector][SerializeField] public int layerCount = 1;
        [HideInInspector][SerializeField] public LayeredTile[] tiles;
        [HideInInspector][SerializeField] public GeneratorWeights weights;
        [HideInInspector][SerializeField] public CSPConnectivity connectivity;
        [HideInInspector][SerializeField] private int neighborhoodRadius;

        public int NumUniqueTiles() { return tiles.Length; }
        public int GetNeighborhoodRadius() { return neighborhoodRadius; }
        public int GetWeightCount() { return weights.GetLength(); }

        void Start()
        {
            if (generateOnStart)
            {
                StartGeneration();
            }
        }

        /**
         * Clears the area of the mapToFill within the boundsToFill.
         */
        public void ClearBounds()
        {
            foreach (Tilemap map in mapToFill)
            {
                map.SetTilesBlock(boundsToFill, new TileBase[boundsToFill.size.x * boundsToFill.size.y]);
            }
        }

        /**
         * Starts the map generation with the given seed.
         * Throws System.ArgumentException if it is impossible to generate a valid map within the given bounds and forceful is set to false. This can often be fixed by removing some nearby tiles or making the 
         * example build maps larger so that it has more connection information, then try again. It can also be fixed by setting forceful to true, provided it is acceptable to remove some previously placed tiles.
         */
        public void StartGenerationWithSeed(int seed)
        {
            Random.State before = Random.state;

            Random.InitState(seed);
            StartGeneration();

            Random.state = before;
        }

        /**
         * Starts the map generation.
         * Throws System.ArgumentException if it is impossible to generate a valid map within the given bound and forceful is set to falses. This can often be fixed by removing some nearby tiles or making the 
         * example build maps larger so that it has more connection information, then try again. It can also be fixed by setting forceful to true, provided it is acceptable to remove some previously placed tiles.
         */
        public void StartGeneration()
        {
            if (tiles == null || tiles.Length == 0)
            {
                Debug.LogError("ERROR. The internal list of unique tiles is empty. This is likely a result of updating to v1.1. To fix this, either build a new generator, or you can update this " +
                    "generator by placing it on the same object as its builder, then setting build mode to LoadFromGenerator and epochs to 0, then rebuild.");
                return;
            }

            if (mapToFill.Count != layerCount)
            {
                Debug.LogError($"ERROR. This generator was built with {layerCount} layers. But this generator's mapToFill has {mapToFill.Count} layers. These layer counts must match.");
                return;
            }

            // maps each position in the grid to a dictionary containing a position of what else is in conflict, and that positions current index. If the index at that position isn't the corresponding value in the dictionary, it is not in conflict.
            SparseSet[,] domains = new SparseSet[boundsToFill.size.x, boundsToFill.size.y];

            List<HighPriority> highPriority = new List<HighPriority>();
            int[] mapIndicies = GetMapIndiciesAndSetDomains(domains, highPriority);

            GeneratorEngine engine = new GeneratorEngine(weights, neighborhoodRadius, new System.Random(Random.Range(int.MinValue, int.MaxValue)), boundsToFill, tiles.Length);

            if (AddPreexistingToMAC(mapIndicies, domains))
            {
                engine.Reset(new System.Random(Random.Range(int.MinValue, int.MaxValue)), 0.1f, mapIndicies);

                List<Vector2Int> remainingPositions = new List<Vector2Int>();
                int[,] engineCollapsedTo = new int[boundsToFill.size.x, boundsToFill.size.y];

                for (int i = 0; !engine.IsDone(); i++)
                {
                    Vector2Int next = engine.NextPos();

                    remainingPositions.Add(next);

                    engineCollapsedTo[next.x, next.y] = engine.PredictAndCollapse(next, -temperature);
                }

                DoCSPFilling(remainingPositions, engineCollapsedTo, mapIndicies, domains, engine, new List<HighPriority>());
            }
            else
            {
                if (!forceful)
                {
                    throw new System.ArgumentException("ERROR. Impossible to generate in the given area. Try removing some nearby tiles or making the example build maps larger, then try again. Alternatively turn on the 'forceful' setting.");
                }

                engine.Reset(new System.Random(Random.Range(int.MinValue, int.MaxValue)), 0.1f, mapIndicies);

                List<Vector2Int> remainingPositions = new List<Vector2Int>();
                int[,] engineCollapsedTo = new int[boundsToFill.size.x, boundsToFill.size.y];

                for (int i = 0; !engine.IsDone(); i++)
                {
                    Vector2Int next = engine.NextPos();

                    engineCollapsedTo[next.x, next.y] = engine.PredictAndCollapse(next, -temperature);
                }

                // Set the entire map to blank and reset domains
                for (int x = 0; x < boundsToFill.size.x; x++)
                {
                    for (int y = 0; y < boundsToFill.size.y; y++)
                    {
                        mapIndicies[x + y * boundsToFill.size.x] = -1;

                        domains[x, y] = new SparseSet(tiles.Length, true);

                        remainingPositions.Add(new Vector2Int(x, y));
                    }
                }

                if (AddPreexistingToMAC(mapIndicies, domains))
                {
                    // Then call CSP filling with the new high priority assignments
                    DoCSPFilling(remainingPositions, engineCollapsedTo, mapIndicies, domains, engine, highPriority);
                }
                else
                {
                    throw new System.ArgumentException("Completely impossible to generate in this area with the current constraints. This is likely due to enforcing border connectivity. To fix this either generate in a larger area, or give more information to the builder and rebuild.");
                }
            }
        }

        public void Build(List<Tilemap> mapToFill, int layerCount, LayeredTile[] tiles, CSPConnectivity connectivity, GeneratorWeights weights, int neighborhoodRadius)
        {
            this.layerCount = layerCount;
            this.mapToFill = new List<Tilemap>(mapToFill);
            this.tiles = tiles;
            this.weights = weights;
            this.connectivity = connectivity;
            this.neighborhoodRadius = neighborhoodRadius;
        }

        private bool AddPreexistingToMAC(int[] mapIndicies, SparseSet[,] domains)
        {
            Queue<ReviseItem> frontier = new Queue<ReviseItem>();
            List<Assignment> domainValuesRemoved = new List<Assignment>();

            for (int x = 0; x < boundsToFill.size.x; x++)
            {
                for (int y = 0; y < boundsToFill.size.y; y++)
                {
                    frontier.Enqueue(new ReviseItem(new Vector2Int(x, y), Direction.Bottom));
                    frontier.Enqueue(new ReviseItem(new Vector2Int(x, y), Direction.Top));
                    frontier.Enqueue(new ReviseItem(new Vector2Int(x, y), Direction.Left));
                    frontier.Enqueue(new ReviseItem(new Vector2Int(x, y), Direction.Right));

                    if (connectivity.con == Connectivity.EightWay)
                    {
                        frontier.Enqueue(new ReviseItem(new Vector2Int(x, y), Direction.BottomLeft));
                        frontier.Enqueue(new ReviseItem(new Vector2Int(x, y), Direction.BottomRight));
                        frontier.Enqueue(new ReviseItem(new Vector2Int(x, y), Direction.TopLeft));
                        frontier.Enqueue(new ReviseItem(new Vector2Int(x, y), Direction.TopRight));
                    }
                }
            }

            while (frontier.Count > 0)
            {
                ReviseItem next = frontier.Dequeue();

                switch (next.dir)
                {
                    case Direction.Bottom:
                        if (ReviseBottom(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                foreach (Assignment restoreDomain in domainValuesRemoved)
                                {
                                    domains[restoreDomain.pos.x, restoreDomain.pos.y].Add(restoreDomain.index);
                                }

                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.Top:
                        if (ReviseTop(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                foreach (Assignment restoreDomain in domainValuesRemoved)
                                {
                                    domains[restoreDomain.pos.x, restoreDomain.pos.y].Add(restoreDomain.index);
                                }

                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.Left:
                        if (ReviseLeft(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                foreach (Assignment restoreDomain in domainValuesRemoved)
                                {
                                    domains[restoreDomain.pos.x, restoreDomain.pos.y].Add(restoreDomain.index);
                                }

                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.Right:
                        if (ReviseRight(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                foreach (Assignment restoreDomain in domainValuesRemoved)
                                {
                                    domains[restoreDomain.pos.x, restoreDomain.pos.y].Add(restoreDomain.index);
                                }

                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.BottomLeft:
                        if (ReviseBottomLeft(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                foreach (Assignment restoreDomain in domainValuesRemoved)
                                {
                                    domains[restoreDomain.pos.x, restoreDomain.pos.y].Add(restoreDomain.index);
                                }

                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.BottomRight:
                        if (ReviseBottomRight(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                foreach (Assignment restoreDomain in domainValuesRemoved)
                                {
                                    domains[restoreDomain.pos.x, restoreDomain.pos.y].Add(restoreDomain.index);
                                }

                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.TopLeft:
                        if (ReviseTopLeft(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                foreach (Assignment restoreDomain in domainValuesRemoved)
                                {
                                    domains[restoreDomain.pos.x, restoreDomain.pos.y].Add(restoreDomain.index);
                                }

                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.TopRight:
                        if (ReviseTopRight(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                foreach (Assignment restoreDomain in domainValuesRemoved)
                                {
                                    domains[restoreDomain.pos.x, restoreDomain.pos.y].Add(restoreDomain.index);
                                }

                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                }
            }

            return true;
        }

        struct CSPState
        {
            public CSPState(Vector2Int pos, int enginePrediction, SparseSet[,] domains, CSPConnectivity connectivity, BoundsInt bounds, GeneratorEngine engine, float temperature, int[] mapIndicies, bool isHighPriority)
            {
                this.pos = pos;
                this.isHighPriority = isHighPriority;

                domain = domains[pos.x, pos.y].ToArray();
                toRestoreDomain = new List<Assignment>();

                if (System.Array.IndexOf(domain, enginePrediction) == -1) // if the initial engine prediction is not available, make a new prediction
                {
                    enginePrediction = engine.CalculateAndPredict(pos, -temperature, mapIndicies);
                }

                // Sort the domain according to the LCV heuristic
                int[] domainSizes = new int[domain.Length];
                for (int i = 0; i < domain.Length; i++)
                {
                    if (domain[i] == enginePrediction) // if it matches engine prediction, put it first
                    {
                        domainSizes[i] = -999999;
                        continue;
                    }

                    if (pos.y > 0) // bottom
                    {
                        foreach (int bottom in domains[pos.x, pos.y - 1])
                        {
                            if (!connectivity.GetBottomConnectivity(domain[i], bottom))
                            {
                                domainSizes[i]++;
                            }
                        }
                    }

                    if (pos.y < bounds.size.y - 1) // top
                    {
                        foreach (int top in domains[pos.x, pos.y + 1])
                        {
                            if (!connectivity.GetTopConnectivity(domain[i], top))
                            {
                                domainSizes[i]++;
                            }
                        }
                    }

                    if (pos.x > 0) // left
                    {
                        foreach (int left in domains[pos.x - 1, pos.y])
                        {
                            if (!connectivity.GetLeftConnectivity(domain[i], left))
                            {
                                domainSizes[i]++;
                            }
                        }
                    }

                    if (pos.x < bounds.size.x - 1) // right
                    {
                        foreach (int right in domains[pos.x + 1, pos.y])
                        {
                            if (!connectivity.GetRightConnectivity(domain[i], right))
                            {
                                domainSizes[i]++;
                            }
                        }
                    }

                    if (connectivity.con == Connectivity.EightWay)
                    {
                        if (pos.y > 0 && pos.x > 0) // bottom left
                        {
                            foreach (int bottomLeft in domains[pos.x - 1, pos.y - 1])
                            {
                                if (!connectivity.GetBottomLeftConnectivity(domain[i], bottomLeft))
                                {
                                    domainSizes[i]++;
                                }
                            }
                        }

                        if (pos.y > 0 && pos.x < bounds.size.x - 1) // bottom right
                        {
                            foreach (int bottomRight in domains[pos.x + 1, pos.y - 1])
                            {
                                if (!connectivity.GetBottomRightConnectivity(domain[i], bottomRight))
                                {
                                    domainSizes[i]++;
                                }
                            }
                        }

                        if (pos.y < bounds.size.y - 1 && pos.x > 0) // top left
                        {
                            foreach (int topLeft in domains[pos.x - 1, pos.y + 1])
                            {
                                if (!connectivity.GetTopLeftConnectivity(domain[i], topLeft))
                                {
                                    domainSizes[i]++;
                                }
                            }
                        }

                        if (pos.y < bounds.size.y - 1 && pos.x < bounds.size.x - 1) // top right
                        {
                            foreach (int topLeft in domains[pos.x + 1, pos.y + 1])
                            {
                                if (!connectivity.GetTopRightConnectivity(domain[i], topLeft))
                                {
                                    domainSizes[i]++;
                                }
                            }
                        }
                    }
                }

                System.Array.Sort(domainSizes, domain); // sorts the domain smallest domain sizes first

                iterIndex = 0;
                expectingReturn = false;
            }

            public CSPState(HighPriority hp, SparseSet[,] domains, CSPConnectivity connectivity, BoundsInt bounds, GeneratorEngine engine, float temperature, int[] mapIndicies, bool isHighPriority)
            {
                this.pos = hp.pos;
                this.isHighPriority = isHighPriority;

                domain = domains[pos.x, pos.y].ToArray();
                toRestoreDomain = new List<Assignment>();

                // Sort the domain according to the LCV heuristic
                int[] domainSizes = new int[domain.Length];
                for (int i = 0; i < domain.Length; i++)
                {
                    if (hp.set.Contains(domain[i])) // if it matches the high priority set, put it first
                    {
                        domainSizes[i] += -999999;
                    }

                    if (pos.y > 0) // bottom
                    {
                        foreach (int bottom in domains[pos.x, pos.y - 1])
                        {
                            if (!connectivity.GetBottomConnectivity(domain[i], bottom))
                            {
                                domainSizes[i]++;
                            }
                        }
                    }

                    if (pos.y < bounds.size.y - 1) // top
                    {
                        foreach (int top in domains[pos.x, pos.y + 1])
                        {
                            if (!connectivity.GetTopConnectivity(domain[i], top))
                            {
                                domainSizes[i]++;
                            }
                        }
                    }

                    if (pos.x > 0) // left
                    {
                        foreach (int left in domains[pos.x - 1, pos.y])
                        {
                            if (!connectivity.GetLeftConnectivity(domain[i], left))
                            {
                                domainSizes[i]++;
                            }
                        }
                    }

                    if (pos.x < bounds.size.x - 1) // right
                    {
                        foreach (int right in domains[pos.x + 1, pos.y])
                        {
                            if (!connectivity.GetRightConnectivity(domain[i], right))
                            {
                                domainSizes[i]++;
                            }
                        }
                    }

                    if (connectivity.con == Connectivity.EightWay)
                    {
                        if (pos.y > 0 && pos.x > 0) // bottom left
                        {
                            foreach (int bottomLeft in domains[pos.x - 1, pos.y - 1])
                            {
                                if (!connectivity.GetBottomLeftConnectivity(domain[i], bottomLeft))
                                {
                                    domainSizes[i]++;
                                }
                            }
                        }

                        if (pos.y > 0 && pos.x < bounds.size.x - 1) // bottom right
                        {
                            foreach (int bottomRight in domains[pos.x + 1, pos.y - 1])
                            {
                                if (!connectivity.GetBottomRightConnectivity(domain[i], bottomRight))
                                {
                                    domainSizes[i]++;
                                }
                            }
                        }

                        if (pos.y < bounds.size.y - 1 && pos.x > 0) // top left
                        {
                            foreach (int topLeft in domains[pos.x - 1, pos.y + 1])
                            {
                                if (!connectivity.GetTopLeftConnectivity(domain[i], topLeft))
                                {
                                    domainSizes[i]++;
                                }
                            }
                        }

                        if (pos.y < bounds.size.y - 1 && pos.x < bounds.size.x - 1) // top right
                        {
                            foreach (int topLeft in domains[pos.x + 1, pos.y + 1])
                            {
                                if (!connectivity.GetTopRightConnectivity(domain[i], topLeft))
                                {
                                    domainSizes[i]++;
                                }
                            }
                        }
                    }
                }

                System.Array.Sort(domainSizes, domain); // sorts the domain smallest domain sizes first

                iterIndex = 0;
                expectingReturn = false;
            }

            public List<Assignment> toRestoreDomain;
            public Vector2Int pos;
            public int[] domain;
            public int iterIndex;
            public bool expectingReturn;
            public bool isHighPriority;
        }

        private void DoCSPFilling(List<Vector2Int> remainingPositions, int[,] engineCollapsedTo, int[] mapIndicies, SparseSet[,] domains, GeneratorEngine engine, List<HighPriority> highPriority)
        {
            Stack<CSPState> stack = new Stack<CSPState>();
            float[,] noise = new float[boundsToFill.size.x, boundsToFill.size.y];
            for (int x = 0; x < boundsToFill.size.x; x++)
            {
                for (int y = 0; y < boundsToFill.size.y; y++)
                {
                    noise[x, y] = Random.value;
                }
            }

            if (highPriority.Count > 0)
            {
                HighPriority bestHP = GetLRVHighPriority(highPriority, domains, noise);
                stack.Push(new CSPState(bestHP, domains, connectivity, boundsToFill, engine, temperature, mapIndicies, true));
            }
            else
            {
                Vector2Int firstPos = GetLRVVariable(remainingPositions, engineCollapsedTo, domains, noise);
                stack.Push(new CSPState(firstPos, engineCollapsedTo[firstPos.x, firstPos.y], domains, connectivity, boundsToFill, engine, temperature, mapIndicies, false));
            }

            while (remainingPositions.Count > 0 || highPriority.Count > 0)
            {
                CSPState curr = stack.Pop();

                if (curr.expectingReturn)
                {
                    foreach (Assignment d in curr.toRestoreDomain)
                    {
                        domains[d.pos.x, d.pos.y].Add(d.index);
                    }
                    curr.toRestoreDomain.Clear();
                }
                else
                {
                    remainingPositions.Remove(curr.pos);
                    if (curr.isHighPriority)
                    {
                        int idx = 0;
                        for (int i = 0; i < highPriority.Count; i++)
                        {
                            if (curr.pos == highPriority[i].pos)
                            {
                                idx = i;
                                break;
                            }
                        }

                        highPriority.RemoveAt(idx);
                    }
                }

                bool shouldReturn = false;
                while (curr.iterIndex < curr.domain.Length)
                {
                    int value = curr.domain[curr.iterIndex];
                    curr.iterIndex++;

                    mapIndicies[curr.pos.x + curr.pos.y * boundsToFill.size.x] = value;

                    if (AC3Modified(curr.pos, mapIndicies, domains, curr.toRestoreDomain))
                    {
                        shouldReturn = true;
                        curr.expectingReturn = true;

                        stack.Push(curr);
                        if (highPriority.Count > 0)
                        {
                            HighPriority bestHP = GetLRVHighPriority(highPriority, domains, noise);
                            stack.Push(new CSPState(bestHP, domains, connectivity, boundsToFill, engine, temperature, mapIndicies, true));
                        }
                        else
                        {
                            Vector2Int nextPos = GetLRVVariable(remainingPositions, engineCollapsedTo, domains, noise);
                            stack.Push(new CSPState(nextPos, engineCollapsedTo[nextPos.x, nextPos.y], domains, connectivity, boundsToFill, engine, temperature, mapIndicies, false));
                        }

                        break;
                    }

                    foreach (Assignment d in curr.toRestoreDomain)
                    {
                        domains[d.pos.x, d.pos.y].Add(d.index);
                    }
                    curr.toRestoreDomain.Clear();
                }
                if (shouldReturn)
                {
                    continue;
                }

                remainingPositions.Add(curr.pos);
                if (curr.isHighPriority)
                {
                    SparseSet set = new SparseSet(tiles.Length, false);
                    foreach (int i in curr.domain)
                    {
                        set.Add(i);
                    }
                    highPriority.Add(new HighPriority(curr.pos, set));
                }

                mapIndicies[curr.pos.x + curr.pos.y * boundsToFill.size.x] = -1;
            }

            foreach (CSPState state in stack)
            {
                for (int layer = 0; layer < layerCount; layer++)
                {
                    mapToFill[layer].SetTile(new Vector3Int(state.pos.x + boundsToFill.position.x, state.pos.y + boundsToFill.position.y), tiles[mapIndicies[state.pos.x + state.pos.y * boundsToFill.size.x]].tiles[layer]);
                }
            }
        }

        private Vector2Int GetLRVVariable(List<Vector2Int> remainingPositions, int[,] engineCollapsedTo, SparseSet[,] domains, float[,] noise)
        {
            Vector2Int best = Vector2Int.zero;
            float smallestDomainSize = 99999;

            foreach (Vector2Int pos in remainingPositions)
            {
                float size = domains[pos.x, pos.y].Count + noise[pos.x, pos.y];
                if (domains[pos.x, pos.y].Contains(engineCollapsedTo[pos.x, pos.y])) // if that domain contains the engine collapsed variable, reduce its domain size by 1
                {
                    size -= 1.0f;
                }

                if (size < smallestDomainSize)
                {
                    best = pos;
                    smallestDomainSize = size;
                }
            }

            return best;
        }

        private HighPriority GetLRVHighPriority(List<HighPriority> highPriority, SparseSet[,] domains, float[,] noise)
        {
            HighPriority best = new HighPriority(Vector2Int.zero, new SparseSet(tiles.Length, false));
            float smallestDomainSize = 99999;

            foreach (HighPriority asn in highPriority)
            {
                float size = domains[asn.pos.x, asn.pos.y].Count + noise[asn.pos.x, asn.pos.y];
                if (asn.pos.x == 0 || asn.pos.y == 0 || asn.pos.x == boundsToFill.size.x - 1 || asn.pos.y == boundsToFill.size.y - 1) // if that variable is on the edge, prioritize it above all others
                {
                    size -= 99999.0f;
                }

                if (size < smallestDomainSize)
                {
                    best = asn;
                    smallestDomainSize = size;
                }
            }

            return best;
        }

        public struct HighPriority
        {
            public HighPriority(Vector2Int pos, SparseSet set)
            {
                this.pos = pos;
                this.set = set;
            }

            public Vector2Int pos;
            public SparseSet set;
        }

        public struct Assignment
        {
            public Assignment(Vector2Int pos, int index)
            {
                this.pos = pos;
                this.index = index;
            }

            public Vector2Int pos;
            public int index;
        }

        public enum Direction
        {
            Bottom,
            Top,
            Left,
            Right,
            BottomLeft,
            BottomRight,
            TopLeft,
            TopRight
        }

        public struct ReviseItem
        {
            public ReviseItem(Vector2Int pos, Direction dir)
            {
                this.pos = pos;
                this.dir = dir;
            }

            public Vector2Int pos;
            public Direction dir;
        }

        // Returns true if the assignment at pos is arc consistent. False otherwise.
        bool AC3Modified(Vector2Int pos, int[] mapIndicies, SparseSet[,] domains, List<Assignment> domainValuesRemoved)
        {
            int initialIdx = mapIndicies[pos.x + pos.y * boundsToFill.size.x];
            Queue<ReviseItem> frontier = new Queue<ReviseItem>();

            EnqueueNext(pos, frontier);

            while (frontier.Count > 0)
            {
                ReviseItem next = frontier.Dequeue();

                switch (next.dir)
                {
                    case Direction.Bottom:
                        if (ReviseBottom(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.Top:
                        if (ReviseTop(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.Left:
                        if (ReviseLeft(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.Right:
                        if (ReviseRight(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.BottomLeft:
                        if (ReviseBottomLeft(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.BottomRight:
                        if (ReviseBottomRight(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.TopLeft:
                        if (ReviseTopLeft(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                    case Direction.TopRight:
                        if (ReviseTopRight(next.pos, mapIndicies, domains, domainValuesRemoved))
                        {
                            if (domains[next.pos.x, next.pos.y].Count == 0)
                            {
                                return false; // a domain became 0, therefore stop
                            }

                            EnqueueNext(next.pos, frontier);
                        }
                        break;
                }
            }

            return true;
        }

        void EnqueueNext(Vector2Int pos, Queue<ReviseItem> frontier)
        {
            if (pos.y > 0) // bottom
            {
                frontier.Enqueue(new ReviseItem(new Vector2Int(pos.x, pos.y - 1), Direction.Top));
            }
            if (pos.y < boundsToFill.size.y - 1) // top
            {
                frontier.Enqueue(new ReviseItem(new Vector2Int(pos.x, pos.y + 1), Direction.Bottom));
            }
            if (pos.x > 0) // left
            {
                frontier.Enqueue(new ReviseItem(new Vector2Int(pos.x - 1, pos.y), Direction.Right));
            }
            if (pos.x < boundsToFill.size.x - 1) // right
            {
                frontier.Enqueue(new ReviseItem(new Vector2Int(pos.x + 1, pos.y), Direction.Left));
            }

            if (connectivity.con == Connectivity.EightWay)
            {
                if (pos.y > 0 && pos.x > 0) // bottom left
                {
                    frontier.Enqueue(new ReviseItem(new Vector2Int(pos.x - 1, pos.y - 1), Direction.TopRight));
                }
                if (pos.y > 0 && pos.x < boundsToFill.size.x - 1) // bottom right
                {
                    frontier.Enqueue(new ReviseItem(new Vector2Int(pos.x + 1, pos.y - 1), Direction.TopLeft));
                }
                if (pos.y < boundsToFill.size.y - 1 && pos.x > 0) // top left
                {
                    frontier.Enqueue(new ReviseItem(new Vector2Int(pos.x - 1, pos.y + 1), Direction.BottomRight));
                }
                if (pos.y < boundsToFill.size.y - 1 && pos.x < boundsToFill.size.x - 1) // top right
                {
                    frontier.Enqueue(new ReviseItem(new Vector2Int(pos.x + 1, pos.y + 1), Direction.BottomLeft));
                }
            }
        }

        // Returns true iff we revise the domain of the given position
        bool ReviseBottom(Vector2Int pos, int[] mapIndicies, SparseSet[,] domains, List<Assignment> domainValuesRemoved)
        {
            bool revised = false;

            if (pos.y > 0)
            {
                int bottomTile = mapIndicies[pos.x + (pos.y - 1) * boundsToFill.size.x];
                if (bottomTile != -1)
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);

                        if (!connectivity.GetBottomConnectivity(idx, bottomTile))
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);
                        bool support = false;

                        for (int j = 0; j < domains[pos.x, pos.y - 1].Count; j++)
                        {
                            if (connectivity.GetBottomConnectivity(idx, domains[pos.x, pos.y - 1].GetDense(j)))
                            {
                                support = true;
                                break;
                            }
                        }

                        if (!support)
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            else if (connectivity.enforceBorderConnectivity.bottom)
            {
                for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                {
                    int idx = domains[pos.x, pos.y].GetDense(i);

                    if (!connectivity.GetBottomBorderConnectivity(idx))
                    {
                        revised = true;

                        domainValuesRemoved.Add(new Assignment(pos, idx));
                        domains[pos.x, pos.y].RemoveAt(i);
                        i--;
                    }
                }
            }

            return revised;
        }

        // Returns true iff we revise the domain of the given position
        bool ReviseTop(Vector2Int pos, int[] mapIndicies, SparseSet[,] domains, List<Assignment> domainValuesRemoved)
        {
            bool revised = false;

            if (pos.y < boundsToFill.size.y - 1)
            {
                int topTile = mapIndicies[pos.x + (pos.y + 1) * boundsToFill.size.x];
                if (topTile != -1)
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);

                        if (!connectivity.GetTopConnectivity(idx, topTile))
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);
                        bool support = false;

                        for (int j = 0; j < domains[pos.x, pos.y + 1].Count; j++)
                        {
                            if (connectivity.GetTopConnectivity(idx, domains[pos.x, pos.y + 1].GetDense(j)))
                            {
                                support = true;
                                break;
                            }
                        }

                        if (!support)
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            else if (connectivity.enforceBorderConnectivity.top)
            {
                for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                {
                    int idx = domains[pos.x, pos.y].GetDense(i);
                    if (!connectivity.GetTopBorderConnectivity(idx))
                    {
                        revised = true;

                        domainValuesRemoved.Add(new Assignment(pos, idx));
                        domains[pos.x, pos.y].RemoveAt(i);
                        i--;
                    }
                }
            }

            return revised;
        }

        // Returns true iff we revise the domain of the given position
        bool ReviseLeft(Vector2Int pos, int[] mapIndicies, SparseSet[,] domains, List<Assignment> domainValuesRemoved)
        {
            bool revised = false;

            if (pos.x > 0)
            {
                int leftTile = mapIndicies[pos.x - 1 + pos.y * boundsToFill.size.x];
                if (leftTile != -1)
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);

                        if (!connectivity.GetLeftConnectivity(idx, leftTile))
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);
                        bool support = false;

                        for (int j = 0; j < domains[pos.x - 1, pos.y].Count; j++)
                        {
                            if (connectivity.GetLeftConnectivity(idx, domains[pos.x - 1, pos.y].GetDense(j)))
                            {
                                support = true;
                                break;
                            }
                        }

                        if (!support)
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            else if (connectivity.enforceBorderConnectivity.left)
            {
                for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                {
                    int idx = domains[pos.x, pos.y].GetDense(i);
                    if (!connectivity.GetLeftBorderConnectivity(idx))
                    {
                        revised = true;

                        domainValuesRemoved.Add(new Assignment(pos, idx));
                        domains[pos.x, pos.y].RemoveAt(i);
                        i--;
                    }
                }
            }

            return revised;
        }

        // Returns true iff we revise the domain of the given position
        bool ReviseRight(Vector2Int pos, int[] mapIndicies, SparseSet[,] domains, List<Assignment> domainValuesRemoved)
        {
            bool revised = false;

            if (pos.x < boundsToFill.size.x - 1)
            {
                int rightTile = mapIndicies[pos.x + 1 + pos.y * boundsToFill.size.x];
                if (rightTile != -1)
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);

                        if (!connectivity.GetRightConnectivity(idx, rightTile))
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);
                        bool support = false;

                        for (int j = 0; j < domains[pos.x + 1, pos.y].Count; j++)
                        {
                            if (connectivity.GetRightConnectivity(idx, domains[pos.x + 1, pos.y].GetDense(j)))
                            {
                                support = true;
                                break;
                            }
                        }

                        if (!support)
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            else if (connectivity.enforceBorderConnectivity.right)
            {
                for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                {
                    int idx = domains[pos.x, pos.y].GetDense(i);
                    if (!connectivity.GetRightBorderConnectivity(idx))
                    {
                        revised = true;

                        domainValuesRemoved.Add(new Assignment(pos, idx));
                        domains[pos.x, pos.y].RemoveAt(i);
                        i--;
                    }
                }
            }

            return revised;
        }

        // Returns true iff we revise the domain of the given position
        bool ReviseBottomLeft(Vector2Int pos, int[] mapIndicies, SparseSet[,] domains, List<Assignment> domainValuesRemoved)
        {
            bool revised = false;

            if (pos.x > 0 && pos.y > 0)
            {
                int bottomLeftTile = mapIndicies[(pos.x - 1) + (pos.y - 1) * boundsToFill.size.x];
                if (bottomLeftTile != -1)
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);

                        if (!connectivity.GetBottomLeftConnectivity(idx, bottomLeftTile))
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);
                        bool support = false;

                        for (int j = 0; j < domains[pos.x - 1, pos.y - 1].Count; j++)
                        {
                            if (connectivity.GetBottomLeftConnectivity(idx, domains[pos.x - 1, pos.y - 1].GetDense(j)))
                            {
                                support = true;
                                break;
                            }
                        }

                        if (!support)
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            return revised;
        }

        // Returns true iff we revise the domain of the given position
        bool ReviseBottomRight(Vector2Int pos, int[] mapIndicies, SparseSet[,] domains, List<Assignment> domainValuesRemoved)
        {
            bool revised = false;

            if (pos.x < boundsToFill.size.x - 1 && pos.y > 0)
            {
                int bottomRightTile = mapIndicies[(pos.x + 1) + (pos.y - 1) * boundsToFill.size.x];
                if (bottomRightTile != -1)
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);

                        if (!connectivity.GetBottomRightConnectivity(idx, bottomRightTile))
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);
                        bool support = false;

                        for (int j = 0; j < domains[pos.x + 1, pos.y - 1].Count; j++)
                        {
                            if (connectivity.GetBottomRightConnectivity(idx, domains[pos.x + 1, pos.y - 1].GetDense(j)))
                            {
                                support = true;
                                break;
                            }
                        }

                        if (!support)
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            return revised;
        }

        // Returns true iff we revise the domain of the given position
        bool ReviseTopLeft(Vector2Int pos, int[] mapIndicies, SparseSet[,] domains, List<Assignment> domainValuesRemoved)
        {
            bool revised = false;

            if (pos.x > 0 && pos.y < boundsToFill.size.y - 1)
            {
                int topLeftTile = mapIndicies[(pos.x - 1) + (pos.y + 1) * boundsToFill.size.x];
                if (topLeftTile != -1)
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);

                        if (!connectivity.GetTopLeftConnectivity(idx, topLeftTile))
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);
                        bool support = false;

                        for (int j = 0; j < domains[pos.x - 1, pos.y + 1].Count; j++)
                        {
                            if (connectivity.GetTopLeftConnectivity(idx, domains[pos.x - 1, pos.y + 1].GetDense(j)))
                            {
                                support = true;
                                break;
                            }
                        }

                        if (!support)
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            return revised;
        }

        // Returns true iff we revise the domain of the given position
        bool ReviseTopRight(Vector2Int pos, int[] mapIndicies, SparseSet[,] domains, List<Assignment> domainValuesRemoved)
        {
            bool revised = false;

            if (pos.x < boundsToFill.size.x - 1 && pos.y < boundsToFill.size.y - 1)
            {
                int topRightTile = mapIndicies[(pos.x + 1) + (pos.y + 1) * boundsToFill.size.x];
                if (topRightTile != -1)
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);

                        if (!connectivity.GetTopRightConnectivity(idx, topRightTile))
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < domains[pos.x, pos.y].Count; i++)
                    {
                        int idx = domains[pos.x, pos.y].GetDense(i);
                        bool support = false;

                        for (int j = 0; j < domains[pos.x + 1, pos.y + 1].Count; j++)
                        {
                            if (connectivity.GetTopRightConnectivity(idx, domains[pos.x + 1, pos.y + 1].GetDense(j)))
                            {
                                support = true;
                                break;
                            }
                        }

                        if (!support)
                        {
                            revised = true;

                            domainValuesRemoved.Add(new Assignment(pos, idx));
                            domains[pos.x, pos.y].RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            return revised;
        }

        private int[] GetMapIndiciesAndSetDomains(SparseSet[,] domains, List<HighPriority> highPriority)
        {
            int[] mapIndicies = new int[boundsToFill.size.x * boundsToFill.size.y];

            Dictionary<TileBase, SparseSet>[] tileAtLayerToDomain = new Dictionary<TileBase, SparseSet>[layerCount];
            for (int layer = 0; layer < layerCount; layer++)
            {
                tileAtLayerToDomain[layer] = new Dictionary<TileBase, SparseSet>();

                for (int i = 0; i < tiles.Length; i++)
                {
                    if (tiles[i].tiles[layer] != null)
                    {
                        SparseSet outSet;

                        if (tileAtLayerToDomain[layer].TryGetValue(tiles[i].tiles[layer], out outSet))
                        {
                            outSet.Add(i);
                        }
                        else
                        {
                            outSet = new SparseSet(tiles.Length, false);
                            outSet.Add(i);
                            tileAtLayerToDomain[layer].Add(tiles[i].tiles[layer], outSet);
                        }
                    }
                }
            }

            Dictionary<LayeredTile, int> map = new Dictionary<LayeredTile, int>(new LayeredTileComparer());
            for (int i = 0; i < tiles.Length; i++)
            {
                map[tiles[i]] = i;
            }

            TileBase[][] mapTiles = new TileBase[layerCount][];
            for (int layer = 0; layer < layerCount; layer++)
            {
                mapTiles[layer] = mapToFill[layer].GetTilesBlock(boundsToFill);
            }

            for (int x = 0; x < boundsToFill.size.x; x++)
            {
                for (int y = 0; y < boundsToFill.size.y; y++)
                {
                    SparseSet domain = null;

                    for (int layer = 0; layer < layerCount; layer++)
                    {
                        TileBase tile = mapTiles[layer][x + y * boundsToFill.size.x];

                        SparseSet outDomain;
                        if (tile != null && tileAtLayerToDomain[layer].TryGetValue(tile, out outDomain))
                        {
                            if (domain == null)
                            {
                                domain = outDomain.Clone();
                            }
                            else
                            {
                                domain.Intersect(outDomain);
                            }
                        }
                    }

                    if (domain == null || domain.Count == 0)
                    {
                        domain = new SparseSet(tiles.Length, true);
                    }
                    else
                    {
                        highPriority.Add(new HighPriority(new Vector2Int(x, y), domain.Clone()));
                    }

                    domains[x, y] = domain;

                    if (domain.Count == 1)
                    {
                        mapIndicies[x + y * boundsToFill.size.x] = domain.GetDense(0);

                        for (int layer = 0; layer < layerCount; layer++)
                        {
                            mapToFill[layer].SetTile(new Vector3Int(x + boundsToFill.position.x, y + boundsToFill.position.y), tiles[domain.GetDense(0)].tiles[layer]);
                        }
                    }
                    else
                    {
                        mapIndicies[x + y * boundsToFill.size.x] = -1;
                    }
                }
            }

            return mapIndicies;
        }

        void OnDrawGizmosSelected()
        {
            if (mapToFill != null)
            {
                Gizmos.color = Color.blue;

                if (boundsToFill.size.z == 0)
                {
                    boundsToFill = new BoundsInt(boundsToFill.position, new Vector3Int(boundsToFill.size.x, boundsToFill.size.y, 1));
                }

                foreach (Tilemap map in mapToFill)
                {
                    TileSelectionGizmos.DrawBounds(map, boundsToFill);
                }
            }
        }
    }
}