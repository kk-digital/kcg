using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteliMap
{
    [Serializable]
    public class GeneratorWeights
    {
        public GeneratorWeights(int uniqueTileCount, int neightborhoodRadius, DirectionalBools acknowledgeBounds)
        {
            this.acknowledgeBounds = acknowledgeBounds;

            this.neightborhoodRadius = neightborhoodRadius;

            int neighborhoodSideLength = (neightborhoodRadius * 2 + 1);
            int neighborhoodArea = neighborhoodSideLength * neighborhoodSideLength;

            dim3multi = uniqueTileCount + 4;
            dim2multi = (neightborhoodRadius * 2 + 1) * dim3multi;
            dim1multi = (neightborhoodRadius * 2 + 1) * dim2multi;
            weights = new float[uniqueTileCount * dim1multi];

            // there are <neighborhoodArea> inputs, and each input is a one-hot vector of size <uniqueTileCount>
            // uniform xavier initialization
            float bound = 1.0f / Mathf.Sqrt(neighborhoodArea);
            for (int i = 0; i < uniqueTileCount; i++)
            {
                for (int j = 0; j < dim1multi; j++)
                {
                    weights[i * dim1multi + j] = UnityEngine.Random.Range(-bound, bound);
                }
            }

            // biases is initialized to all zeros
            biases = new float[uniqueTileCount];
            for (int i = 0; i < uniqueTileCount; i++)
            {
                biases[i] = 1.0f;
            }

            epochsTrained = 0;
        }

        [SerializeField] private float[] weights; // indexed by tileToPlace, nX, nY, tileAtLocation (tileAtLocation can also be an uncollapsed tile)
        [SerializeField] private float[] biases;
        [SerializeField] private int dim1multi;
        [SerializeField] private int dim2multi;
        [SerializeField] private int dim3multi;

        [SerializeField] public int epochsTrained;
        [SerializeField] public int neightborhoodRadius;

        [SerializeField] public DirectionalBools acknowledgeBounds;

        public int GetLength()
        {
            return weights.Length;
        }

        public float GetWeight(int tileToPlace, int nX, int nY, int tileAtLocation)
        {
            return weights[tileToPlace * dim1multi + nX * dim2multi + nY * dim3multi + tileAtLocation];
        }

        public void SetWeight(int tileToPlace, int nX, int nY, int tileAtLocation, float val)
        {
            weights[tileToPlace * dim1multi + nX * dim2multi + nY * dim3multi + tileAtLocation] = val;
        }

        public void AddToWeight(int tileToPlace, int nX, int nY, int tileAtLocation, float val)
        {
            weights[tileToPlace * dim1multi + nX * dim2multi + nY * dim3multi + tileAtLocation] += val;
        }

        public float GetBias(int tileToPlace)
        {
            return biases[tileToPlace];
        }

        public void AddToBias(int tileToPlace, float val)
        {
            biases[tileToPlace] += val;
        }
    }
}