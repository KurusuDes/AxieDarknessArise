using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ADR.Skills;
using ADR.Core;

namespace ADR.Core
{
    public enum BodyParts
    {
        Eyes,
        Mouth,
        Ears,
        Horn,
        Back,
        Tail
    }
    [CreateAssetMenu(fileName = "Axie", menuName = "ADR/Resources/Axie", order = 100), InlineEditor]
    public class Axie : SerializedScriptableObject
    {
        [FoldoutGroup("Entity"),HideLabel, SerializeField] public Entity Entity = new();
        [FoldoutGroup("Entity"), HideLabel, SerializeField] public GameObject AxieModelPrefab;
        [FoldoutGroup("Settings"), SerializeField] public Dictionary<BodyParts, Skill> Skills = new();
        [FoldoutGroup("Settings"), SerializeField] public int BaseLife = 400;

        public List<Skill> GetAxieSkills()
        {
            List<Skill> skills = new();
            foreach (var item in Skills)
            {
                if (item.Value != null)
                    skills.Add(item.Value);
            }
            return skills;
        }
        public int AverageDamage()
        {
            int averageDamage = 0;
            foreach (var item in Skills)
            {
                if (item.Value != null)
                    averageDamage += item.Value.Damage;
            }
            //Debug.Log("Average basic output damage: "+averageDamage / Skills.Count);
            return averageDamage/ Skills.Count;
        }
    }
}
