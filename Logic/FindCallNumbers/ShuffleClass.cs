using System;
using System.Collections.Generic;

//https://stackoverflow.com/questions/273313/randomize-a-listt
namespace JoshMkhariPROG7312Game.Logic.FindCallNumbers
{
    public static class ShuffleClass
    {
        private static Random rng = new Random();  

        public static void Shuffle<T>(this IList<T> list)  
        {  
            int n = list.Count;  
            while (n > 1) {  
                n--;  
                int k = rng.Next(n + 1);  
                (list[k], list[n]) = (list[n], list[k]);
            }  
        }
    }
}