using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using ADR.Core;
using DG.Tweening;
using UnityEngine.InputSystem;
using ADR.Utilities;

namespace ADR.UI
{
    public class UICardsHolder: MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] private UISkillCard _skillCard;
        [FoldoutGroup("References"), SerializeField] private Transform _skillCardTransform;
        [FoldoutGroup("References"), SerializeField] private UICardHolder _cardHolderPrefab;
        [FoldoutGroup("References"), SerializeField] private List<UICardHolder> _cardsHolder;

        [FoldoutGroup("Settings"), SerializeField] private float _delayToAddCards;
        [FoldoutGroup("Settings"), SerializeField] private int InternalCount = 0;
        [FoldoutGroup("Settings"), SerializeField] private bool ultiAvalible = false;
        //[FoldoutGroup("Settings"), SerializeField] public MMF_Player HideAllCardHolders;

        void Start()
        {
            GameManager.Instance.SkillTriggered += PopulateCardHolder;
            for (int i = 0; i < GameManager.Instance.GetMaxSkillsAvalible; i++)
            {
                UICardHolder cardHolder = Instantiate(_cardHolderPrefab, _skillCardTransform);
                cardHolder.SetUp(i + 1);
                _cardsHolder.Add(cardHolder);
            }
            PopulateCardHolder();
            #region Inputs
            //_axieControls.Enable();
            GameManager.Instance.AxieControls.GameInputs.Ability1.performed += AbilityReader;
            GameManager.Instance.AxieControls.GameInputs.Ability2.performed += AbilityReader;
            GameManager.Instance.AxieControls.GameInputs.Ability3.performed += AbilityReader;
            GameManager.Instance.AxieControls.GameInputs.Ability4.performed += AbilityReader;
            GameManager.Instance.AxieControls.GameInputs.Ability5.performed += AbilityReader;
            #endregion
            GameManager.Instance.PlayerLoss += RemoveInputs;
        }
        private void RemoveInputs()
        {
            GameManager.Instance.AxieControls.GameInputs.Ability1.performed -= AbilityReader;
            GameManager.Instance.AxieControls.GameInputs.Ability2.performed -= AbilityReader;
            GameManager.Instance.AxieControls.GameInputs.Ability3.performed -= AbilityReader;
            GameManager.Instance.AxieControls.GameInputs.Ability4.performed -= AbilityReader;
            GameManager.Instance.AxieControls.GameInputs.Ability5.performed -= AbilityReader;
        }
        private void AbilityReader(InputAction.CallbackContext context)
        {
            string abilityName = context.action.name;
            char lastCharacter = abilityName[abilityName.Length - 1];

            if (int.TryParse(lastCharacter.ToString(), out int AbilitySlot))
            {
                //print("AbilitySlot: " + AbilitySlot);
                TriggerAbilitySlot(AbilitySlot - 1);
            }
            else
                print("Error");
        }
        private void TriggerAbilitySlot(int slot)
        {
            if (slot >= 0 && slot <= _cardsHolder.Count - 1)
            {
                if (_cardsHolder[slot] != null && _cardsHolder[slot].SkillCard != null)
                {
                    _cardsHolder[slot].SkillCard.BtnSetSkill();
                }
                //ShowSelectedCard(slot);
            }
           
        }
       /* private void ShowSelectedCard(int slot)
        {
            for (int i = 0; i < _cardsHolder.Count; i++)
            {
                if(i == slot)
                {
                    _cardsHolder[i].ShowCard();
                }
                else
                {
                    _cardsHolder[i].HideCard();
                }
            }
        }*/
        public void HideAllCardsExcept(int slot)
        {
            for (int i = 0; i < _cardsHolder.Count; i++)
            {
                if (i != slot)
                    _cardsHolder[i].Animator.Hide();             
            }
        }
        private void OnDestroy()
        {
            GameManager.Instance.SkillTriggered -= PopulateCardHolder;

        }
        public void RemoveFromList(UISkillCard uiSkillCard)
        {
            foreach (UICardHolder cardHolder in _cardsHolder)
            {
                if(cardHolder.SkillCard == uiSkillCard)
                {
                    cardHolder.RemoveSkillCard();
                    break;
                }
            }
            InternalCount--;
        }
        [Button]
        public void UpgradeRandomCard()
        {
            ultiAvalible = true;
            //int randomIndex = Random.Range(0, _cardsHolder.Count);
            /*List<int> randomIndex = ADRUtilities.GenerateRandomList(_cardsHolder.Count);

            for (int i = 0; i < _cardsHolder.Count; i++)
            {
                if (_cardsHolder[randomIndex[i]] != null 
                    && _cardsHolder[randomIndex[i]].SkillCard != null 
                    && _cardsHolder[randomIndex[i]].SkillCard.Skill.SkillUpgrade !=null)
                {
                    UISkillCard card = _cardsHolder[randomIndex[i]].SkillCard;
                    card.Animator.InstantHide();
                    _cardsHolder[randomIndex[i]].SkillCard.SetUp(card.Skill.SkillUpgrade,_cardsHolder[randomIndex[i]]);
                    //card.OnTrigger.PlayFeedbacks();
                    break;
                }
            }*/
        }
        private void PopulateCardHolder()
        {
            if(InternalCount < GameManager.Instance.GetMaxSkillsAvalible)
            {
                StartCoroutine(AddSkillCard(_delayToAddCards));
                InternalCount++;
            }
        }
        IEnumerator AddSkillCard(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            foreach (UICardHolder cardHolder in _cardsHolder)
            {
                if(cardHolder.SkillCard == null)
                {
                    if(ultiAvalible == false)
                    {
                        UISkillCard uiSkillCard = Instantiate(_skillCard, cardHolder.Anchor.transform);
                        uiSkillCard.SetUp(GameManager.Instance.SkillsAvalible[Random.Range(0, GameManager.Instance.SkillsAvalible.Count)], cardHolder);
                        cardHolder.AddSkillCard(uiSkillCard);
                    }
                    else
                    {
                        UISkillCard uiSkillCard = Instantiate(_skillCard, cardHolder.Anchor.transform);
                        uiSkillCard.SetUp(GameManager.Instance.SkillsAvalible[Random.Range(0, GameManager.Instance.SkillsAvalible.Count)].SkillUpgrade, cardHolder);
                        cardHolder.AddSkillCard(uiSkillCard);
                        ultiAvalible = false;
                    }
                    
                }
            }
            PopulateCardHolder();
        }
        /*[Button]
        public void RefillList<T>(this List<T> list)
        {
            if (list == null || list.Count < 2)
            {
                return; // The list is too short to have gaps.
            }

            // Use the reverse loop to avoid issues with shifting elements.
            for (int i = list.Count - 1; i > 0; i--)
            {
                if (EqualityComparer<T>.Default.Equals(list[i], default(T)))
                {
                    for (int j = i; j < list.Count - 1; j++)
                    {
                        list[j] = list[j + 1];
                    }
                    list[list.Count - 1] = default(T);
                }
            }
        }*/
    }
}
