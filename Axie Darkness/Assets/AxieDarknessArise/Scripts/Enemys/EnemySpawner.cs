using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ADR.Core;
using ADR.Utilities;
using MoreMountains.Feedbacks;
using System.Linq;

namespace ADR.Enemys
{
    public class EnemySpawner : MonoBehaviour
    {
        //22-12
        [FoldoutGroup("References"), SerializeField] private EnemyResources _enemyResources;
        [FoldoutGroup("References"), SerializeField] private GameObject _enemyPlatform;
        [FoldoutGroup("References"), SerializeField] private List<Transform> _spawnPoints;
        [FoldoutGroup("References"), SerializeField] private Transform _spawnPointsParent;
        [FoldoutGroup("References"), SerializeField] private Transform _enemysParent;

        [FoldoutGroup("Settings"), SerializeField] private List<Enemy> _currentEnemys;
        [FoldoutGroup("Settings"), SerializeField,MinMaxSlider(-10,10)] private Vector2Int _enemyValueRange;
        [FoldoutGroup("Settings"), SerializeField] private Vector3 _spawnPointOffset;
        [FoldoutGroup("Settings"), SerializeField] private int _bossBattleTurn = 50;

        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnSpawnEnemys;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnMoveEnemys;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnSpawnsBoss;

        [FoldoutGroup("Debug"), SerializeField] private List<Transform> _currentSpawnPoints;
        void Start()
        {
            _currentSpawnPoints = new(_spawnPoints);
            GameManager.Instance.SkillTriggered += EnemySpawnerMechanism;
            GameManager.Instance.PlayerWin += DestroyAllEnemys;
        }
        #region Utilities
        public void EnemySpawnerMechanism()
        {
            
            OnMoveEnemys.PlayFeedbacks();

            if(GameManager.Instance.TurnCount < _bossBattleTurn)
            {
                int enemiesTospawn = Random.Range(1, GameManager.Instance.RebornCount);
                for (int i = 0; i < enemiesTospawn; i++)
                {
                    OnSpawnEnemys.PlayFeedbacks();
                }
                
            }
            else if(GameManager.Instance.TurnCount == _bossBattleTurn)
            {
                OnSpawnsBoss.PlayFeedbacks();
            }
            
            
        }
        public void MoveCurrentEnemys()
        {
            ADRUtilities.RemoveNullItems(_currentEnemys);
            _currentEnemys = _currentEnemys.Where(item => item != null).ToList();
            foreach (Enemy enemy in _currentEnemys)
            {
                enemy.TriggerAction();
            }
            GameManager.Instance.AbleToTriggerSkill = true;
        }
        public void SpawnEnemyAtPos(Transform spawnPoint,int multiplier = 1)
        {
            Enemy enemyData = _enemyResources.GetRandomEnemy();
            Enemy enemy = Instantiate(enemyData, spawnPoint.position, Quaternion.identity, _enemysParent);
            enemy.SetUp(0, multiplier);
            _currentEnemys.Add(enemy);
        }
        public void SpawnEnemys()
        {
            Transform spawnPoint = GetRandomSpawnPoint();

            Vector2Int playervalue = new(_enemyValueRange.x+GameManager.Instance.playerValue(),_enemyValueRange.y+GameManager.Instance.playerValue());
            playervalue = ADRUtilities.Clamp(playervalue,0,50);

            Enemy enemyData = _enemyResources.GetRandomEnemy(playervalue, GameManager.Instance.playerValue());
            Enemy enemy = Instantiate(enemyData, spawnPoint.position, Quaternion.identity, _enemysParent);
            enemy.SetUp();
            _currentEnemys.Add(enemy);


        }
        public void SpawnBoss()
        {
            Transform spawnPoint = GetRandomSpawnPoint();

            int playervalue = GameManager.Instance.playerValue();
            //print("Player value" + playervalue);

            Boss bossData = _enemyResources.GetRandomBoss();
            Boss boss = Instantiate(bossData, spawnPoint.position, Quaternion.identity, _enemysParent);
            boss.SetUp(playervalue);
            _currentEnemys.Add(boss);
            

            //GetRandomBoss
        }
        public void DestroyAllEnemys()
        {
            ADRUtilities.RemoveNullItems(_currentEnemys);
            _currentEnemys = _currentEnemys.Where(item => item != null).ToList();
            foreach (Enemy enemy in _currentEnemys)
            {
                if (enemy != null && enemy.EnemyType == Enemy.Type.Normal)
                {
                    enemy.SetDestruction();   
                }
            }
        }
        public Transform GetRandomSpawnPoint()
        {
            if (_currentSpawnPoints.Count == 0) _currentSpawnPoints = new(_spawnPoints);
            int index = Random.Range(0, _currentSpawnPoints.Count - 1);
            Transform spawnPoint = _currentSpawnPoints[index];
            _currentSpawnPoints.RemoveAt(index);

            return spawnPoint;
        }
        [Button]
        private void SpawnObjectsInCircle(float radius, int amount)
        {
            foreach (Transform item in _spawnPoints)
            {
                DestroyImmediate(item.gameObject);
            }
            _spawnPoints.Clear();


            float angleStep = 360.0f / amount;

            for (int i = 0; i < amount; i++)
            {
                float angle = i * angleStep;
                float radians = angle * Mathf.Deg2Rad;

                float x = radius * Mathf.Cos(radians);
                float z = radius * Mathf.Sin(radians);

                Vector3 spawnPosition = transform.position + new Vector3(x+ _spawnPointOffset.x, 0f+ _spawnPointOffset.y, z+_spawnPointOffset.z);
                Transform spawnPoint = Instantiate(_enemyPlatform, spawnPosition, Quaternion.identity, _spawnPointsParent).GetComponent<Transform>();
                spawnPoint.name = "SpawnPoint" + i;
                _spawnPoints.Add(spawnPoint);
            }
        }
        #endregion
    }
}
