using System;
using System.Collections.Generic;
using UnityEngine;

namespace ADR.Utilities
{
    public class ADRUtilities
    {
        public static void RemoveNullItems<T>(List<T> list)
        {
            list.RemoveAll(item => item == null );
        }
        public static Vector2Int Clamp(Vector2Int value, int min, int max)
        {
            int x = Mathf.Clamp(value.x, min, max);
            int y = Mathf.Clamp(value.y, min, max);
            return new Vector2Int(x, y);
        }
        public static void ShuffleList<T>(List<T> list)
        {
            int n = list.Count;
            System.Random rng = new System.Random();

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static List<int> GenerateRandomList(int quantity)
        {
            List<int> randomList = new List<int>();

            for (int i = 0; i < quantity; i++)
            {
                randomList.Add(i);
            }

            // Shuffle the list to make it random.
            for (int i = 0; i < quantity; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, quantity);
                int temp = randomList[i];
                randomList[i] = randomList[randomIndex];
                randomList[randomIndex] = temp;
            }

            return randomList;
        }
    }
}
