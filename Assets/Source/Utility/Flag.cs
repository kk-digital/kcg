using System;

namespace Utility
{
    public static class Flag
    {
        public static bool Set<T>(ref T value, T flag) where T : Enum
        {
            if (value.HasFlag(flag)) return false;
            
            ulong numericValue = Convert.ToUInt64(value);
            numericValue |= Convert.ToUInt64(flag);
            value = (T)Enum.ToObject(typeof(T), numericValue);

            return true;
        }
        
        public static bool UnsetFlag<T>(ref T value, T flag) where T : Enum
        {
            if (!value.HasFlag(flag)) return false;
            
            ulong numericValue = Convert.ToUInt64(value);
            numericValue &= ~Convert.ToUInt64(flag);
            value = (T)Enum.ToObject(typeof(T), numericValue);

            return true;
        }
    }
}