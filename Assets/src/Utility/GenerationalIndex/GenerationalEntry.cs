namespace Assets.src.Utility.GenerationalIndex
{
    public struct GenerationalEntry<T>
    {
        public T Value { get; set; }
        public bool IsFree { get; set; }
        public int Generation { get; set; }
    }
}
