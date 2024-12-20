using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using ADR.Core;
namespace ADR.UI
{
    public class UIGameOverScreen : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] private MMF_Player OnShowGameOver;
        void Start()
        {
            GameManager.Instance.PlayerLoss += ShowLossScreen;
        }
        private void ShowLossScreen()
        {
            OnShowGameOver.PlayFeedbacks();
        }


    }
}

