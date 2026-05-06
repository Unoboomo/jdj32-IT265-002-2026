using UnityEngine;
using System.Collections.Generic;

public static class ListExtensions 
{
   public static T Draw<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            Debug.Log("Tried to draw a card from an empty list");
            return default;
        }
        int r = Random.Range(0, list.Count);
        T t = list[r];
        list.RemoveAt(r);
        return t;
    }
}
