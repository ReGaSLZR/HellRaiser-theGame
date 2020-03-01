using Character.Movement;
using Character.Skill;
using Character.Stats;
using GamePlay.Base;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using Utils;

namespace Character.AI {

    public class BaseAI : MonoBehaviour
    {

        [Header("----- Base variables -----")]

        [SerializeField]
        protected TargetDetector m_targetDetector;

        [Space]

        [SerializeField]
        [Required]
        protected BaseMovement m_movement;

        [SerializeField]
        [Required]
        protected BaseStats m_stats;

        [SerializeField]
        [Required]
        protected BaseSkill m_skillMain;

        [Space]

        [SerializeField]
        private BaseTrigger m_triggerOnDeath;

        protected CompositeDisposable m_disposables = new CompositeDisposable();
        protected CompositeDisposable m_disposablesUncontrolled = new CompositeDisposable();

        protected virtual void Awake()
        {
            DistributeStats();
        }

        protected virtual void OnEnable()
        {
            m_movement.enabled = true;
            m_stats.enabled = true;

            InitControlledObservers();
        }

        protected virtual void OnDisable() {
            m_movement.enabled = false;
            m_stats.enabled = false;

            m_disposables.Clear();
        }

        protected virtual void Start() {
            InitUncontrolledObservers();
        }

        protected void DistributeStats() {
            m_movement.SetStatMovement(m_stats.GetStatMovement());
            m_skillMain.SetStatOffense(m_stats.GetStatOffense());
        }

        protected void InitUncontrolledObservers() {
            m_disposablesUncontrolled.Clear();

            //upon getting hurt
            m_stats.IsHurt()
                .Subscribe(isHurt =>
                {
                    if (isHurt)
                    {
                        m_movement.StunMovement();

                        if (m_stats.GetHealth().Value <= 0)
                        {
                            OnDeath();
                        }
                    }
                    else
                    {
                        m_movement.UnStunMovement();

                        if (m_targetDetector != null && m_targetDetector.IsTargetDetected().Value)
                        {
                            OnTargetDetection(true);
                        }

                    }
                })
                .AddTo(m_disposablesUncontrolled);
        }

        protected void InitControlledObservers() {
            if (m_targetDetector != null)
            {
                //upon target detection
                m_targetDetector.IsTargetDetected()
                    .Where(isDetected => (!m_stats.IsHurt().Value && m_skillMain.IsExecutionFinished().Value))
                    .Subscribe(isDetected => {
                        OnTargetDetection(isDetected);
                    })
                    .AddTo(m_disposables);

                //on skill finish execution
                m_skillMain.IsExecutionFinished()
                    .Where(isFinished => isFinished && m_targetDetector.GetTargets().Count == 0)
                    .Subscribe(_ =>
                    {
                        m_skillMain.StopSkill(false);

                        if (!m_stats.IsHurt().Value)
                        {
                            m_movement.SetMovementEnabled(true);
                        }

                    })
                    .AddTo(m_disposables);
            }
        }

        protected virtual void OnDeath() {
            if(m_triggerOnDeath != null)
            {
                LogUtil.PrintInfo(this, GetType(), "OnDeath(): Executing trigger.");
                m_triggerOnDeath.Execute();
            }

            m_skillMain.StopSkill(false);
            m_movement.TerminateMovement();
            Destroy(gameObject, m_stats.GetStatMovement().m_deathLength);
        }

        protected virtual void OnTargetDetection(bool isDetected) {
            m_movement.SetMovementEnabled(!isDetected);

            if (isDetected && (m_targetDetector.GetTargets().Count > 0))
            {
                Collider2D firstTarget = m_targetDetector.GetTargets()[0];
                if (firstTarget != null) {
                    m_movement.Face(firstTarget.gameObject.transform);
                }
                m_skillMain.UseSkill();
            }
            else
            {
                m_skillMain.StopSkill(false);
            }
        }

    }

}