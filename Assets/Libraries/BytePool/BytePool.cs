using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


public struct ByteData
{
    public byte[] Base;

    public Int64 Start;
    public Int64 Length;
}

public class BytePool
{
    struct ByteObject
    {
        public Int64 Offset;
        public Int64 Size;

        public bool IsFree;

        public ByteObject(Int64 offset, Int64 size, bool isFree)
        {
            Offset = offset;
            Size = size;
            IsFree = isFree;
        }
    }



    byte[] DataArray;
    Int64 DataArrayCapacity;

    // list of allocated bytes
    // can be accessed by index
    List <ByteObject> IndexArray = new List<ByteObject>();

    // double linked list of free blocks
    // for fast insertion deletion
    // we wont be deallocating much
    LinkedList <int> FreeIndices = new LinkedList<int>();

    Int64 TotalAllocations;
    Int64 TotalDeallocations;

    Int64 TotalUsedSpace;

    Int64 NumberOfExpandCalls;
    
    
    BytePool(Int64 BaseCapacity = 1024 * 1024)
    {

        Int64 BaseSize = BaseCapacity;
        DataArray = new byte[BaseSize];
        DataArrayCapacity = BaseSize;


        IndexArray.Add(new ByteObject(0, BaseSize, true));
        int blockID = IndexArray.Count - 1;

        // we start with one giant free block
        // by adding its index to the free list
        FreeIndices.AddFirst(blockID);
    }

    Int64 ExpandCapacityFunction(Int64 PreviousCapacity)
    {
        // if we have a small pool like 64mb we double 
        // the previous capacity
        if (PreviousCapacity < 1024 * 1024 * 64)
        {
            return PreviousCapacity * 2;
        }
        else
        {
            return (Int64)(PreviousCapacity * 1.5);
        }

    }


    void Expand(Int64 newCapacity)
    {
        if (newCapacity > DataArrayCapacity)
        {
            Int64 oldCapacity = DataArrayCapacity;
            Int64 difference = newCapacity - oldCapacity;
            NumberOfExpandCalls++;

            DataArrayCapacity = newCapacity;

            // create a new byte[] and copy the old one
            // would love to use a memcpy here
            byte[] newDataArray = new byte[DataArrayCapacity];
            for(Int64 index = 0; index < oldCapacity; index++)
            {
                newDataArray[index] = DataArray[index];
            }
            DataArray = newDataArray;

            // the new block will be added in the free list
            IndexArray.Add(new ByteObject(oldCapacity, difference, true));
            int blockID = IndexArray.Count - 1;
            FreeIndices.AddLast(blockID);
        }
    }
    
    // used to get the memory from an index
    // the index is provided in the Allocate method
    ByteData Get(int index)
    {
        ByteData byteData = new ByteData();
        byteData.Base = null;
        byteData.Start = 0;
        byteData.Length = 0;

        if (index >= 0 && index < IndexArray.Count)
        {
            ByteObject byteObject = IndexArray[index];

            byteData.Base = DataArray;
            byteData.Start = byteObject.Offset;
            byteData.Length = byteObject.Size;
        }

        return byteData;
    }


    // Allocate memory 
    // returns an identifier to the memory block
    // the memory can be accesed using the Get function
    // so that we can keep track of its usage every frame
    int Allocate(Int64 size)
    {
        TotalAllocations++;
        TotalUsedSpace += size;

        LinkedListNode <int> found = null;

        // we allocate memory by going through the list of 
        // empty chunks and find one that fits our size.
        // if we cant find one we just expand the size.
        for(LinkedListNode <int> iterator = FreeIndices.First; 
                iterator != null; iterator = iterator.Next)
        {
            // this is a free chunk of memory
            ByteObject byteObject = IndexArray[iterator.Value];

            if (byteObject.Size >= size)
            {
                found = iterator;
                break;
            }
        }

        // if we could not find a free chunk with
        // the size we want, we need to expand
        if (found == null)
        {
            Int64 oldCapacity = DataArrayCapacity;
            Int64 newCapacity = ExpandCapacityFunction(oldCapacity);

            Int64 difference = newCapacity - oldCapacity;

            // here we make sure to allocate more than the size we need
            while(difference < size)
            {
                newCapacity = ExpandCapacityFunction(newCapacity);
                difference = newCapacity - oldCapacity;
            }

            Expand(newCapacity);    
            for(LinkedListNode <int> iterator = FreeIndices.Last; 
                iterator != null; iterator = iterator.Previous)
            {
                // this is a free chunk of memory
                ByteObject byteObject = IndexArray[iterator.Value];

                if (byteObject.Size >= size)
                {
                    found = iterator;
                    break;
                }
            }
        }


        int byteObjectID = found.Value;
        int newIndex = 0;
        ByteObject byteObjet = IndexArray[byteObjectID];
        if (byteObjet.Size == size)
        {
            FreeIndices.Remove(found);
            byteObjet.IsFree = false;

            newIndex = byteObjectID;
        }
        else
        {
            IndexArray.Add(new ByteObject(byteObjet.Offset, size, false));
            byteObjet.Offset += size;
            byteObjet.Size -= size;

            newIndex = IndexArray.Count - 1;
        }

        IndexArray[byteObjectID] = byteObjet;


        return newIndex;
	}
    

    // release the memory so that it can be used
    // by other allocation calls
    void Deallocate(int index)
    {
        if (index >= 0 && index < IndexArray.Count)
        {
            ByteObject byteObject = IndexArray[index];

            TotalDeallocations++;
                    
            Int64 size = byteObject.Size;
            TotalUsedSpace -= size;

            byteObject.IsFree = true;

            IndexArray[index] = byteObject;
            FreeIndices.AddFirst(index);
        }
    }
    
}