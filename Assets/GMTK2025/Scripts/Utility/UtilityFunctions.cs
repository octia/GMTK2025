using System.Collections.Generic;
using UnityEngine;

public static class UtilityFunctions
{
    public static void Shuffle<T>(IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static int GetKeyOrLast(int i, int length)
    {
        return i >= length ? length - 1 : i;
    }
}
