using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Character.Movement {

    public class PatrolMovement : BaseMovement
    {

        private const string DIRECTION_HORIZONTAL = "Horizontal";
        private const string DIRECTION_VERTICAL = "Vertical";
        private string[] m_dropdownOptions = new string[] {
            DIRECTION_HORIZONTAL,
            DIRECTION_VERTICAL
        };

        [SerializeField]
        [Dropdown("m_dropdownOptions")]
        private string m_patrolDirection;

        [SerializeField]
        private bool m_shouldFlipOnHorizontalDirectionChange;

        private float m_patrolMovementWithDirection = 1f;

        private void Start()
        {
            m_ground.IsOnEdge()
                .Where(isOnEdge => isOnEdge)
                .Subscribe(isOnEdge =>
                {
                    m_patrolMovementWithDirection = (m_ground.GetEdgeSide() == Ground.GroundType.Ground_Left) ?
                        1f : -1f;
                })
                .AddTo(this);

            m_ground.IsWallHit()
                .Where(isWallHit => isWallHit)
                .Subscribe(isWallHit =>
                {
                    m_patrolMovementWithDirection = (m_ground.GetWallSide() == Ground.GroundType.Wall_Minus) ?
                                    1f : -1f;
                })
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Select(_ => m_reactiveIsMovEnabled.Value)
                .Where(isMovEnabled => isMovEnabled)
                .Subscribe(_ => {
                    Vector2 movement = Vector2.zero;

                    if (m_patrolDirection.Equals(DIRECTION_HORIZONTAL))
                    {
                        movement.x = m_patrolMovementWithDirection;
                        m_compSpriteRenderer.flipX = m_shouldFlipOnHorizontalDirectionChange 
                            ? (m_patrolMovementWithDirection < 0f) : false;
                    }
                    else
                    {
                        movement.y = m_patrolMovementWithDirection;
                    }

                    StartMovement(movement);
                                        
                })
                .AddTo(this);

        }

    }

}