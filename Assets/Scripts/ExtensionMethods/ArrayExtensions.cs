using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayExtensions
{
    public static void ForEach<T>(this T[] array, Action<T> action)
    {
        for (int i = 0; i < array.Length; i++)
        {
            action(array[i]);
        }
    }
}