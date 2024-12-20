using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ADR.Core;
using UnityEngine.InputSystem;
using MoreMountains.Feedbacks;
using Doozy.Runtime.UIManager.Animators;

namespace ADR.UI
{
    public class UIPauseMenu : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] private UIContainerUIAnimator _container;

        [FoldoutGroup("Feedbacks"), SerializeField] private MMF_Player OnOpenPauseMenu;
        [FoldoutGroup("Feedbacks"), SerializeField] private MMF_Player OnClosePuaseMenu;
        [FoldoutGroup("Debug"), SerializeField] private bool IsMenuOpen = false;
        void Start()
        {
            GameManager.Instance.AxieControls.UI.OpenPauseMenu.performed += TriggerPauseMenu;
        }

        private void TriggerPauseMenu(InputAction.CallbackContext context)
        {
            BtnPause();
        }
        public void BtnPause()
        {
           
            if (!IsMenuOpen)
            {
                _container.Show();
                OnOpenPauseMenu.PlayFeedbacks();
            }
            else
            {
                _container.Hide();
                OnClosePuaseMenu.PlayFeedbacks();
            }
            IsMenuOpen = !IsMenuOpen;
        }
    }
}

