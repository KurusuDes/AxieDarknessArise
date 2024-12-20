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
    public class Boss : Enemy
    {

        [FoldoutGroup("Boss Settings"), SerializeField] public int LifeMultiplier = 5;
        [FoldoutGroup("Boss Settings"), SerializeField] public int SpawnMultiplier = 2;
        public override void SetUp(int playerValue = 0, int multiplier = 1)
        {
            base.SetUp();
            CurrentLife += playerValue * (LifeMultiplier + GameManager.Instance.RebornCount);
            OnAlert.PlayFeedbacks();
        }
        public override void TriggerAction()//->que salte al azar a alguna de las plataformas y invoque un enemigo
        {
            //base.TriggerAction();
            if (CurrentState != State.Idle) return;

            Attack();
            Movement();
            
        }
        public override void DecreaseRange()//-su rango de ataque es siempree asi que aqui solo evitare que se mueva
        {
           // base.DecreaseRange();
        }
        public override void Attack()//->invoca un enemigo
        {
            OnAttack.PlayFeedbacks();
        }
        public override void Movement(Transform target = null, float amount = 0)//->muevete random
        {
            //base.Movement(target, amount);
            OnMovement.PlayFeedbacks();
            SetState(State.Movement);
        }
        public void StartMovement()
        {
            Transform target = GameManager.Instance.EnemySpawner.GetRandomSpawnPoint();
            transform.DOMove(target.position, MovementTime).SetEase(MovementEase).OnComplete(() => SetState(State.Idle));
        }
        public void SpawnMob()
        {
            int multiplierChance = UnityEngine.Random.Range(1, SpawnMultiplier);
            GameManager.Instance.EnemySpawner.SpawnEnemyAtPos(transform, multiplierChance);
        }
        public void BossDefeated()//->que todos los enemigos mueran
        {
            GameManager.Instance.OnCompleteStage();
        }
    }
}
