using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using MoreMountains.Feedbacks;
using ADR.Skills;
using TMPro;
using ADR.Core;
using Doozy.Runtime.UIManager.Animators;

namespace ADR.UI
{
    public class UISkillCard : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField,DisableInEditorMode] private Skill _skill;

        [FoldoutGroup("References"), SerializeField] private Image _card;
        [FoldoutGroup("References"), SerializeField] private TextMeshProUGUI _name;
        [FoldoutGroup("References"), SerializeField] private TextMeshProUGUI _energyTxt;
        [FoldoutGroup("References"), SerializeField] private TextMeshProUGUI _damageTxt;
        [FoldoutGroup("References"), SerializeField] private Image _skillRangeIcon;
        [FoldoutGroup("References"), SerializeField,DisableInEditorMode] private UICardHolder _parent;
        [FoldoutGroup("References"), SerializeField] public UIContainerUIAnimator Animator;

        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnSpawn;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnSelect;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnTrigger;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnHasNoMana;

        public Skill Skill => _skill;

        private void Start()//cambiar despues
        {
        }
        public void SetUp(Skill skill,UICardHolder uiHolder)
        {
            _skill = skill;
            _card.sprite = skill.Entity.Icon;
            _name.text = skill.Entity.Name;
            _energyTxt.text = skill.ManaCost.ToString();
            _damageTxt.text = skill.Damage.ToString();
            _parent = uiHolder;
            _skillRangeIcon.sprite = GameManager.Instance.SkillResources.GetAreaReference(skill.AreaReference);
            OnSpawn.PlayFeedbacks();
        }
        public void OnSkillTrigger()
        {
            OnTrigger.PlayFeedbacks();
            GameManager.Instance.UICardsHolder.RemoveFromList(this);
            GameManager.Instance.RemoveSkillSlot();
            GameManager.Instance.SkillCardUsed();
            _parent.Animator.Hide();
        }
        #region External
        public void BtnSetSkill()
        {
            if (!GameManager.Instance.HasMana(_skill))
            {
                GameManager.Instance.UIManaBar.NotEnoughtEnergy();
                OnHasNoMana.PlayFeedbacks();
                return;
            }
            _parent.Animator.Show();
            GameManager.Instance.SetSkillSlot(_skill, this);
            GameManager.Instance.UICardsHolder.HideAllCardsExcept(_parent.Slot - 1);
            OnSelect.PlayFeedbacks();
            /*
            if (GameManager.Instance.SelectedSkill == null || GameManager.Instance.SelectedSkill.Skill != _skill)
            {
                _parent.Animator.Show();
                GameManager.Instance.SetSkillSlot(_skill, this);
                GameManager.Instance.UICardsHolder.HideAllCardsExcept(_parent.Slot - 1);
                OnSelect.PlayFeedbacks();
            }
            else
            {
                _parent.Animator.Hide();
                GameManager.Instance.RemoveSkillSlot();
               
            }*/
        }

        #endregion
    }
}

