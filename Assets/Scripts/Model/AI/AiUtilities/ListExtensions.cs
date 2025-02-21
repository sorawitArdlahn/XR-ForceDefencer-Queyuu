 using System;
 using System.Collections.Generic;

 public static class ListExtensions
 {
     private static Random rng;

     public static IList<T> Shuffle<T>(this IList<T> list)
     {
         if (rng == null) rng = new Random();
         int count = list.Count;
         while (count > 1)
         {
             --count;
             int index = rng.Next(count + 1);
             (list[index], list[count])  = (list[count], list[index]);
         }

         return list;
     }
 }
