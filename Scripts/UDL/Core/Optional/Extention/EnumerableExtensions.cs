using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UDL.Core
{
    public static class EnumerableExtensions
    {
        public static T RandomPick<T>(this List<T> list)
        {
            if(list == null || list.Count == 0)
            {
                return default;
            }
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T RandomPick<T>(this List<T> list, System.Random random)
        {
            if (list == null || list.Count == 0)
            {
                return default;
            }
            return list[random.Next(list.Count)];
        }

        public static void Shuffle<T>(this List<T> sequence)
        {
            for (var i = sequence.Count() - 1; i > 0; i--)
            {
                var swapIndex = UnityEngine.Random.Range(0, i);   // get num between 0 and index
                if (swapIndex == i) continue;           // don't replace with itself
                var temp = sequence[i];                 // get item at index i...
                sequence[i] = sequence[swapIndex];      // set index i to new item
                sequence[swapIndex] = temp;             // place temp-item to swap-slot
            }
        }

        public static void Shuffle<T>(this T[] sequence)
        {
            for (var i = sequence.Count() - 1; i > 0; i--)
            {
                var swapIndex = UnityEngine.Random.Range(0, i);   // get num between 0 and index
                if (swapIndex == i) continue;           // don't replace with itself
                var temp = sequence[i];                 // get item at index i...
                sequence[i] = sequence[swapIndex];      // set index i to new item
                sequence[swapIndex] = temp;             // place temp-item to swap-slot
            }
        }
    }
}
