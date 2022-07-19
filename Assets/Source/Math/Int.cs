using System.Runtime.CompilerServices;

namespace KMath
{
    public static class Int
    {
        [MethodImpl((MethodImplOptions) 256)]
        public static int IntMin(int val1, int val2) => val1 > val2 ? val2 : val1;
        
        [MethodImpl((MethodImplOptions) 256)]
        public static int IntMax(int val1, int val2) => val1 < val2 ? val2 : val1;
    }
}

