using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace ADR.Skills
{
    [SerializeField]
    [CreateAssetMenu(fileName = "Skill Resources", menuName = "ADR/Resources/SkillResources", order = 100), InlineEditor]
    public class SkillResources : SerializedScriptableObject
    {
        [FoldoutGroup("Settings"), SerializeField] private Dictionary<string, ParticleSystem> SkillsVFX = new();
        [FoldoutGroup("Settings"), SerializeField] private Dictionary<SkillAreaReference, Sprite> AreaReference = new();

        public ParticleSystem TryToGetParticleSystem(string vfxName)
        {

            if (SkillsVFX.TryGetValue(vfxName, out ParticleSystem foundVFX))
            {
                return foundVFX; // VFX with vfxName found in the dictionary.
            }
            else
            {
                if (SkillsVFX.Count > 0)
                {
                    // VFX with vfxName not found; return the first ParticleSystem in the dictionary.
                    Debug.LogError("Failed to find VFX with name: " + vfxName);
                    return SkillsVFX.Values.First();
                }
                else
                {
                    // The dictionary is empty; you may want to return null or handle it differently.
                    Debug.LogError("The VFX dictionary is empty.");
                    return null;
                }
            }
        }
        public Sprite GetAreaReference(SkillAreaReference areaReference)
        {
            if (AreaReference.TryGetValue(areaReference, out Sprite sprite))
            {
                return sprite;
            }
            return null;
        }
    }
}
