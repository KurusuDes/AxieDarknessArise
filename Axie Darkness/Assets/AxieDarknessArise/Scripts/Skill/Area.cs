using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ADR.Skills
{
    public enum SkillRange
    {
        CloseRange,
        LongRange
    }
    public class Area
    {

        [BoxGroup("Area"), SerializeField] private SkillRange _skillRange;

        [BoxGroup("Area"), SerializeField,ShowIf("_skillRange", SkillRange.LongRange)] private float _maxRadius;
        [BoxGroup("Area"), SerializeField] private float _radius;
        [BoxGroup("Area"), SerializeField] private float _amplitude;
        [BoxGroup("Area"), SerializeField] private bool _targetable;
        //this means that the start point of the area is not from the charactert its a zone that can be put inside the max radius range
       

        public float MaxRadius => _maxRadius;
        public float Amplitude => _amplitude;
        public bool Targetable => _targetable;
        public float Radius => _radius;

    }
}
