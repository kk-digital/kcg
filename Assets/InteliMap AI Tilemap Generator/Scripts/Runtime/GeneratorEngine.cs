using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InteliMap
{
    public class GeneratorEngine
    {
        public GeneratorEngine(GeneratorWeights weights, int neighborhoodRadius, System.Random threadRand, BoundsInt bounds, int uniqueCount)
        {
            this.weights = weights;
            this.neighborhoodRadius = neighborhoodRadius;
            this.bounds = bounds;
            this.uniqueCount = uniqueCount;

            frontier = new PriorityQueue<float>(bounds);

            // Indexed by x, y, tile
            unactivated = new float[bounds.size.x, bounds.size.y, uniqueCount];
            activated = new float[bounds.size.x, bounds.size.y, uniqueCount];
        }

        private GeneratorWeights weights;
        private int neighborhoodRadius;
        private BoundsInt bounds;
        private int uniqueCount;

        private PriorityQueue<float> frontier;
        private float[,,] unactivated;
        private float[,,] activated;

        public void Reset(System.Random threadRand, float randomness, int[] mapIndicies)
        {
            frontier.Clear();

            // First initiailize all the unactivated
            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    for (int i = 0; i < uniqueCount; i++)
                    {
                        unactivated[x, y, i] = (float)threadRand.NextDouble() * randomness * 2.0f - randomness + weights.GetBias(i);
                    }
                }
            }
            // Then iterate through all for the unactivated addition
            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    for (int i = 0; i < uniqueCount; i++)
                    {
                        for (int nX = -neighborhoodRadius; nX <= neighborhoodRadius; nX++)
                        {
                            for (int nY = -neighborhoodRadius; nY <= neighborhoodRadius; nY++)
                            {
                                int oX = x + nX;
                                int oY = y + nY;

                                if (oX >= 0 && oY >= 0 && oX < bounds.size.x && oY < bounds.size.y)
                                {
                                    int idx = mapIndicies[x + y * bounds.size.x];
                                    if (idx != -1)
                                    {
                                        unactivated[oX, oY, i] += weights.GetWeight(i, neighborhoodRadius - nX, neighborhoodRadius - nY, idx); // in bounds
                                    }
                                }
                                else
                                {
                                    if (oX < 0 && weights.acknowledgeBounds.left)
                                    {
                                        unactivated[x, y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount); // out of bounds
                                    }
                                    else if (oY < 0 && weights.acknowledgeBounds.bottom)
                                    {
                                        unactivated[x, y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 1); // out of bounds
                                    }
                                    else if (oX >= bounds.size.x && weights.acknowledgeBounds.right)
                                    {
                                        unactivated[x, y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 2); // out of bounds
                                    }
                                    else if (oY >= bounds.size.y && weights.acknowledgeBounds.top)
                                    {
                                        unactivated[x, y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 3); // out of bounds
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Then add everything to the frontier
            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    if (mapIndicies[x + y * bounds.size.x] == -1)
                    {
                        frontier.Enqueue(new Vector2Int(x, y), Activate(x, y));
                    }
                }
            }
        }

        public bool IsDone()
        {
            return frontier.IsEmpty();
        }

        // Returns the next position to be collapsed
        public Vector2Int NextPos()
        {
            return frontier.Dequeue();
        }

        // Calculates and returns the prediction for the given position. No updating is done.
        public int CalculateAndPredict(Vector2Int pos, float rerolls, int[] mapIndicies)
        {
            for (int i = 0; i < uniqueCount; i++)
            {
                unactivated[pos.x, pos.y, i] = weights.GetBias(i);
                for (int nX = -neighborhoodRadius; nX <= neighborhoodRadius; nX++)
                {
                    for (int nY = -neighborhoodRadius; nY <= neighborhoodRadius; nY++)
                    {
                        int oX = pos.x + nX;
                        int oY = pos.y + nY;

                        if (oX >= 0 && oY >= 0 && oX < bounds.size.x && oY < bounds.size.y)
                        {
                            int idx = mapIndicies[oX + oY * bounds.size.x];

                            if (idx != -1)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, idx);
                            }
                        }
                        else
                        {
                            if (oX < 0 && weights.acknowledgeBounds.left)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount); // out of bounds
                            }
                            else if (oY < 0 && weights.acknowledgeBounds.bottom)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 1); // out of bounds
                            }
                            else if (oX >= bounds.size.x && weights.acknowledgeBounds.right)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 2); // out of bounds
                            }
                            else if (oY >= bounds.size.y && weights.acknowledgeBounds.top)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 3); // out of bounds
                            }
                        }
                    }
                }
            }
            Activate(pos.x, pos.y);

            int collapsedTo = Predict(pos, rerolls);

            return collapsedTo;
        }

        // Calculates and returns the prediction for the given position. No updating is done.
        public int CalculateAndPredictAndReset(Vector2Int pos, float rerolls, int[] mapIndicies)
        {
            for (int i = 0; i < uniqueCount; i++)
            {
                unactivated[pos.x, pos.y, i] = weights.GetBias(i);
                for (int nX = -neighborhoodRadius; nX <= neighborhoodRadius; nX++)
                {
                    for (int nY = -neighborhoodRadius; nY <= neighborhoodRadius; nY++)
                    {
                        int oX = pos.x + nX;
                        int oY = pos.y + nY;

                        if (oX >= 0 && oY >= 0 && oX < bounds.size.x && oY < bounds.size.y)
                        {
                            int idx = mapIndicies[oX + oY * bounds.size.x];

                            if (idx != -1)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, idx);
                            }
                        }
                        else
                        {
                            if (oX < 0 && weights.acknowledgeBounds.left)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount); // out of bounds
                            }
                            else if (oY < 0 && weights.acknowledgeBounds.bottom)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 1); // out of bounds
                            }
                            else if (oX >= bounds.size.x && weights.acknowledgeBounds.right)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 2); // out of bounds
                            }
                            else if (oY >= bounds.size.y && weights.acknowledgeBounds.top)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 3); // out of bounds
                            }
                        }
                    }
                }
            }
            Activate(pos.x, pos.y);

            int collapsedTo = Predict(pos, rerolls);

            // Reset
            for (int i = 0; i < uniqueCount; i++)
            {
                unactivated[pos.x, pos.y, i] = weights.GetBias(i);
            }

            return collapsedTo;
        }

        // Returns the tile index that was collapsed to, and updates the predictions accordingly
        public int PredictAndCollapse(Vector2Int pos, float rerolls)
        {
            int collapsedTo = Predict(pos, rerolls);

            UpdateAfterCollapse(pos, collapsedTo);

            return collapsedTo;
        }

        private int Predict(Vector2Int pos, float rerolls)
        {
            int collapsedTo = uniqueCount - 1;
            if (rerolls >= 0.0f)
            {
                float collapsedConfidence = 0.0f;

                for (int attempt = 0; attempt <= rerolls || (attempt == (int)rerolls + 1 && Random.Range(0.0f, 1.0f) < (rerolls - attempt + 1.0f)); attempt++)
                {
                    float val = Random.Range(0.0f, 1.0f);
                    for (int i = 0; i < uniqueCount; i++)
                    {
                        val -= activated[pos.x, pos.y, i];
                        if (val < 0.0f)
                        {
                            if (activated[pos.x, pos.y, i] >= collapsedConfidence)
                            {
                                collapsedConfidence = activated[pos.x, pos.y, i];
                                collapsedTo = i;
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                float collapsedConfidence = 1.0f;
                for (int attempt = 0; attempt >= rerolls || (attempt == (int)rerolls - 1 && Random.Range(0.0f, 1.0f) < (rerolls - attempt)); attempt--)
                {
                    float val = Random.Range(0.0f, 1.0f);
                    for (int i = 0; i < uniqueCount; i++)
                    {
                        val -= activated[pos.x, pos.y, i];
                        if (val < 0.0f)
                        {
                            if (activated[pos.x, pos.y, i] <= collapsedConfidence)
                            {
                                collapsedConfidence = activated[pos.x, pos.y, i];
                                collapsedTo = i;
                            }
                            break;
                        }
                    }
                }
            }

            return collapsedTo;
        }

        // Updates the biases/weights to more accurately predict the expectedIndex. Returns the total loss.
        public float Train(Vector2Int pos, int[] mapIndicies, int expectedIndex, float learningRate)
        {
            // Recalculate the unactivated/activated at this index as it may have changed due to changing weights/biases
            for (int i = 0; i < uniqueCount; i++)
            {
                unactivated[pos.x, pos.y, i] = weights.GetBias(i);
                for (int nX = -neighborhoodRadius; nX <= neighborhoodRadius; nX++)
                {
                    for (int nY = -neighborhoodRadius; nY <= neighborhoodRadius; nY++)
                    {
                        int oX = pos.x + nX;
                        int oY = pos.y + nY;

                        if (oX >= 0 && oY >= 0 && oX < bounds.size.x && oY < bounds.size.y)
                        {
                            int idx = mapIndicies[oX + oY * bounds.size.x];

                            if (idx != -1)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, idx);
                            }
                        }
                        else
                        {
                            if (oX < 0 && weights.acknowledgeBounds.left)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount); // out of bounds
                            }
                            else if (oY < 0 && weights.acknowledgeBounds.bottom)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 1); // out of bounds
                            }
                            else if (oX >= bounds.size.x && weights.acknowledgeBounds.right)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 2); // out of bounds
                            }
                            else if (oY >= bounds.size.y && weights.acknowledgeBounds.top)
                            {
                                unactivated[pos.x, pos.y, i] += weights.GetWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 3); // out of bounds
                            }
                        }
                    }
                }
            }
            Activate(pos.x, pos.y);

            // Update the biases/weights
            float totalLoss = 0.0f;
            for (int i = 0; i < uniqueCount; i++)
            {
                float a = (i == expectedIndex) ? 1.0f : 0.0f;
                float error = a - activated[pos.x, pos.y, i];
                totalLoss += -a * Mathf.Log(activated[pos.x, pos.y, i]);

                weights.AddToBias(i, error * learningRate);

                for (int nX = -neighborhoodRadius; nX <= neighborhoodRadius; nX++)
                {
                    for (int nY = -neighborhoodRadius; nY <= neighborhoodRadius; nY++)
                    {
                        int oX = pos.x + nX;
                        int oY = pos.y + nY;

                        if (oX >= 0 && oY >= 0 && oX < bounds.size.x && oY < bounds.size.y)
                        {
                            int idx = mapIndicies[oX + oY * bounds.size.x];

                            if (idx != -1)
                            {
                                weights.AddToWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, idx, error * learningRate); // train the collapsed weight
                            }
                        }
                        else
                        {
                            if (oX < 0 && weights.acknowledgeBounds.left)
                            {
                                weights.AddToWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount, error * learningRate);
                            }
                            else if (oY < 0 && weights.acknowledgeBounds.bottom)
                            {
                                weights.AddToWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 1, error * learningRate);
                            }
                            else if (oX >= bounds.size.x && weights.acknowledgeBounds.right)
                            {
                                weights.AddToWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 2, error * learningRate);
                            }
                            else if (oY >= bounds.size.y && weights.acknowledgeBounds.top)
                            {
                                weights.AddToWeight(i, nX + neighborhoodRadius, nY + neighborhoodRadius, uniqueCount + 3, error * learningRate);
                            }
                        }
                    }
                }
            }

            UpdateAfterCollapse(pos, expectedIndex);

            return totalLoss;
        }

        // Recomputes the activation at the given coordinates, also returns the highest confidence value for that position
        private float Activate(int x, int y)
        {
            // Essentially performs a softmax

            // the natural exponent
            float total = 0.0f;
            for (int i = 0; i < uniqueCount; i++)
            {
                activated[x, y, i] = Mathf.Exp(unactivated[x, y, i]);
                total += activated[x, y, i];
            }

            // the argmax
            float highest = 0.0f;
            for (int i = 0; i < uniqueCount; i++)
            {
                activated[x, y, i] /= total;
                if (activated[x, y, i] > highest)
                {
                    highest = activated[x, y, i];
                }
            }

            return highest;
        }

        private void UpdateAfterCollapse(Vector2Int pos, int collapsedTo)
        {
            for (int nX = -neighborhoodRadius; nX <= neighborhoodRadius; nX++)
            {
                for (int nY = -neighborhoodRadius; nY <= neighborhoodRadius; nY++)
                {
                    int oX = pos.x + nX;
                    int oY = pos.y + nY;

                    if (oX >= 0 && oY >= 0 && oX < bounds.size.x && oY < bounds.size.y)
                    {
                        for (int i = 0; i < uniqueCount; i++)
                        {
                            unactivated[oX, oY, i] += weights.GetWeight(i, neighborhoodRadius - nX, neighborhoodRadius - nY, collapsedTo); // add the new collapsed weight
                        }
                        frontier.Update(new Vector2Int(oX, oY), Activate(oX, oY));
                    }
                }
            }
        }
    }
}