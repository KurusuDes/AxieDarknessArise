using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ADR.Core;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using TMPro;
using Doozy.Runtime.UIManager.Components;

namespace ADR.UI
{

    public class UIManaBar: MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] private UISlider _externalSlider;
        [FoldoutGroup("References"), SerializeField] private UISlider _internalSlider;

        [FoldoutGroup("Feedbacks"), SerializeField] private MMF_Player OnNotEnoughMana;

        void Start()
        {
            _externalSlider.maxValue = GameManager.Instance.MaxMana;
            _internalSlider.maxValue = GameManager.Instance.MaxMana;
            _externalSlider.value = 0;
            _externalSlider.value = 0;
        }
        public void UpdateEnergyBar()
        {
            if (_externalSlider == null || _internalSlider == null) return;

            _externalSlider.value = Mathf.FloorToInt(GameManager.Instance.CurrentMana);
            _internalSlider.value = GameManager.Instance.CurrentMana;
        }
        public void NotEnoughtEnergy()
        {
            OnNotEnoughMana.PlayFeedbacks();
        }
    }
}
