using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ADR.Core;
using ADR.Skills;
using ADR.Zones;
using ADR.UI;
using UnityEngine.InputSystem;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ADR.Enemys;

namespace ADR.Core
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        private static GameManager instance;
        public static GameManager Instance
        {
            get { return instance; }
        }
        #endregion
        public enum EntityState
        {
            Idle,
            Death
        }
        [FoldoutGroup("References"), SerializeField] private Axie _selectedAxie;
        [FoldoutGroup("References"), SerializeField] private Animator _axieAnimator;
        [FoldoutGroup("References"), SerializeField] private AxieControls _axieControls;
        [FoldoutGroup("References"), SerializeField] private List<Zone> _zones;
        [FoldoutGroup("References"), SerializeField] private List<Skill> _skillsAvalible;
        [FoldoutGroup("References"), SerializeField] private AreaResources _areaResources;
        [FoldoutGroup("References"), SerializeField] private Transform _skillAnchor;
        [FoldoutGroup("References"), SerializeField] private SkillReader _skillReaderPrefab;
        [FoldoutGroup("References"), SerializeField] private EnemySpawner _enemySpawner;
        [FoldoutGroup("References"), SerializeField] private AxieController _axieController;
        [FoldoutGroup("References"), SerializeField] private SkillResources _skillResources;
        [FoldoutGroup("References/UI"), SerializeField] private UICardsHolder _uiCardsHolder;
        [FoldoutGroup("References/UI"), SerializeField] private UIManaBar _uiManaBar;

        
        [FoldoutGroup("Settings"), SerializeField] private int _currentLife;
        [FoldoutGroup("Settings"), SerializeField] private int _currentCombo;

        [FoldoutGroup("Settings/Skills"), SerializeField,Range(1,6)] private int _maxSkillsAvalible = 1;
        [FoldoutGroup("Settings/Skills"), SerializeField] private SkillReader _selectedSkill;

        [FoldoutGroup("Settings/Energy"), SerializeField] private float _maxMana = 10;
        [FoldoutGroup("Settings/Energy"), SerializeField] private float _energyGenerationRate = 1;

        [FoldoutGroup("Settings/Energy"), SerializeField] private float _currentMana;

        [FoldoutGroup("Statistics"), SerializeField] private int _generalMultiplier = 1;
        [FoldoutGroup("Statistics"), SerializeField] private int _maxCombo = 0;
        [FoldoutGroup("Statistics"), SerializeField] private int _maxTurn = 0;
        [FoldoutGroup("Statistics"), SerializeField] private int _maxMultiKill = 0;

        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnStartGame;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnHit;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnRage;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnCombo;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnLoss;
        [FoldoutGroup("Feedbacks"), SerializeField] public MMF_Player OnStageComplete;
        #region Actions
        public event Action EnemyKilled;
        public event Action SkillTriggered;
        public event Action PlayerHit;
        public event Action PlayerLoss;
        public event Action PlayerWin;
        public event Action OnComboAdded;
        public event Action OnTurnPass;
        #endregion
        #region Getters
        public EntityState State = EntityState.Idle;
        public AreaResources AreaResources => _areaResources;
        public int CurrentCombo => _currentCombo;
        public SkillReader SelectedSkill => _selectedSkill;
        public int GetMaxSkillsAvalible => _maxSkillsAvalible;
        public List<Skill> SkillsAvalible => _skillsAvalible;
        public UICardsHolder UICardsHolder => _uiCardsHolder;
        public UIManaBar UIManaBar => _uiManaBar;
        public AxieControls AxieControls => _axieControls;
        public AxieController AxieController => _axieController;
        public SkillResources SkillResources => _skillResources;
        public EnemySpawner EnemySpawner => _enemySpawner;
        public int MaxLife => _selectedAxie.BaseLife;
        public int CurrentLife => _currentLife;
        public int TurnCount = 0;

        public float MaxMana => _maxMana;
        public float CurrentMana => _currentMana;
        public bool AbleToTriggerSkill = true;

        public int RebornCount => _generalMultiplier;
        public int MaxCombo => _maxCombo;
        public int MaxTurn => _maxTurn;
        #endregion
        public void SetUp()
        {
            _currentLife = _selectedAxie.BaseLife;
            _currentCombo = 0;
            _skillsAvalible = _selectedAxie.GetAxieSkills();
        }
        private void Awake()
        {
            #region Singleton Implementation
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
            #endregion
            _axieControls = new AxieControls();
            GeneralLoadAndSet();
            SetUp();
        }
        void Start()
        {
            #region Inputs
            _axieControls.Enable();
            _axieControls.GameInputs.Action.performed += TriggerSkill;
            _axieControls.GameInputs.Cancel.performed += CancelSkill;
            #endregion
            #region Events
            OnComboAdded += TriggerUlti;
            PlayerLoss += RemoveInpunts;
            PlayerWin += SaveMaxStage;

            #endregion
            EnergySystemSetUp();
            _enemySpawner.OnSpawnEnemys.PlayFeedbacks();
            OnStartGame.PlayFeedbacks();
        }
        #region SaveLoad
        public void GeneralLoadAndSet()
        {
            //print("TRY TO LOAD");
            if (PlayerPrefs.HasKey("MaximunStage"))
            {
                _generalMultiplier = PlayerPrefs.GetInt("MaximunStage");
                //print("KeyFound"+ PlayerPrefs.GetInt("MaximunStage"));
            }
            else
                PlayerPrefs.SetInt("MaximunStage", _generalMultiplier);

            if (PlayerPrefs.HasKey("MaxCombo"))
            {
                _maxCombo = PlayerPrefs.GetInt("MaxCombo");
                //print("KeyFound"+ PlayerPrefs.GetInt("MaxCombo"));
            }
            else
                PlayerPrefs.SetInt("MaxCombo", _maxCombo);

            if (PlayerPrefs.HasKey("MaxTurn"))
            {
                _maxTurn = PlayerPrefs.GetInt("MaxTurn");
                //print("KeyFound"+ PlayerPrefs.GetInt("MaxTurn"));
            }
            else
                PlayerPrefs.SetInt("MaxTurn", _maxTurn);

        }
        public void SaveMaxStage()
        {
            _generalMultiplier++;
            PlayerPrefs.SetInt("MaximunStage", _generalMultiplier);
        }
        public void SaveMaxCombo()
        {
            PlayerPrefs.SetInt("MaxCombo", _maxCombo);
        }
        public void SaveMaxTurn()
        {
            PlayerPrefs.SetInt("MaxTurn", _maxTurn);
        }
        #endregion
        private void RemoveInpunts()
        {
            _axieControls.Disable();
            _axieControls.GameInputs.Action.performed -= TriggerSkill;
            _axieControls.GameInputs.Cancel.performed -= CancelSkill;
        }
        private void OnDestroy()
        {
            RemoveInpunts();
        }
        private void OnDisable()
        {
            _axieControls.Disable();
        }
        void FixedUpdate()
        {
            GenerateEnergy();
        }
        #region Behavior
        private void EnergySystemSetUp()
        {
            // Calcula la tasa de generación de energía y el tiempo entre generaciones
            _currentMana = 0;

            _uiManaBar.UpdateEnergyBar();
        }
        private void GenerateEnergy()
        {
            if (_currentMana < _maxMana)
            {
                _currentMana += Time.deltaTime * _energyGenerationRate;
                _uiManaBar.UpdateEnergyBar();
            }
            
        }
        #endregion
        #region Events
       
        public void KilledEnemy()
        {
            EnemyKilled?.Invoke();//->este evento
        }
        public void SkillCardUsed()
        {
            AddTurn();
            if (_maxTurn < TurnCount)
            {
                _maxTurn = TurnCount;
                print("MAX TURN" + _maxTurn);
                SaveMaxTurn();
            }
            SkillTriggered?.Invoke();
        }
        public void AddTurn()
        {
            TurnCount++;
            OnTurnPass?.Invoke();
        }
        #endregion
        #region Utilities
        public int playerValue()
        {
            return _selectedAxie.AverageDamage() + _currentCombo;
        }
        public bool HasMana(Skill skill)
        {
            if (CurrentMana >= skill.ManaCost)
                return true;
            return false;
        }
        public void SubstractMana(Skill skill)
        {
            _currentMana -= skill.ManaCost;
        }
        public bool IsMouseOverUI()
        {
            if (EventSystem.current != null)
            {
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                eventData.position = Input.mousePosition;

                var results = new System.Collections.Generic.List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);
                foreach (var result in results)
                {
                    if (result.gameObject.GetComponent<Selectable>() != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void AddCombo()
        {
            _currentCombo++;
            if(_maxCombo < _currentCombo)
            {
                _maxCombo = _currentCombo;
                SaveMaxCombo();
            }
            OnComboAdded?.Invoke();
        }
        public void SetSkillSlot(Skill skill,UISkillCard skillCard)
        {
            if (_selectedSkill == null || _selectedSkill.Skill != skill)
            {
                RemoveSkillSlot();
                SkillReader skillReader = Instantiate(_skillReaderPrefab, _skillAnchor);
                skillReader.SetUp(skill,skillCard);

                _selectedSkill = skillReader;
            }
        }
        public void RemoveSkillSlot()
        {
            if (_selectedSkill != null)
            {
                _selectedSkill.UnselectEnemys();
                Destroy(_selectedSkill.gameObject);
            }
            _selectedSkill = null;
        }
        public void ResetCombo()
        {
            _currentCombo = 0;
        }
        public void GetHit(int damage)
        {
            if (State == EntityState.Death) return;
            _currentLife -= damage;
            PlayerHit.Invoke();
            if (_currentLife <= 0)
            {
                OnLoss.PlayFeedbacks();
                _currentLife = 0;
                //Time.timeScale = 0;
                State = EntityState.Death;
                PlayerLoss?.Invoke();
            }
            else
            {
                OnHit.PlayFeedbacks();
            }
            
        }
        [Button]
        public void OnCompleteStage()
        {
            OnStageComplete.PlayFeedbacks();
            PlayerWin?.Invoke();
        }
        public void SetPlayerAnimations(string name)
        {
            _axieAnimator.Play(name);
        }
        public void TriggerUlti()
        {
            if (_currentCombo % 10 == 0)
                _uiCardsHolder.UpgradeRandomCard();
        }
        #endregion
        #region Inputs

        public void TriggerSkill(InputAction.CallbackContext context)
        {
            if (AbleToTriggerSkill == false)
                return;
            if (IsMouseOverUI())
            {
                print("OVER UI");
                return;
            }
            if (_selectedSkill != null)
            {
                _selectedSkill.TriggerSkill();
                AbleToTriggerSkill = false;
            }

        }
        public void CancelSkill(InputAction.CallbackContext context)
        {
            if (_selectedSkill != null)
            {
                RemoveSkillSlot();

                print("RemoveSkill");
            }

        }
        #endregion
        [Button]
        public void StartGame()
        {
            EnergySystemSetUp();
            _enemySpawner.OnSpawnEnemys.PlayFeedbacks();
        }
        [Button]
        public void ResetPlayerPrefabs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}




