namespace KMath.Random {
    public struct XorShift {
        private ulong a;

        public void SetSeed(ulong seed) {
            if (seed == 0) return; // seed cannot be 0
            a = seed;
        }

        public ulong GetUInt64() {
            ulong x = a;
            x ^= x >> 12; // a
            x ^= x << 25; // b
            x ^= x >> 27; // c
            a = x;
            return x * 0x2545F4914F6CDD1DUL;
        }

        public uint GetUInt32() {
            ulong tmp = GetUInt64();
            return (uint)((tmp * 0x2545F4914F6CDD1DUL) & 0x00000000FFFFFFFFUL);
        }

        public void FillArray(byte[] arr) {
            for(int i = 0; i < arr.Length / 4; i++) {
                uint tmp = GetUInt32();
                arr[i * 4 + 0] = (byte) (tmp & 0x000000FF);
                arr[i * 4 + 1] = (byte)((tmp & 0x0000FF00) >>  8);
                arr[i * 4 + 2] = (byte)((tmp & 0x00FF0000) >> 16);
                arr[i * 4 + 3] = (byte)((tmp & 0xFF000000) >> 24);
            }
        }
    }
}
