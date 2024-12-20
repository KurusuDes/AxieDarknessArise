using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ADR.Core;
using MoreMountains.Feedbacks;
using TMPro;

namespace ADR.UI
{
    public class UITurnCount : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] private TextMeshProUGUI _turnText;
        void Start()
        {
            GameManager.Instance.SkillTriggered += UpdateTurnText;
            GameManager.Instance.OnTurnPass += UpdateTurnText;
            _turnText.text = "0";
        }
        private void UpdateTurnText()
        {
            _turnText.text = GameManager.Instance.TurnCount.ToString();
        }
    }
}
