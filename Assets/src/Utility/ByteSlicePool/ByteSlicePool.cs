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


    public void ExpandIfFull()
    {
        Int64 currentCapacity = Capacity;
        if (Length >= Capacity)
        {
            Expand((Int64)(currentCapacity * 1.25) + 1024);
        }
    }
    public void Expand(Int64 newCapacity)
    {
        if (newCapacity > Capacity)
        {
            Capacity = newCapacity;
            Memory = (void*)Marshal.ReAllocHGlobal((IntPtr)Memory, (IntPtr)(Capacity * ElementSize));
        }
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


public class ByteSlicePool
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


    // TODO(mehdi): maybe use a better data structure
    // List is a dynamic array and deleting element from that
    // is O(N) complexity
    UnmanagedArray IndexArray;
    UnmanagedArray FreeIndices;
    
    
    unsafe ByteSlicePool()
    {
        IndexArray = new UnmanagedArray(1024, sizeof(ByteObject));
        FreeIndices = new UnmanagedArray(1024, sizeof(IndexObject));

        Int64 BaseSize = 1024 * 1024;
        DataArray = (IntPtr)Marshal.AllocHGlobal((IntPtr)BaseSize);
        DataArrayCapacity = BaseSize;

        Int64 blockID = AddIndex(0, BaseSize, true);
        AddFreeIndex(blockID);

    }

    ~ByteSlicePool()
    {
       IndexArray.Destroy();
       FreeIndices.Destroy(); 

       Marshal.FreeHGlobal(DataArray);
       DataArrayCapacity = 0;
    }


    unsafe Int64 AddIndex(Int64 offset, Int64 size, bool IsFree)
    {
        IndexArray.ExpandIfFull();

        *((ByteObject *)IndexArray[IndexArray.Length]) = 
                    new ByteObject{ Offset = offset, Size = size, IsFree = IsFree};
        IndexArray.Length++;

        return 0;
    }

    unsafe void AddFreeIndex(Int64 blockId)
    {
        FreeIndices.ExpandIfFull();

        *((IndexObject *)FreeIndices[FreeIndices.Length]) = new IndexObject{ ID = blockId};
        FreeIndices.Length++;
    }

    void Expand(Int64 newCapacity)
    {
        if (newCapacity > DataArrayCapacity)
        {
            DataArrayCapacity = newCapacity;
            DataArray = Marshal.ReAllocHGlobal((IntPtr)DataArray, (IntPtr)(DataArrayCapacity));
        }
    }
    
    // used to get the memory from an index
    // the index is provided in the Allocate method
    unsafe IntPtr Get(Int64 index)
    {
        IntPtr ptr = (IntPtr)0;

        if (index >= 0 && index < IndexArray.Length)
        {
            ptr = (IntPtr)IndexArray[index];
        }

        return ptr;
    }

    // Allocate memory 
    // returns an identifier to the memory block
    // the memory can be accesed using the Get function
    // so that we can keep track of its usage every frame
    Int64 Allocate(int size)
    {

        return 0;
	}
    

    // release the memory so that it can be used
    // by other allocation calls
    void Deallocate(Int64 index)
    {
    }
    
}