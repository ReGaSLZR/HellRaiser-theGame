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
            m_characterStats.GetHealth()
                .Where(health => (health <= 0))
                .Subscribe(_ => {
                    OnDeath();
                })
                .AddTo(this);

            m_characterStats.IsHurt()
                .Subscribe(isHurt => {
                    m_movement.SetMovementEnabled(!isHurt);
                })
                .AddTo(this);
        }

        protected virtual void OnDeath() {
            Destroy(gameObject);
        }

    }

}