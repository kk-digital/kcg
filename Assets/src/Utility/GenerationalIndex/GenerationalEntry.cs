namespace Assets.src.Utility.GenerationalIndex
{
    public struct GenerationalEntry<T>
    {
        public T Value;
        public bool IsFree;
        public int Generation;
    }
}
