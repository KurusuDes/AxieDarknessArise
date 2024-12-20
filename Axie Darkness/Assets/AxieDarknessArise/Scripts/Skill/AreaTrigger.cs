using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ADR.Skills;
using ADR.Enemys;
using MoreMountains.Feedbacks;


namespace ADR.Skills
{
    public class AreaTrigger : MonoBehaviour
    {
        [SerializeField] private List<Enemy> triggerEnemies = new List<Enemy>();
        [SerializeField] private List<MMF_Player> triggerZones = new List<MMF_Player>();

        #region Utilities
        public List<Enemy> GetTargets()
        {
            triggerEnemies.RemoveAll(item => item == null || item.Equals(null));
            List<Enemy> enemies = new(triggerEnemies);

            return enemies;

        }
        public List<MMF_Player> GetPlatforms()
        {
            triggerZones.RemoveAll(item => item == null || item.Equals(null));
            List<MMF_Player> platforms = new(triggerZones);

            return platforms;

        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger) return;
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();

                if (enemy.CurrentState == Enemy.State.Idle &&!triggerEnemies.Contains(enemy))
                {
                    triggerEnemies.Add(enemy);
                    enemy.OnSelected(true);
                }
            }
            if (other.CompareTag("Platform"))
            {
                MMF_Player platform = other.gameObject.GetComponent<MMF_Player>();
                if (!triggerZones.Contains(platform))
                {
                    triggerZones.Add(platform);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.isTrigger) return;
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                enemy.OnSelected(false);
                triggerEnemies.Remove(enemy);
            }
            if (other.CompareTag("Platform"))
            {
                MMF_Player platform = other.gameObject.GetComponent<MMF_Player>();
                triggerZones.Remove(platform);
            }
        }
        #endregion
    }
}
