using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UDL.Core
{
    public class WeightedLottery
    {
        public static List<T> GetList<T>(List<Tuple<float, T>> tuples, int count, int seed = -1)
        {
            System.Random random = new System.Random(seed);

            if(tuples.Count == 0)
            {
                Debug.LogWarning("Lottery can't choose anything because tuples are empty.");
                return new List<T>();
            }

            List<T> results = new List<T>();

            tuples.Sort((x, y) => (int)((y.Item1 - x.Item1) * 100000));

            float totalFrequency = tuples.Select(x => x.Item1).Sum();

            while (results.Count < count)
            {
                float r = (seed == -1) ? UnityEngine.Random.Range(0, totalFrequency) : (float)(random.NextDouble() * totalFrequency);
                foreach (var tuple in tuples)
                {
                    if (tuple.Item1 > r)
                    {
                        results.Add(tuple.Item2);
                        break;
                    }
                    else
                    {
                        r -= tuple.Item1;
                    }
                }
            }
            return results;
        }
    }
}
