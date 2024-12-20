using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;

namespace ADR.Utilities
{
    public class SplineAnimatorController : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] private SkeletonAnimation _animator;
        void Start()
        {

        }

        public void TriggerContinousAnimation(string animationName)
        {
            _animator.AnimationState.SetAnimation(0, animationName, false);
            _animator.AnimationState.AddAnimation(0, "action/idle/normal", true, 0);
        }
        public void TriggerAnimation(string animationName)
        {
            _animator.AnimationState.SetAnimation(0, animationName, false);
        }
    }
}
