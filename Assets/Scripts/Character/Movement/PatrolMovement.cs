using Character.Ground;
using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using static UnityEngine.RectTransform;

namespace Character.Movement {

    public class PatrolMovement : BaseMovement
    {

        [Header("----- Child variables -----")]

        [SerializeField]
        [Required]
        private GroundManager m_ground;

        private float m_patrolMovementWithDirection = 1f;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_ground.IsOnEdge()
                .Where(isOnEdge => isOnEdge)
                .Subscribe(isOnEdge =>
                {
                    m_patrolMovementWithDirection = (m_ground.GetEdgeSide() == Ground.GroundType.Ground_Left) ?
                        1f : -1f;
                })
                .AddTo(m_disposables);

            m_ground.IsWallHit()
                .Where(isWallHit => isWallHit)
                .Subscribe(isWallHit =>
                {
                    m_patrolMovementWithDirection = (m_ground.GetWallSide() == Ground.GroundType.Wall_Minus) ?
                                    1f : -1f;
                })
                .AddTo(m_disposables);

            this.UpdateAsObservable()
                .Select(_ => m_reactiveIsMovEnabled.Value)
                .Where(isMovEnabled => isMovEnabled)
                .Subscribe(_ =>
                {
                    Vector2 movement = Vector2.zero;
                    bool shouldFlip = ShouldFlip(m_patrolMovementWithDirection);

                    if (Axis.Horizontal == m_movementDirection)
                    {
                        movement.x = m_patrolMovementWithDirection;
                        m_compSpriteRenderer.flipX = shouldFlip;
                    }
                    else
                    {
                        movement.y = m_patrolMovementWithDirection;
                        m_compSpriteRenderer.flipY = shouldFlip;
                    }

                    StartMovement(movement);

                })
                .AddTo(m_disposables);

        }

    }

}