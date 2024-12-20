using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using ADR.Core;
namespace ADR.UI
{
    public class UICompleteStageScreen : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] private MMF_Player OnCompleteGame;
        void Start()
        {
            GameManager.Instance.PlayerWin += ShowWinScreen;
        }
        private void ShowWinScreen()
        {
            OnCompleteGame.PlayFeedbacks();
            print("winner");
        }


    }
}

