using System;
using System.Collections;
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
}
