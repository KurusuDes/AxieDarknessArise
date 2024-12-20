using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace ADR.Enemys
{
    [SerializeField]
    [CreateAssetMenu(fileName = "Enemys Resources", menuName = "ADR/Resources/EnemysResources", order = 100), InlineEditor]
    public class EnemyResources : SerializedScriptableObject
    {
        [FoldoutGroup("Settings"), SerializeField] private List<Enemy> EnemysPool = new();
        [FoldoutGroup("Settings"), SerializeField] private List<Boss> BossPool = new();
        [FoldoutGroup("Settings"), SerializeField] private Dictionary<int,List<Enemy>> EnemysDatabase = new();

        [Button]
        public void PopulateEnemysDatabase()
        {
            EnemysDatabase.Clear();

            foreach (Enemy enemy in EnemysPool)
            {
                int cost = enemy.Value;
                if (EnemysDatabase.ContainsKey(cost))
                {
                    EnemysDatabase[cost].Add(enemy);
                }
                else
                {
                    List<Enemy> enemyList = new List<Enemy> { enemy };
                    EnemysDatabase.Add(cost, enemyList);
                }
            }
        }
        [Button]
        public Enemy GetRandomEnemy(Vector2Int ValueRange, int PlayerValue)
        {
            List<Enemy> validEnemies = new List<Enemy>();
            foreach (int cost in Enumerable.Range(ValueRange.x, ValueRange.y - ValueRange.x + 1))
            {
                if (EnemysDatabase.TryGetValue(cost, out var EnemysPool))
                {
                    validEnemies.AddRange(EnemysPool);
                }
            }
            if (validEnemies.Count > 0)
            {
                return validEnemies[Random.Range(0, validEnemies.Count)];
            }
            return GetNearestEnemy(PlayerValue);


        }
        public Enemy GetRandomEnemy()
        {
            List<Enemy> allEnemies = new List<Enemy>();

            foreach (var enemyList in EnemysDatabase.Values)
            {
                allEnemies.AddRange(enemyList);
            }

            if (allEnemies.Count > 0)
            {
                return allEnemies[Random.Range(0, allEnemies.Count)];
            }

            return null;


        }
        public Boss GetRandomBoss()
        {
            return BossPool[Random.Range(0, BossPool.Count)];
        }
        public Enemy GetNearestEnemy(int playerValue)
        {
            // Find the nearest key in descending order based on playerValue.
            int nearestKey = -1;
            int minDifference = int.MaxValue;
            foreach (int key in EnemysDatabase.Keys.OrderByDescending(key => key))
            {
                int difference = playerValue - key;
                if (difference >= 0 && difference < minDifference)
                {
                    nearestKey = key;
                    minDifference = difference;
                }
            }

            if (nearestKey != -1)
            {
                // Retrieve an enemy from the nearest key in the EnemysDatabase.
                if (EnemysDatabase.TryGetValue(nearestKey, out var nearestEnemysPool))
                {
                    return nearestEnemysPool[Random.Range(0, nearestEnemysPool.Count)];
                }
            }

            return null;
        }
    }
}
