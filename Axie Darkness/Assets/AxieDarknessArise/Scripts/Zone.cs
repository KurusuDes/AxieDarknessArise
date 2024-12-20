using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ADR.Core;
using ADR.Enemys;

namespace ADR.Zones
{
    public enum Type
    {
        Inner,
        Mid,
        External
    }
    
    public class Neighbors
    {
        [FoldoutGroup("Neighbors"), SerializeField] public GameObject Left;
        [FoldoutGroup("Neighbors"), SerializeField] public GameObject Right;
    }

    public class Zone : MonoBehaviour
    {
        [FoldoutGroup("Settings"), ShowInInspector,HideLabel] private Neighbors _neighbors = new();
        [FoldoutGroup("Settings"), SerializeField,Range(1,3)] private int _maximunAmount = 1;
        [FoldoutGroup("Settings"), SerializeField] private List<Enemy> _enemys = new();
        [FoldoutGroup("Settings"), SerializeField] private Type _type;
        [FoldoutGroup("Settings"), SerializeField] private Direction _direction;

        private void Start()
        {
            
        }
        public void AddEnemy(Enemy enemy)
        {
            if(CheckAvalibity(enemy))
            {
                _enemys.Add(enemy);

            }
        }
        public void AddEnemy(List<Enemy> enemys)
        {
            if (CheckAvalibity(enemys))
            {
                _enemys.AddRange(enemys);

            }
        }
        private bool CheckAvalibity(Enemy enemy)
        {
            if (_enemys.Count >= _maximunAmount-1)
                return false;
            else
                return true;
        }
        private bool CheckAvalibity(List<Enemy> enemy)
        {
            if (_enemys.Count + enemy.Count > _maximunAmount-1)
                return false;
            else
                return true;
        }
    }
}
