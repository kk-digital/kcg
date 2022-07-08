using System;
using UnityEngine;

namespace InteliMap
{
    // A simple priority queue using an arrayed binary max heap.
    // This priority queue has a few unique attributes however, when enqueuing if two elements have the same priority.
    public class PriorityQueue<TPriority> where TPriority : IComparable<TPriority>
    {
        struct Item
        {
            public Item(Vector2Int key, TPriority priority)
            {
                this.key = key;
                this.priority = priority;
            }

            public Vector2Int key;
            public TPriority priority;
        }

        private Item[] arr;
        private int[,] indicies; // -1 indiciates it does not exist
        private int count;
        private int capacity;

        public PriorityQueue(BoundsInt bounds)
        {
            capacity = bounds.size.x * bounds.size.y + 1; // +1 needed for 0th empty slot
            arr = new Item[capacity];
            count = 0;

            indicies = new int[bounds.size.x, bounds.size.y];
            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    indicies[x, y] = -1;
                }
            }
        }

        public bool IsEmpty()
        {
            return count == 0;
        }

        public void Clear()
        {
            count = 0;
        }

        public int Count()
        {
            return count;
        }

        public TPriority TopPriority()
        {
            return arr[1].priority;
        }

        public void Enqueue(Vector2Int key, TPriority priority)
        {
            count++;

            // Double the capacity if it is exceeded
            if (count == capacity)
            {
                capacity *= 2;
                Array.Resize(ref arr, capacity);
            }
            arr[count] = new Item(key, priority);
            indicies[key.x, key.y] = count;

            RestoreHeapUpwards(count);
        }

        public Vector2Int Dequeue()
        {
            Vector2Int toReturn = arr[1].key;
            indicies[toReturn.x, toReturn.y] = -1;

            arr[1] = arr[count];
            indicies[arr[1].key.x, arr[1].key.y] = 1;
            count--;

            RestoreHeapDownwards(1);

            return toReturn;
        }

        // Returns true if a matching key was found and updated with a new priority, false if no matching key was found
        public bool Update(Vector2Int key, TPriority newPriority)
        {
            int idx = indicies[key.x, key.y];
            if (idx == -1)
            {
                return false;
            }

            int cmp = newPriority.CompareTo(arr[idx].priority);
            arr[idx].priority = newPriority;

            if (cmp < 0) // less than previous priority
            {
                RestoreHeapDownwards(idx);
            }
            else if (cmp > 0) // greater than previous priority
            {
                RestoreHeapUpwards(idx);
            }
            return true;
        }

        public void Remove(Vector2Int key)
        {
            int idx = indicies[key.x, key.y];
            if (idx == -1)
            {
                return;
            }

            if (idx == count)
            {
                count--;
                return;
            }

            indicies[key.x, key.y] = -1;

            int cmp = arr[count].priority.CompareTo(arr[idx].priority);

            arr[idx] = arr[count];
            indicies[arr[idx].key.x, arr[idx].key.y] = idx;
            count--;

            if (cmp < 0) // less than previous priority
            {
                RestoreHeapDownwards(idx);
            }
            else if (cmp > 0) // greater than previous priority
            {
                RestoreHeapUpwards(idx);
            }
        }

        private void RestoreHeapUpwards(int idx)
        {
            if (idx == 1)
                return;

            int parentIdx = idx / 2;
            if (arr[parentIdx].priority.CompareTo(arr[idx].priority) < 0)
            {
                (arr[parentIdx], arr[idx]) = (arr[idx], arr[parentIdx]);

                indicies[arr[idx].key.x, arr[idx].key.y] = idx;
                indicies[arr[parentIdx].key.x, arr[parentIdx].key.y] = parentIdx;

                RestoreHeapUpwards(parentIdx);
            }
        }

        private void RestoreHeapDownwards(int idx)
        {
            int leftIdx = idx * 2;
            int rightIdx = leftIdx + 1;

            if (leftIdx >= count) // no children, stop
            {
                return;
            }
            else if (rightIdx >= count) // only left child
            {
                if (arr[leftIdx].priority.CompareTo(arr[idx].priority) > 0)
                {
                    (arr[leftIdx], arr[idx]) = (arr[idx], arr[leftIdx]);

                    indicies[arr[idx].key.x, arr[idx].key.y] = idx;
                    indicies[arr[leftIdx].key.x, arr[leftIdx].key.y] = leftIdx;

                    RestoreHeapDownwards(leftIdx);
                }
            }
            else // both children
            {
                if (arr[leftIdx].priority.CompareTo(arr[rightIdx].priority) > 0) // left is greater
                {
                    if (arr[leftIdx].priority.CompareTo(arr[idx].priority) > 0)
                    {
                        (arr[leftIdx], arr[idx]) = (arr[idx], arr[leftIdx]);

                        indicies[arr[idx].key.x, arr[idx].key.y] = idx;
                        indicies[arr[leftIdx].key.x, arr[leftIdx].key.y] = leftIdx;

                        RestoreHeapDownwards(leftIdx);
                    }
                }
                else // right is greater
                {
                    if (arr[rightIdx].priority.CompareTo(arr[idx].priority) > 0)
                    {
                        (arr[rightIdx], arr[idx]) = (arr[idx], arr[rightIdx]);

                        indicies[arr[idx].key.x, arr[idx].key.y] = idx;
                        indicies[arr[rightIdx].key.x, arr[rightIdx].key.y] = rightIdx;

                        RestoreHeapDownwards(rightIdx);
                    }
                }
            }
        }

        public override string ToString()
        {
            string print = "Count: {" + count + "}  ";
            for (int i = 1; i <= count; i++)
            {
                print += "[" + arr[i].key + " | " + arr[i].priority + "], ";
            }

            return print;
        }
    }
}