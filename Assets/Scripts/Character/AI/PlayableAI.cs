using Character.Skill;
using Cinemachine;
using GamePlay.Input;
using GamePlay.Stats;
using GamePlay.Camera;
using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;
using Zenject;

namespace Character.AI {

    public class PlayableAI : BaseAI
    {

        [Inject]
        private BaseInputModel m_modelInput;
        [Inject]
        private readonly GamePlayStatsModel.Setter m_modelStatsSetter;
        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStatsGetter;
        [Inject]
        private readonly CameraFXModel.Setter m_cameraFX;

        [Header("----- Child variables -----")]

        [SerializeField]
        [Required]
        private BaseSkill m_skillSecondary;

        [SerializeField]
        [Required]
        private BaseSkill m_skillTertiary;

        [Space]

        [SerializeField]
        [Tooltip("Must be unique. Each PlayableAI must have their OWN virtual camera.")]
        [Required]
        private CinemachineVirtualCamera m_uniqueCamera;

        public bool m_isNotOnCycle = false; //determines if the PlayableAI is to be ignored in Character switching

        protected override void Awake()
        {
            base.Awake();

            m_skillSecondary.SetStatOffense(m_stats.GetStatOffense());
            m_skillTertiary.SetStatOffense(m_stats.GetStatOffense());

            if (m_targetDetector != null) {
                LogUtil.PrintWarning(gameObject, GetType(), "Awake(): TargetDetector is not needed in PlayerAI.");
                m_targetDetector = null;
            }

            m_uniqueCamera.m_Follow = gameObject.transform;

            m_skillMain.SetSkillDuration(m_stats.GetCharacterSkills()[0].m_duration);
            m_skillSecondary.SetSkillDuration(m_stats.GetCharacterSkills()[1].m_duration);
            m_skillTertiary.SetSkillDuration(m_stats.GetCharacterSkills()[2].m_duration);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            InitInputObservers();
            m_uniqueCamera.gameObject.SetActive(true);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            //safe check; for some reason, Unity loses the reference to the camera and prooduces a null exception
            //TODO investigate this bug/error log deeper
            if (m_uniqueCamera != null) { 
                m_uniqueCamera.gameObject.SetActive(false);
            }
        }

        private void InitInputObservers() {
            //skill MAIN
            this.UpdateAsObservable()
                .Select(_ => m_modelInput.m_skillMain)
                .Where(isUsingSkill => isUsingSkill && 
                    IsStaminaValueEnough(m_stats.GetCharacterSkills()[0].m_cost) &&
                    m_skillMain.IsExecutionFinished().Value)
                .Subscribe(_ =>
                {
                    m_skillMain.UseSkill();
                    m_cameraFX.ShowFX(m_stats.GetCharacterSkills()[0].m_cameraFXType, m_uniqueCamera);
                    UpdateStamina(m_stats.GetCharacterSkills()[0].m_cost);
                })
                .AddTo(m_disposables);

            //skill 2
            this.UpdateAsObservable()
               .Select(_ => m_modelInput.m_skill2)
               .Where(isUsingSkill => isUsingSkill && 
                    IsStaminaValueEnough(m_stats.GetCharacterSkills()[1].m_cost) &&
                    m_skillSecondary.IsExecutionFinished().Value)
               .Subscribe(_ =>
               {
                   m_skillSecondary.UseSkill();
                   m_cameraFX.ShowFX(m_stats.GetCharacterSkills()[1].m_cameraFXType, m_uniqueCamera);
                   UpdateStamina(m_stats.GetCharacterSkills()[1].m_cost);
               })
               .AddTo(m_disposables);

            //skill 3
            this.UpdateAsObservable()
               .Select(_ => m_modelInput.m_skill3)
               .Where(isUsingSkill => isUsingSkill && 
                    IsStaminaValueEnough(m_stats.GetCharacterSkills()[2].m_cost) &&
                    m_skillTertiary.IsExecutionFinished().Value)
               .Subscribe(isUsingSkill =>
                {
                    m_skillTertiary.UseSkill();
                    m_cameraFX.ShowFX(m_stats.GetCharacterSkills()[2].m_cameraFXType, m_uniqueCamera);
                    UpdateStamina(m_stats.GetCharacterSkills()[2].m_cost);
                })
                .AddTo(m_disposables);
        }

        private bool IsStaminaValueEnough(int decrement) {
            return (m_modelStatsGetter.GetActiveCharacterStamina().Value >= decrement);
        }

        private void UpdateStamina(int decrement) {
            m_modelStatsSetter.UpdateCharacterStamina(m_stats.GetCharacterName(),
                            m_modelStatsGetter.GetActiveCharacterStamina().Value - decrement);
        }

        protected override void OnDeath() {
            m_skillMain.StopSkill(false);
            m_skillSecondary.StopSkill(false);
            m_skillTertiary.StopSkill(false);

            base.OnDeath();
        }

    }

}