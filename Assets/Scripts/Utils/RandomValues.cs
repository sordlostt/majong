using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class RandomValues
{
    static System.Random random;

    public static void Init()
    {
        random = new System.Random();
    }

    public static int GetRandom(int limit)
    {
        return random.Next(limit);
    }

    public static IEnumerable<T> ShuffleCollection<T>(IEnumerable<T> collection)
    {
        var source = collection.ToList();

        int i = source.Count;
        while (i > 1)
        {
            i--;
            int n = random.Next(i + 1);
            var tmp = source[n];
            source[n] = source[i];
            source[i] = tmp;
        }

        return source;
    }
}
