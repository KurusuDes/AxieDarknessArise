using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using Doozy.Runtime.UIManager.Containers;
using ADR.Core;

namespace ADR.UI
{

    public class UIStatistics : MonoBehaviour
    {
        [FoldoutGroup("References"),SerializeField]private UITooltip tooltip;
        void Start()
        {
            GameManager.Instance.OnComboAdded += UpdateValues;
            GameManager.Instance.SkillTriggered += UpdateValues;
            GameManager.Instance.PlayerLoss += UpdateValues;
            GameManager.Instance.PlayerWin += UpdateValues;

            UpdateValues();
        }
        private void UpdateValues()
        {
            tooltip.SetTexts((GameManager.Instance.RebornCount -1).ToString(),GameManager.Instance.MaxCombo.ToString(), GameManager.Instance.MaxTurn.ToString());
        }
    }
}
