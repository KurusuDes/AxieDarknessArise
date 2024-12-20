using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector; 
using ADR.Core;

namespace ADR.Skills
{
   
    public enum SkillType
    {
        Attack,
        Skill
    }
    public enum SkillAreaReference
    {
        Small,
        Medium,
        Large,
        Ultimate
    }
    [SerializeField]
    [CreateAssetMenu(fileName = "Skill", menuName = "ADR/Resources/Skill", order = 100), InlineEditor]
    public class Skill : SerializedScriptableObject
    {
        #region Setters
        [FoldoutGroup("Entity"), SerializeField] public Entity Entity;

        [FoldoutGroup("Settings"), SerializeField] private SkillAreaReference _areaReference = SkillAreaReference.Small;
        [FoldoutGroup("Settings"), SerializeField] private SkillType _skillType;
        [FoldoutGroup("Settings"), SerializeField] private AreaType _areaType;

        [FoldoutGroup("Settings"), SerializeField,ShowIf("_skillType" , SkillType.Attack)] private int _damage;
        [FoldoutGroup("Settings"), SerializeField, ShowIf("_skillType", SkillType.Skill)] private int _shield;
        [FoldoutGroup("Settings"), SerializeField, ShowIf("_skillType", SkillType.Skill)] private int _heal;


        [FoldoutGroup("Settings"), SerializeField] private List<StatusEffect> _statusEffects;
        [Space]
        [FoldoutGroup("Settings"), SerializeField] private int _manaCost;
        [Space]
        [FoldoutGroup("Settings"), SerializeField] private Skill _skillUpgrade;

        [FoldoutGroup("Settings"), SerializeField] public bool overrideVFXrotation = false;
        [Header("VFX")]
        [FoldoutGroup("Settings/VFX"), SerializeField] private List<string> _randomVFXS = new();
        [FoldoutGroup("Settings/VFX"), SerializeField] private List<string> _squentialVFXS = new();
        [FoldoutGroup("Settings/VFX"), SerializeField] private GameObject _mainProyectile;

        
        
        #endregion
        #region Getters
        public int Damage => _damage;
        public int Shield => _shield;
        public int Heal => _heal;
        public int ManaCost => _manaCost;
        public AreaType AreaType => _areaType;
        public SkillAreaReference AreaReference => _areaReference;

        public List<string> RandomVFXs => _randomVFXS;
        public List<string> SequentialVFXs => _squentialVFXS;
        public Skill SkillUpgrade => _skillUpgrade;
        public GameObject MainProyectile => _mainProyectile;
        #endregion
    }
}


