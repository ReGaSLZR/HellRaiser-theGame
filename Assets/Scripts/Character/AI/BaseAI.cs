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

        protected virtual void Awake()
        {
            m_movement.SetMovementSpeed(m_characterStats.GetMovementSpeed());
        }

        protected virtual void Start() {
            InitObservers();
        }

        private void InitObservers() {
            //target detection
            if (m_targetDetector != null)
            {
                m_targetDetector.m_isTargetDetected
                    .Subscribe(isDetected => OnTargetDetection(isDetected))
                    .AddTo(this);
            }

            //is hurt
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
                    }
                })
                .AddTo(this);
        }

        protected virtual void OnDeath() {
            Destroy(gameObject, m_characterStats.GetStunLength() + 0.1f);
        }

        protected virtual void OnTargetDetection(bool isDetected) {
            m_movement.SetMovementEnabled(!isDetected);
            
            if (isDetected)
            {
                m_skillMain.UseSkill();
                m_movement.Face(m_targetDetector.m_targets[0].gameObject.transform);
            }
            else {
                m_skillMain.StopSkill(false);
            }
            
        }

    }

}