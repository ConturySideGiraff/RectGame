
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class Util
{
    public static int[] GetNoneOverlapNumbers(int maxCount, int n)
    {
        var source = new int[maxCount];
        for (var i = 0; i < maxCount; i++)
        {
            source[i] = i;
        }

        var result = new int[n];
        for (var i = 0; i < n; i++)
        {
            var pick = Random.Range(0, maxCount);

            result[i] = source[pick];
            source[pick] = source[maxCount - 1];
            maxCount -= 1;
        }

        return result;
    }
    
    public static int[] GetNoneOverlapNumbers(int n)
    {
        var maxCount = n;
        
        var source = new int[maxCount];
        for (var i = 0; i < maxCount; i++)
        {
            source[i] = i;
        }

        var result = new int[n];
        for (var i = 0; i < n; i++)
        {
            var pick = Random.Range(0, maxCount);

            result[i] = source[pick];
            source[pick] = source[maxCount - 1];
            maxCount -= 1;
        }

        return result;
    }
}
