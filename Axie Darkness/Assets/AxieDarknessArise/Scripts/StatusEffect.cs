using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ADR.Skills
{
    public class StatusEffect
    {
        public enum Type
        {
            Poison,
            Stunned,
            Cleanser
        }
        [FoldoutGroup("Settings"), SerializeField] private Type _type;
        [FoldoutGroup("Settings"), SerializeField] private int stacks;
    }
}
