using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using Doozy.Runtime.UIManager.Animators;
using Doozy.Runtime.UIManager.Components;
using ADR.Core;
using DG.Tweening;

public class UIPassTurn : MonoBehaviour
{
    [FoldoutGroup("References"), SerializeField] private UISlider _remaingTimeSlider;

    [FoldoutGroup("Settings"), SerializeField] private float _maxValue = 60;
    [FoldoutGroup("Settings"), SerializeField] private float _fillTime = 1;
    [FoldoutGroup("Settings"), SerializeField] private float _baseDecreaseSpeed = 1;

    private IEnumerator _fillCoroutine = null;
    void Start()
    {
        _remaingTimeSlider.maxValue = _maxValue;
        _remaingTimeSlider.value = _maxValue;

        FillSlider();
        GameManager.Instance.SkillTriggered += FillSlider;

    }
    private void FillSlider()
    {
        DOTween.To(() => _remaingTimeSlider.value, x => _remaingTimeSlider.value = x, _remaingTimeSlider.maxValue, _fillTime)
               .SetEase(Ease.Linear)
               .OnComplete(() =>
               {
                   if (_fillCoroutine == null)
                   {
                       _fillCoroutine = DepletePassTurnBar();
                       StartCoroutine(_fillCoroutine);
                   }
               }
           );
    }
    private IEnumerator DepletePassTurnBar()
    {
        while (_remaingTimeSlider.value > 0)
        {
            _remaingTimeSlider.value -= Time.deltaTime * _baseDecreaseSpeed;
            yield return null;
        }

        GameManager.Instance.AddTurn();
        GameManager.Instance.EnemySpawner.EnemySpawnerMechanism();
        print("Depleed");
        FillSlider();
        _fillCoroutine = null;
    }
}
