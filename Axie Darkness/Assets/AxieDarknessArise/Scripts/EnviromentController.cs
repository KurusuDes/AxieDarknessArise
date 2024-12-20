using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ADR.Core;
using MoreMountains.Feedbacks;

public class EnviromentController : MonoBehaviour
{
    [FoldoutGroup("Enviroment"),SerializeField]List<MMF_Player> EnviromentFeedbacks = new();
    void Start()
    {
        GameManager.Instance.SkillTriggered += TriggerEnviromentFeedbacks;
    }
    [Button]
    private void TriggerEnviromentFeedbacks()
    {
        foreach (MMF_Player feedback in EnviromentFeedbacks)
        {
            feedback.PlayFeedbacks();
        }
    }
}
