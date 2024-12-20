using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace ADR.Skills
{
    public enum AreaType
    {
        InnerCone,
        MidCone,
        ExternalCone,
        InnerCircle,
        MidCircle,
        ExternalCircle,

        HalfInnerCircle,
        SlimLongRange,
        SegmentLongRange,
        SegmentMediumRange,
        SegmentLongMediumRange,
        Ultimate
    }
    [CreateAssetMenu(fileName = "Areas Resources", menuName = "ADR/Resources/Areas", order = 100), InlineEditor]
    public class AreaResources : SerializedScriptableObject
    {
        [BoxGroup("Areas")]public Dictionary<AreaType, AreaTrigger> AreaTypes = new();

        
    }
}
