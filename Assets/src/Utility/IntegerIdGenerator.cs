using System;
using System.Collections.Generic;

namespace Assets.src.Utility
{
    public class IntegerIdGenerator
    {
        private readonly Random random = new Random();
        private readonly List<int> randomList = new List<int>();

        public int NewId()
        {
            int newId = random.Next(0, 1000);

            while (randomList.Contains(newId))
                newId = random.Next(0, 1000);

            randomList.Add(newId);

            return newId;
        }
    }
}
