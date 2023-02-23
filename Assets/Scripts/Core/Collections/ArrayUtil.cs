using System;
using System.Collections.Generic;

namespace Rogue.Core
{
    /// <summary>
    /// Defines a static class which contains utilities for arrays.
    /// </summary>
    public static class ArrayUtil
    {
        /// <summary>
        /// Shuffle the elements in an array.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the list.</typeparam>
        /// <param name="list">List to shuffle.</param>
        /// <param name="begin">First index to shuffle.</param>
        /// <param name="end">Last index to shuffle.</param>
        public static void Shuffle<T>(IList<T> list, int begin, int end)
        {
            if (begin > end)
            {
                throw new ArgumentException("Begin can no be greather than the end");
            }

            for (int i = end; i >= begin; i--)
            {
                int s  = (int)((end - begin + 1) * UnityEngine.Random.value);

                if (s < end)
                {
                    T temp  = list[s];
                    list[s] = list[i];
                    list[i] = temp;
                }
            }
        }

        /// <summary>
        /// Rotate the elements of a list to the right.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the list.</typeparam>
        /// <param name="list">List to rotate.</param>
        public static void RotateRight<T>(IList<T> list)
        {
            if (list == null || list.Count <= 1)
            {
                return;
            }

            T tail = list[list.Count - 1];

            for (int i = list.Count - 1; i > 0; i--)
            {
                list[i] = list[i - 1];
            }
            list[0] = tail;
        }

        public static void Swap<T>(IList<T> list, int a, int b)
        {
            var tmp = list[a];
            list[a] = list[b];
            list[b] = tmp;
        }

        public static void RemoveAndSwap<T>(IList<T> list, int index)
        {
            int a = index;
            int b = list.Count - 1;

            Swap(list, a, b);
            list.RemoveAt(b);
        }
    }
}
