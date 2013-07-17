using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coob
{
    class EntityBitmask
    {
        BitArray bitArray;

        public EntityBitmask(byte[] mask)
        {
            bitArray = new BitArray(mask);
        }

        public int GetDataSize()
        {
            int size = 0;
            increaseBy(ref size, new int[] { 7, 9, 21, 22, 31, 32, 37, 42 }, 1);
            increaseBy(ref size, new int[] { 14 }, 2);
            increaseBy(ref size, new int[] { 5, 6, 8, 10, 11, 12, 15, 16, 17, 18, 19, 20, 23, 27, 28, 29, 33, 34, 38, 47 }, 4);
            increaseBy(ref size, new int[] { 35, 36 }, 8);
            increaseBy(ref size, new int[] { 1, 2, 3, 4, 24, 25, 26, 39, 41 }, 12);
            increaseBy(ref size, new int[] { 45 }, 16);
            increaseBy(ref size, new int[] { 30 }, 20);
            increaseBy(ref size, new int[] { 0, 40 }, 24);
            increaseBy(ref size, new int[] { 46 }, 44);
            increaseBy(ref size, new int[] { 13 }, 172);
            increaseBy(ref size, new int[] { 43 }, 280);
            increaseBy(ref size, new int[] { 44 }, 3640);
            return size;
        }

        void increaseBy(ref int size, int[] indices, int val)
        {
            foreach (int i in indices)
            {
                if (bitArray.Get(i))
                    size += val;
            }
        }
    }
}
