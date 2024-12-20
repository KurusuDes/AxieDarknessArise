using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ADR.Skills;
using ADR.Core;
using ADR.Enemys;
using MoreMountains.Feedbacks;
using ADR.UI;
using DG.Tweening;

namespace ADR.Skills
{
    public enum State
    {
        Unselected,
        Selected,
        Trigger
    }
    public class SkillReader : MonoBehaviour
    {

        [FoldoutGroup("References"), SerializeField] private Skill _skill;
        //[FoldoutGroup("References"), SerializeField] public GameObject _selected;
        [FoldoutGroup("References"), SerializeField] private Transform _areaAnchor;
        [FoldoutGroup("References"), SerializeField] private AreaTrigger _areaTrigger;

        [FoldoutGroup("Settings"), SerializeField] private State _state;
        [FoldoutGroup("Settings"), SerializeField] private UISkillCard _skillCard;
        [FoldoutGroup("Settings"), SerializeField] private Vector3 _skillVFXOffset;
        [FoldoutGroup("Settings"), SerializeField] private float _vfxScale;
        [FoldoutGroup("Settings"), SerializeField] private float _customvfxScale;
        [FoldoutGroup("Settings"), SerializeField] private bool overrideVFXrotation = false;

        [FoldoutGroup("Feedbacks"), SerializeField] private MMF_Player _onTriggerAttack;
        [FoldoutGroup("Feedbacks"), SerializeField] private MMF_Player _onDeselect;

        #region Getters
        public Skill Skill => _skill;
        #endregion
        public void SetUp(Skill skill, UISkillCard skillCard)
        {
            _skill = skill;
            _skillCard = skillCard;
            overrideVFXrotation = skill.overrideVFXrotation;
            StartComponents();
        }
        void Start()
        {
            

        }
        [Button]
        private void StartComponents()
        {
            if (GameManager.Instance.AreaResources.AreaTypes.TryGetValue(_skill.AreaType, out AreaTrigger area))
            {
                AreaTrigger areaTrigger = Instantiate(area, _areaAnchor);
                _areaTrigger = areaTrigger;
            }
        }
        void Update()
        {
            
        }
        private void UpdateState()
        {
        }
        #region Utilities
        public void TriggerSkill()
        {
            if (_areaTrigger != null && _skillCard != null) 
            {
                foreach (Enemy enemy in _areaTrigger.GetTargets())
                {
                    int comboDamage = GameManager.Instance.CurrentCombo;
                    enemy.GetHit(_skill.Damage, comboDamage);
                }
                foreach (MMF_Player platform in _areaTrigger.GetPlatforms())
                {
                    platform.PlayFeedbacks();
                }
                _onTriggerAttack.PlayFeedbacks();
                InstantiateVFXs(_skill.SequentialVFXs,_skillVFXOffset, _customvfxScale, false);
                InstantiateVFXs(_skill.RandomVFXs, _skillVFXOffset, _vfxScale, true);
                _skillCard.OnSkillTrigger();
               
                GameManager.Instance.SubstractMana(_skill);


                //_skill.SequentialVFXs

            }
        }
        public void SetProyectile()
        {
            GameObject particles = Instantiate(_skill.MainProyectile, transform.position, transform.rotation).gameObject;

        }
        public void InstantiateVFXs(List<string> psList,Vector3 offset,float scale,bool random)
        {
            if(random == false)
            {
                foreach (string vfx in psList)
                {
                    if (overrideVFXrotation == true)
                    {
                        Instantiate(GameManager.Instance.SkillResources.TryToGetParticleSystem(vfx));
                    }
                    else
                    {
                        GameObject particles = Instantiate(GameManager.Instance.SkillResources.TryToGetParticleSystem(vfx), transform.position, transform.rotation).gameObject;
                        particles.transform.localScale = Vector3.one * scale;
                        particles.transform.position = Vector3.zero + offset;
                    }
                        
                }
            }
            else
            {
                GameObject particles = Instantiate(GameManager.Instance.SkillResources.TryToGetParticleSystem(psList[Random.Range(0, psList.Count)]), transform.position, transform.rotation).gameObject;
                particles.transform.localScale = Vector3.one * scale;
                particles.transform.position = Vector3.zero + offset;
            }
            
            
        }
        [Button]
        public void InstantiateTest(string vfx)
        {
            Instantiate(GameManager.Instance.SkillResources.TryToGetParticleSystem(vfx).gameObject);
            print("Skill is  going tru");
        }
        public void SetPlayerAnimations(string name) => GameManager.Instance.SetPlayerAnimations(name);
        public void SetSkillState(State state)
        {
            _state = state;
            UpdateState();
        }
        public void UnselectEnemys()
        {
            foreach (Enemy enemy in _areaTrigger.GetTargets())
            {
                enemy.OnSelected(false);
            }
        }
        #endregion

    }
}
