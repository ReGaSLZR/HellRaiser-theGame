using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;

namespace Character.Movement {

    public class FollowMovement : BaseMovement
    {

        [Header("----- Child variables -----")]

        [SerializeField]
        [Range(0.1f, 10f)]
        private float m_followDistance;

        [SerializeField]
        private bool m_shouldFollowUntilDeath;

        private Transform m_followTarget;

        protected override void OnEnable()
        {
            base.OnEnable();

            this.FixedUpdateAsObservable()
                .Where(_ => m_reactiveIsMovEnabled.Value && (m_followTarget != null))
                .Select(_ => Vector2.Distance(m_followTarget.position, gameObject.transform.position))
                .Where(distance => (distance >= m_followDistance))
                .Subscribe(distance =>
                {
                    //reference for getting the normalized Vector direction from 2 positions:
                    //https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
                    StartMovement((m_followTarget.position - gameObject.transform.position).normalized);
                    m_compSpriteRenderer.flipX = ShouldFlip(m_followTarget.transform);
                })
                .AddTo(m_disposables);

        }

        public void SetFollowTarget(Transform newTarget) {
            if ((m_shouldFollowUntilDeath && (m_followTarget == null) || !m_shouldFollowUntilDeath)) {
                m_followTarget = newTarget;
                return;
            }

            LogUtil.PrintInfo(gameObject, GetType(), "SetFollowTarget(): " +
                "Still following current target. Not renewing target.");
        }

    }

}