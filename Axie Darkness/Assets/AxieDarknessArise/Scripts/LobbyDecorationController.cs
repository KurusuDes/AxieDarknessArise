using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
public class LobbyDecorationController : MonoBehaviour
{
    [FoldoutGroup("References"), SerializeField] private List<MMF_Player> Decorations;
    [FoldoutGroup("References"), SerializeField] private MMF_Player OnStart;

    [FoldoutGroup("Settings"), SerializeField] private int _numberToTrigger;
    [FoldoutGroup("Settings"), SerializeField] private float _interval;
    void Start()
    {
        OnStart.PlayFeedbacks();
    }
    public void TriggerDecotarions()
    {
        for (int i = 0; i < _numberToTrigger; i++)
        {
            Decorations[Random.Range(0, Decorations.Count)].PlayFeedbacks();
        }
        StartCoroutine(Cooldown(_interval));
    }
    private IEnumerator Cooldown(float interval)
    {
        yield return new WaitForSeconds(interval);
        TriggerDecotarions();
    }
}

