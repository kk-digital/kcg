using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;



unsafe struct UnmanagedArray
{

	public Int64 Capacity;
	public Int64 ElementSize;
    public Int64 Length;
 
	public void* Memory;
	public UnmanagedArray(Int64 capacity, Int64 elementSize)
	{
		Memory = (void*)Marshal.AllocHGlobal((IntPtr)(capacity * elementSize));
		Capacity = capacity;
		ElementSize = elementSize;
        Length = 0;
	}

    public void Expand(Int64 newCapacity)
    {
        if (newCapacity > Capacity)
        {
            Capacity = newCapacity;
            Memory = (void*)Marshal.ReAllocHGlobal((IntPtr)Memory, (IntPtr)(Capacity * ElementSize));
        }
    }

    public Int64 RemainingCapacity()
    {
        return Capacity - Length;
    }

	public void* this[Int64 index]
	{
		get
		{
			return ((byte*)Memory) + ElementSize * index;
		}
	}
	public void Destroy()
	{
		Marshal.FreeHGlobal((IntPtr)Memory);
		Memory = null;
		Capacity = 0;
        Length = 0;
	}
}


public class BytePool
{
    struct IndexObject
    {
        public Int64 ID;
    }

    struct ByteObject
    {
        public Int64 Offset;
        public Int64 Size;

        public bool IsFree;

    }



    IntPtr DataArray;
    Int64 DataArrayCapacity;

    UnmanagedArray IndexArray;
    UnmanagedArray FreeIndices;

    Int64 TotalAllocations;
    Int64 TotalDeallocations;

    Int64 TotalUsedSpace;

    Int64 NumberOfExpandCalls;
    
    
    unsafe BytePool(Int64 BaseCapacity = 1024 * 1024, Int64 IndexArrayInitialCapacity = 1024)
    {
        IndexArray = new UnmanagedArray(IndexArrayInitialCapacity, sizeof(ByteObject));
        FreeIndices = new UnmanagedArray(IndexArrayInitialCapacity, sizeof(IndexObject));

        Int64 BaseSize = BaseCapacity;
        DataArray = (IntPtr)Marshal.AllocHGlobal((IntPtr)BaseSize);
        DataArrayCapacity = BaseSize;

        Int64 blockID = AddIndex(0, BaseSize, true);
        AddFreeIndex(blockID);

    }

    ~BytePool()
    {
       IndexArray.Destroy();
       FreeIndices.Destroy(); 

       Marshal.FreeHGlobal(DataArray);
       DataArrayCapacity = 0;
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

    unsafe Int64 AddIndex(Int64 offset, Int64 size, bool IsFree)
    {
        // check to see if we have space for 1 more index
        // if not we expand the memory
        if (IndexArray.RemainingCapacity() <= 0)
        {
            Int64 newCapacity = ExpandCapacityFunction(IndexArray.Capacity);
            IndexArray.Expand(newCapacity);
        }


        Int64 index = IndexArray.Length;
        *((ByteObject *)IndexArray[IndexArray.Length]) = 
                    new ByteObject{ Offset = offset, Size = size, IsFree = IsFree};
        IndexArray.Length++;

        return index;
    }

    unsafe void RemoveIndex(Int64 ID)
    {
        for(Int64 index = ID; index < IndexArray.Length - 1; index++)
        {
            ByteObject* obj1 = (ByteObject *)IndexArray[index];
            ByteObject* obj2 = (ByteObject *)IndexArray[index + 1];

            *obj1 = *obj2;
        }
    }

    unsafe void AddFreeIndex(Int64 blockId)
    {
        // check to see if we have space for 1 more index
        // if not we expand the memory
        if (FreeIndices.RemainingCapacity() <= 0)
        {
            Int64 newCapacity = ExpandCapacityFunction(FreeIndices.Capacity);
            FreeIndices.Expand(newCapacity);
        }

        *((IndexObject *)FreeIndices[FreeIndices.Length]) = new IndexObject{ ID = blockId};
        FreeIndices.Length++;
    }

    unsafe void RemoveFreeIndex(Int64 ID)
    {
        for(Int64 index = ID; index < FreeIndices.Length - 1; index++)
        {
            IndexObject* obj1 = (IndexObject *)FreeIndices[index];
            IndexObject* obj2 = (IndexObject *)FreeIndices[index + 1];

            *obj1 = *obj2;
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
            DataArray = Marshal.ReAllocHGlobal((IntPtr)DataArray, (IntPtr)(DataArrayCapacity));

             Int64 blockID = AddIndex(oldCapacity, difference, true);
             AddFreeIndex(blockID);
        }
    }
    
    // used to get the memory from an index
    // the index is provided in the Allocate method
    unsafe IntPtr Get(Int64 index)
    {
        IntPtr ptr = (IntPtr)0;

        if (index >= 0 && index < IndexArray.Length)
        {
            ByteObject *byteObject = (ByteObject *)IndexArray[index];
            ptr = (IntPtr)((char *)DataArray + ((*byteObject).Offset));
        }

        return ptr;
    }


    // Allocate memory 
    // returns an identifier to the memory block
    // the memory can be accesed using the Get function
    // so that we can keep track of its usage every frame
    unsafe Int64 Allocate(int size)
    {
        TotalAllocations++;
        TotalUsedSpace += size;

        Int64 foundIndex = -1;
        IndexObject *found = null;

        // we allocate memory by going through the list of 
        // empty chunks and find one that fits our size.
        // if we cant find one we just expand the size.
        for(Int64 freeIndex = 0; freeIndex < FreeIndices.Length; freeIndex++)
        {
            IndexObject *index = (IndexObject *)FreeIndices[freeIndex];

            // this is a free chunk of memory
            ByteObject *byteObject = (ByteObject *)IndexArray[(*index).ID];

            if ((*byteObject).Size >= size)
            {
                foundIndex = freeIndex;
                found = index;
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
        }


        for(Int64 freeIndex = 0; freeIndex < FreeIndices.Length; freeIndex++)
        {
            IndexObject *index = (IndexObject *)FreeIndices[freeIndex];

            // this is a free chunk of memory
            ByteObject *byteObject = (ByteObject *)IndexArray[(*index).ID];

            if ((*byteObject).Size >= size)
            {
                foundIndex = freeIndex;
                found = index;
                break;
            }
        }




        Int64 byteObjectID = (*found).ID;
        ByteObject *byteObjet = (ByteObject *)IndexArray[byteObjectID];
        if ((*byteObjet).Size == size)
        {
            RemoveFreeIndex(foundIndex);
        }
        else
        {
            byteObjectID = AddIndex((*byteObjet).Offset, size, false);
            (*byteObjet).Offset += size;
            (*byteObjet).Size -= size;
        }



        return byteObjectID;
	}
    

    // release the memory so that it can be used
    // by other allocation calls
    unsafe void Deallocate(Int64 index)
    {
        if (index >= 0 && index < IndexArray.Length)
        {
            ByteObject *byteObject = (ByteObject *)IndexArray[index];

            TotalDeallocations++;
                    
            Int64 size = (*byteObject).Size;
            TotalUsedSpace -= size;

            (*byteObject).IsFree = true;
            RemoveFreeIndex(index);
        }
    }
    
}