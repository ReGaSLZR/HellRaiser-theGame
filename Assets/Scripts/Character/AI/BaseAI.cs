using Character.Movement;
using Character.Skill;
using Character.Stats;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Character.AI {

    public abstract class BaseAI : MonoBehaviour
    {

        [SerializeField]
        [Required]
        protected BaseMovement m_movement;

        [SerializeField]
        [Required]
        protected BaseStats m_characterStats;

        [SerializeField]
        protected TargetDetector m_targetDetector;

        [SerializeField]
        [Required]
        protected BaseSkill m_skillMain;

        protected virtual void OnEnable()
        {
            m_movement.SetStatMovement(m_characterStats.GetStatMovement());
            m_skillMain.SetStatOffense(m_characterStats.GetStatOffense());
        }

        protected virtual void Start() {
            InitObservers();
        }

        private void InitObservers() {
            if (m_targetDetector != null)
            {
                //upon target detection
                m_targetDetector.m_isTargetDetected
                    .Subscribe(isDetected => {
                        if (!m_characterStats.IsHurt().Value && m_skillMain.m_isExecutionFinished.Value) { 
                            OnTargetDetection(isDetected);
                        }
                    })
                    .AddTo(this);

                //on skill finish execution
                m_skillMain.m_isExecutionFinished
                    .Where(isFinished => isFinished && m_targetDetector.m_targets.Count == 0)
                    .Subscribe(_ =>
                    {
                        m_skillMain.StopSkill(false);

                        if (!m_characterStats.IsHurt().Value)
                        {
                            m_movement.SetMovementEnabled(true);
                        }

                    })
                    .AddTo(this);
            }

            //upon getting hurt
            m_characterStats.IsHurt()
                .Subscribe(isHurt => {
                    if (isHurt)
                    {
                        m_movement.StunMovement();

                        if (m_characterStats.GetHealth().Value <= 0)
                        {
                            OnDeath();
                        }
                    }
                    else
                    {
                        m_movement.UnStunMovement();

                        if (m_targetDetector != null && m_targetDetector.m_isTargetDetected.Value)
                        {
                            OnTargetDetection(true);
                        }
                        
                    }
                })
                .AddTo(this);
        }

        protected virtual void OnDeath() {
            Destroy(gameObject, m_characterStats.GetStatMovement().m_stunLength + 0.01f);
        }

        protected virtual void OnTargetDetection(bool isDetected) {
            m_movement.SetMovementEnabled(!isDetected);

            if (isDetected)
            {
                m_movement.Face(m_targetDetector.m_targets[0].gameObject.transform);
                m_skillMain.UseSkill();
            }
            else
            {
                m_skillMain.StopSkill(false);
            }
        }

    }

}