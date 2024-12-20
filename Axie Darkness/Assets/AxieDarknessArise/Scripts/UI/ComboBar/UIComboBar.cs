using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using Sirenix.OdinInspector;
using ADR.Core;
using UnityEngine.UI;
using DG.Tweening;
using AssetKits.ParticleImage;
using TMPro;
using MoreMountains.Feedbacks;

namespace ADR.UI
{
    public class UIComboBar : MonoBehaviour
    {
        [FoldoutGroup("References"), SerializeField] private DamageNumber _comboNumber;
        [FoldoutGroup("References"), SerializeField] private RectTransform _comboAnchor;
        [FoldoutGroup("References"), SerializeField] private TextMeshProUGUI _comboText;
        [FoldoutGroup("References"), SerializeField] private Slider _comboSlider;

        [FoldoutGroup("Settings"), SerializeField] private float _fillTime = 1;
        [FoldoutGroup("Settings"), SerializeField] private float _baseDecreaseSpeed = 1;
        [FoldoutGroup("Settings"), SerializeField] private float _DecreaseMultiplier = 0.2f;

        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnComboAdded;

        private IEnumerator _fillCoroutine = null;
        private float depletionSpeedModified = 0;
        void Start()
        {
            GameManager.Instance.EnemyKilled += AddCombo;
            UpdateComboText();
        }
        private void OnDestroy()
        {
            GameManager.Instance.EnemyKilled -= AddCombo;
        }
        void Update()
        {

        }

        [Button]
        public void AddCombo()
        {
            GameManager.Instance.AddCombo();
            UpdateComboText();
            OnComboAdded.PlayFeedbacks();
            _comboNumber.Spawn(_comboAnchor, Vector2.zero, GameManager.Instance.CurrentCombo);

            depletionSpeedModified = _baseDecreaseSpeed + (GameManager.Instance.CurrentCombo * _DecreaseMultiplier);

            float fillTime = (_comboSlider.maxValue - _comboSlider.value) / _comboSlider.maxValue * _fillTime;

            DOTween.To(() => _comboSlider.value, x => _comboSlider.value = x, _comboSlider.maxValue, fillTime)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (_fillCoroutine == null)
                    {
                        _fillCoroutine = DepleteComboBar();
                        StartCoroutine(_fillCoroutine);
                    }
                }
            );

        }
        private IEnumerator DepleteComboBar()
        {
            while (_comboSlider.value > 0)
            {
                //print(" modifier"+ depletionSpeedModified);
                float delta = Time.deltaTime;
                _comboSlider.value -= delta * depletionSpeedModified;
                yield return null;
            }
            GameManager.Instance.ResetCombo();
            UpdateComboText();
            _fillCoroutine = null;
        }
        private void UpdateComboText()
        {
            _comboText.text = "x" + GameManager.Instance.CurrentCombo;
        }
    }
}
