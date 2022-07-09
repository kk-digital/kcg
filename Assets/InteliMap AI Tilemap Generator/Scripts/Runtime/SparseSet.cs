using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteliMap
{
    public class SparseSet
    {
        public SparseSet(int capacity, bool full)
        {
            this.capacity = capacity;

            denseArr = new int[capacity];
            sparseArr = new int[capacity];

            if (full)
            {
                count = capacity;
                for (int i = 0; i < capacity; i++)
                {
                    denseArr[i] = i;
                    sparseArr[i] = i;
                }
            }
            else
            {
                count = 0;
            }
        }

        private int capacity;
        private int count;
        private int[] denseArr; // full of values
        private int[] sparseArr; // full of indexes to denseArr

        public int Count
        {
            get { return count; }
        }

        public SparseSet Clone()
        {
            SparseSet returnable = new SparseSet(this.capacity, false);
            for (int i = 0; i < count; i++)
            {
                returnable.Add(denseArr[i]);
            }
            return returnable;
        }

        public void Intersect(SparseSet other)
        {
            for (int i = 0; i < count; i++)
            {
                if (!other.Contains(denseArr[i]))
                {
                    RemoveAt(i);
                    i--;
                }
            }
        }

        public void Add(int value)
        {
            denseArr[count] = value;
            sparseArr[value] = count;
            count++;
        }

        public void Remove(int value)
        {
            denseArr[sparseArr[value]] = denseArr[count - 1];
            sparseArr[denseArr[count - 1]] = sparseArr[value];
            count--;
        }

        public bool Contains(int value)
        {
            return sparseArr[value] < count && denseArr[sparseArr[value]] == value;
        }

        public int GetDense(int idx)
        {
            return denseArr[idx];
        }

        public void RemoveAt(int idx)
        {
            denseArr[idx] = denseArr[count - 1];
            sparseArr[denseArr[count - 1]] = idx;
            count--;
        }

        public IEnumerator<int> GetEnumerator()
        {
            int i = 0;
            while (i < count)
            {
                yield return denseArr[i];
                i++;
            }
        }

        public int[] ToArray()
        {
            int[] arr = new int[count];
            for (int i = 0; i < count; i++)
            {
                arr[i] = denseArr[i];
            }
            return arr;
        }
    }
}
