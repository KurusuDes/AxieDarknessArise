using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class SelectTooltip : MonoBehaviour
{
    [FoldoutGroup("Settings"), SerializeField] private UnityEvent _onPlay;
    [FoldoutGroup("Settings"), SerializeField] private UnityEvent _onStop;
    public void OnPlayEvents()
    {
        _onPlay.Invoke();
    }
    public void OnStopEvents()
    {
        _onPlay.Invoke();
    }
}
