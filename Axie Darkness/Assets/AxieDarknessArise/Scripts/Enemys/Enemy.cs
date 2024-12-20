using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ADR.Core;
using DamageNumbersPro;
using Spine.Unity;
using DG.Tweening;
using MoreMountains.Feedbacks;
using ADR.Zones;
using System;
using Spine;

namespace ADR.Enemys
{
    public class Enemy : MonoBehaviour
    {
        public enum State
        {
            Idle,
            Movement,
            Death
        }
        public enum Range
        {
            Zone1,
            Zone2,
            Zone3,

        }
        public enum Type
        {
            Normal,
            Special
        }

        #region Setters
        [FoldoutGroup("Entity"), HideLabel, SerializeField] public Entity Entity = new();

        [FoldoutGroup("References"), SerializeField] private DamageNumber _damagePopUp;
        [FoldoutGroup("References"), SerializeField] private DamageNumber _comboDamagePopUp;
        [FoldoutGroup("References"), SerializeField] private SkeletonAnimation _animator;
        [FoldoutGroup("References"), SerializeField] private GameObject select;

        [FoldoutGroup("Settings"), SerializeField] private int _maxLife;
        [FoldoutGroup("Settings"), SerializeField] private int _damage;
        [FoldoutGroup("Settings"), SerializeField] private State _state;
        [FoldoutGroup("Settings"), SerializeField] private Type _type = Type.Normal;
        [FoldoutGroup("Settings"), SerializeField] private Range _attackRange =Range.Zone1;
        [FoldoutGroup("Settings"), SerializeField] public Ease MovementEase = Ease.Linear;
        [FoldoutGroup("Settings"), SerializeField] private float _movementTime;
        [FoldoutGroup("Settings"), SerializeField,Range(0,1)] private float _chanceToMove;

        [FoldoutGroup("Debug"), SerializeField] private float _moveAmount;
        [FoldoutGroup("Debug"), SerializeField] private Range _currentRange = Range.Zone3;
        [FoldoutGroup("Debug"), SerializeField] private int _currentLife;

        [FoldoutGroup("Feedbacks"), SerializeField] private MMF_Player OnSelect;

        [FoldoutGroup("Feedbacks"), SerializeField] private MMF_Player OnStart;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnAttack;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnHit;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnDead;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnMovement;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnAlert;

        #endregion
        #region Getters
        public int Value => _maxLife + _damage;
        public State CurrentState => _state;
        public float MovementTime => _movementTime;
        public Type EnemyType => _type;

        public int CurrentLife
        {
            get { return _currentLife; }
            set { _currentLife = value; }
        }
        #endregion
        public virtual void SetUp(int playerValue =0,int multiplier = 1)
        {
            _currentLife = _maxLife;
            _state = State.Idle;
            OnStart.PlayFeedbacks();
            SetInitialOrientation();

            _currentLife *= multiplier;
            _damage *= multiplier;
        }
        void Start()
        {
            float distance = Vector3.Distance(transform.position, GameManager.Instance.AxieController.gameObject.transform.position);
            int numberOfStates = Enum.GetValues(typeof(Range)).Length;
            _moveAmount = distance / numberOfStates+1;
            //SetUp();
        }
        void Update()
        {
            FaceTheCamera(_animator.transform);
        }
        public void TriggerContinousAnimation(string animationName)
        {
            _animator.AnimationState.SetAnimation(0, animationName, false);
            _animator.AnimationState.AddAnimation(0, "action/idle/normal", true,0);
        }
        public void TriggerAnimation(string animationName)
        {
            _animator.AnimationState.SetAnimation(0, animationName, false);
        }
        #region Utilities
        [Button]
        public virtual void TriggerAction()
        {
            if (_state != State.Idle) return;

            if(_attackRange != _currentRange)
            {
                if(UnityEngine.Random.value < _chanceToMove)
                {
                    Movement(GameManager.Instance.AxieController.gameObject.transform, _moveAmount);
                    DecreaseRange();
                }
                
            }
            else
            {
                Attack();
            }
            
            
        }
        public virtual void Movement(Transform target = null,float amount = 0)
        {
            OnMovement.PlayFeedbacks();
            Vector3 direction = target.position - transform.position;
            direction.y = 0f;

            direction.Normalize();
            Vector3 targetPosition = transform.position + direction * amount;
            transform.DOMove(targetPosition, _movementTime).SetEase(MovementEase);
        }
        public virtual void DecreaseRange()
        {
            int currentRangeValue = (int)_currentRange;
            currentRangeValue--;

            // Ensure the resulting value is within the enum range
            if (currentRangeValue < (int)Range.Zone1)
            {
                currentRangeValue = (int)Range.Zone1;
            }

            _currentRange = (Range)currentRangeValue;
            if(_attackRange == _currentRange)
            {
                OnAlert.PlayFeedbacks();
            }
        }
        private void SetInitialOrientation()
        {
            if(transform.position.x <0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        public virtual void Attack()
        {
            OnAttack.PlayFeedbacks();
            GameManager.Instance.GetHit(_damage);
        }
        [Button]
        public void GetHit(int damage , int comboDamage)
        {
            if (_state == State.Death) 
            { 
                return; 
            }
            _currentLife -= (damage + comboDamage);

            _damagePopUp.Spawn(transform.position + Vector3.left, damage);
            if (comboDamage > 0)
                _comboDamagePopUp.Spawn(transform.position + Vector3.right, comboDamage);

            if (_currentLife <= 0)
            {
                SetDestruction();
            }
            else
                OnHit.PlayFeedbacks();
        }
        public void SetDestruction()
        {
            _state = State.Death;
            GameManager.Instance.KilledEnemy();
            OnDead.PlayFeedbacks();
            OnSelect.StopFeedbacks();
            _currentLife = 0;
        }
        [Button]
        public void OnSelected(bool selected)
        {
            if (_state == State.Death) return;
            if (selected)
                OnSelect.PlayFeedbacks();
            else
                OnSelect.StopFeedbacks();
        }
        private void FaceTheCamera(Transform target)
        {
            target.transform.rotation = Camera.main.transform.rotation;
        }
        public void SetState(State state)
        {
            _state = state;
        }
        #endregion
    }
}
