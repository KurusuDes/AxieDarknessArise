using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using MoreMountains.Feedbacks;
using Doozy.Runtime.UIManager.Animators;

namespace ADR.UI
{
    public class UICardHolder : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] private UISkillCard _skillCard;
        [FoldoutGroup("References"), SerializeField] private TextMeshProUGUI _slotTxt;
        [FoldoutGroup("References"), SerializeField] private Transform _anchor;
        [FoldoutGroup("Settings"), SerializeField] private int _slotNumber = 0;
        [FoldoutGroup("Feebacks"), SerializeField] private UIContainerUIAnimator _animator;
        public int Slot = 0;
        public Transform Anchor => _anchor;
        public UISkillCard SkillCard => _skillCard;
        public UIContainerUIAnimator Animator => _animator;
        public void SetUp(int slot)
        {
            _slotNumber = slot;
            _slotTxt.text = slot.ToString();
            Slot = slot;
        }
        #region External
        public void AddSkillCard(UISkillCard Card)
        {
            _skillCard = Card;
        }
        public void RemoveSkillCard()
        {
            _skillCard = null;
        }
        public void ShowCard()
        {
            _animator.Show();
        }
        public void HideCard()
        {
            _animator.Hide();
        }
        #endregion
    }
}
