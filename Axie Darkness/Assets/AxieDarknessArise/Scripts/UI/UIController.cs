using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using ADR.Core;


namespace ADR.UI
{
    public class UIController : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] MMF_Player OnPlayerGotHit;
        void Start()
        {
            GameManager.Instance.PlayerHit += OnPlayerHit;
        }
        private void OnPlayerHit()
        {
            OnPlayerGotHit.PlayFeedbacks();
        }
    }
}
