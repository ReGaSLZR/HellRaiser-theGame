using Character.Movement;
using Character.Stats;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Character.AI {

    public abstract class BaseCharacterAI : MonoBehaviour
    {

        [SerializeField]
        [Required]
        private BaseCharacterMovement m_movement;

        [SerializeField]
        [Required]
        private BaseCharacterStats m_characterStats;
        
        private void Awake()
        {
            m_movement.SetMovementSpeed(m_characterStats.GetMovementSpeed());
        }

        protected virtual void Start() {
            m_characterStats.IsHurt()
                .Subscribe(isHurt => {
                    if (isHurt)
                    {
                        m_movement.StunMovement();

                        if (m_characterStats.GetHealth().Value <= 0) {
                            OnDeath();
                        }
                    }
                    else
                    {
                        m_movement.SetMovementEnabled(true);
                    }
                })
                .AddTo(this);
        }

        protected virtual void OnDeath() {
            Destroy(gameObject, m_characterStats.GetStunLength() + 0.1f);
        }

    }

}