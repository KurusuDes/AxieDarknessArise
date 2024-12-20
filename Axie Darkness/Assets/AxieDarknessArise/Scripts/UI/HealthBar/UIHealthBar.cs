using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ADR.Core;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Components;

public class UIHealthBar : MonoBehaviour
{
    [FoldoutGroup("References"),SerializeField] private UISlider _slider;
    void Start()
    {
        _slider.maxValue = GameManager.Instance.MaxLife;
        _slider.value = GameManager.Instance.CurrentLife;
        //print(GameManager.Instance.CurrentLife);
        GameManager.Instance.PlayerHit += UpdateHealthBar;
    }
    private void UpdateHealthBar()
    {
        _slider.value = GameManager.Instance.CurrentLife;
       // print(GameManager.Instance.CurrentLife);
    }
}
