using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using ADR.Skills;
using ADR.Core;

namespace MoreMountains.Feedbacks
{
    [AddComponentMenu("")]
    [FeedbackHelp("This feedback allows you to instantiate the object specified in its inspector, at the feedback's position (plus an optional offset). You can also optionally (and automatically) create an object pool at initialization to save on performance. In that case you'll need to specify a pool size (usually the maximum amount of these instantiated objects you plan on having in your scene at each given time).")]
    [FeedbackPath("ADR/Instantiate VFX")]
    public class MMF_InstantiateVFX : MMF_Feedback
    {
        public static bool FeedbackTypeAuthorized = true;
#if UNITY_EDITOR
        public override Color FeedbackColor { get { return MMFeedbacksInspectorColors.GameObjectColor; } }
        public override bool EvaluateRequiresSetup() { return (NameOfTheVFX == null); }
        public override string RequiredTargetText { get { return NameOfTheVFX != null ? NameOfTheVFX : ""; } }
        public override string RequiresSetupText { get { return "This feedback requires that a GameObjectToInstantiate be set to be able to work properly. You can set one below."; } }
#endif
        [MMFInspectorGroup("Instantiate VFX", true, 37, true)]
        [Tooltip("the name of the VFX")]
        [FormerlySerializedAs("VfxToInstantiate")]
        public string NameOfTheVFX;

		[MMFInspectorGroup("Position", true, 39)]
		public Transform TargetTransform;
		/// the position offset at which to instantiate the object
		[Tooltip("the position offset at which to instantiate the object")]
		[FormerlySerializedAs("VfxPositionOffset")]
		public Vector3 PositionOffset;

        [FormerlySerializedAs("VfxScaleOffset")]
        public float scaleOffset;

        [MMFInspectorGroup("Parent", true, 47)]
        /// if specified, the instantiated object (or the pool of objects) will be parented to this transform 
        [Tooltip("if specified, the instantiated object (or the pool of objects) will be parented to this transform ")]
        public Transform ParentTransform;


        protected GameObject _newGameObject;
        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            if (!Active || !FeedbackTypeAuthorized || (NameOfTheVFX == ""))
            {
                return;
            }

            _newGameObject = GameObject.Instantiate(GameManager.Instance.SkillResources.TryToGetParticleSystem(NameOfTheVFX),GameManager.Instance.transform.position,GameManager.Instance.transform.rotation).gameObject;
            if (_newGameObject != null)
            {
                SceneManager.MoveGameObjectToScene(_newGameObject, Owner.gameObject.scene);
                PositionObject(position);
            }

            throw new System.NotImplementedException();
        }
        protected virtual void PositionObject(Vector3 position)
        {
            _newGameObject.transform.position = GetPosition(position);
            if ((ParentTransform != null))
            {
                _newGameObject.transform.SetParent(ParentTransform);
            }
            _newGameObject.transform.localScale = new(_newGameObject.transform.localScale.x+ scaleOffset, _newGameObject.transform.localScale.y+ scaleOffset, _newGameObject.transform.localScale.z+ scaleOffset);
        }
        protected virtual Vector3 GetPosition(Vector3 position)
        {
            return position + PositionOffset;
            
        }
    }
}
