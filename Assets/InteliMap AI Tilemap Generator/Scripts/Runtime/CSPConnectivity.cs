using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteliMap
{
    public enum Connectivity
    {
        FourWay,
        EightWay
    }

    [Serializable]
    public class CSPConnectivity
    {
        public CSPConnectivity(Connectivity con, int uniqueCount, DirectionalBools enforceBorderConnectivity)
        {
            this.con = con;
            this.uniqueCount = uniqueCount;
            this.enforceBorderConnectivity = enforceBorderConnectivity;

            topConnectivity = new bool[uniqueCount * uniqueCount];
            bottomConnectivity = new bool[uniqueCount * uniqueCount];
            leftConnectivity = new bool[uniqueCount * uniqueCount];
            rightConnectivity = new bool[uniqueCount * uniqueCount];

            if (con == Connectivity.EightWay)
            {
                topLeftConnectivity = new bool[uniqueCount * uniqueCount];
                topRightConnectivity = new bool[uniqueCount * uniqueCount];
                bottomLeftConnectivity = new bool[uniqueCount * uniqueCount];
                bottomRightConnectivity = new bool[uniqueCount * uniqueCount];
            }

            if (enforceBorderConnectivity.top)
            {
                topBorderConnectivity = new bool[uniqueCount];
            }
            if (enforceBorderConnectivity.bottom)
            {
                bottomBorderConnectivity = new bool[uniqueCount];
            }
            if (enforceBorderConnectivity.left)
            {
                leftBorderConnectivity = new bool[uniqueCount];
            }
            if (enforceBorderConnectivity.right)
            {
                rightBorderConnectivity = new bool[uniqueCount];
            }
        }

        [SerializeField] public Connectivity con;

        [SerializeField] private bool[] topConnectivity;
        [SerializeField] private bool[] bottomConnectivity;
        [SerializeField] private bool[] leftConnectivity;
        [SerializeField] private bool[] rightConnectivity;
        [SerializeField] private int uniqueCount;

        [SerializeField] private bool[] topLeftConnectivity;
        [SerializeField] private bool[] topRightConnectivity;
        [SerializeField] private bool[] bottomLeftConnectivity;
        [SerializeField] private bool[] bottomRightConnectivity;

        [SerializeField] public DirectionalBools enforceBorderConnectivity;
        [SerializeField] private bool[] topBorderConnectivity;
        [SerializeField] private bool[] bottomBorderConnectivity;
        [SerializeField] private bool[] leftBorderConnectivity;
        [SerializeField] private bool[] rightBorderConnectivity;

        public bool GetTopConnectivity(int mainIdx, int topIdx)
        {
            return topConnectivity[mainIdx * uniqueCount + topIdx];
        }

        public bool GetBottomConnectivity(int mainIdx, int bottomIdx)
        {
            return bottomConnectivity[mainIdx * uniqueCount + bottomIdx];
        }

        public bool GetLeftConnectivity(int mainIdx, int leftIdx)
        {
            return leftConnectivity[mainIdx * uniqueCount + leftIdx];
        }

        public bool GetRightConnectivity(int mainIdx, int rightIdx)
        {
            return rightConnectivity[mainIdx * uniqueCount + rightIdx];
        }

        public bool GetTopLeftConnectivity(int mainIdx, int topLeftIdx)
        {
            return topLeftConnectivity[mainIdx * uniqueCount + topLeftIdx];
        }

        public bool GetTopRightConnectivity(int mainIdx, int topRightIdx)
        {
            return topRightConnectivity[mainIdx * uniqueCount + topRightIdx];
        }

        public bool GetBottomLeftConnectivity(int mainIdx, int bottomLeftIdx)
        {
            return bottomLeftConnectivity[mainIdx * uniqueCount + bottomLeftIdx];
        }

        public bool GetBottomRightConnectivity(int mainIdx, int bottomRightIdx)
        {
            return bottomRightConnectivity[mainIdx * uniqueCount + bottomRightIdx];
        }

        public bool GetTopBorderConnectivity(int idx)
        {
            return topBorderConnectivity[idx];
        }

        public bool GetBottomBorderConnectivity(int idx)
        {
            return bottomBorderConnectivity[idx];
        }

        public bool GetLeftBorderConnectivity(int idx)
        {
            return leftBorderConnectivity[idx];
        }

        public bool GetRightBorderConnectivity(int idx)
        {
            return rightBorderConnectivity[idx];
        }

        // Set
        public void SetTopConnectivity(int mainIdx, int topIdx, bool val)
        {
            topConnectivity[mainIdx * uniqueCount + topIdx] = val;
        }

        public void SetBottomConnectivity(int mainIdx, int bottomIdx, bool val)
        {
            bottomConnectivity[mainIdx * uniqueCount + bottomIdx] = val;
        }

        public void SetLeftConnectivity(int mainIdx, int leftIdx, bool val)
        {
            leftConnectivity[mainIdx * uniqueCount + leftIdx] = val;
        }

        public void SetRightConnectivity(int mainIdx, int rightIdx, bool val)
        {
            rightConnectivity[mainIdx * uniqueCount + rightIdx] = val;
        }

        public void SetTopLeftConnectivity(int mainIdx, int topLeftIdx, bool val)
        {
            topLeftConnectivity[mainIdx * uniqueCount + topLeftIdx] = val;
        }

        public void SetTopRightConnectivity(int mainIdx, int topRightIdx, bool val)
        {
            topRightConnectivity[mainIdx * uniqueCount + topRightIdx] = val;
        }

        public void SetBottomLeftConnectivity(int mainIdx, int bottomLeftIdx, bool val)
        {
            bottomLeftConnectivity[mainIdx * uniqueCount + bottomLeftIdx] = val;
        }

        public void SetBottomRightConnectivity(int mainIdx, int bottomRightIdx, bool val)
        {
            bottomRightConnectivity[mainIdx * uniqueCount + bottomRightIdx] = val;
        }

        public void SetTopBorderConnectivity(int idx, bool val)
        {
            topBorderConnectivity[idx] = val;
        }

        public void SetBottomBorderConnectivity(int idx, bool val)
        {
            bottomBorderConnectivity[idx] = val;
        }

        public void SetLeftBorderConnectivity(int idx, bool val)
        {
            leftBorderConnectivity[idx] = val;
        }

        public void SetRightBorderConnectivity(int idx, bool val)
        {
            rightBorderConnectivity[idx] = val;
        }
    }
}