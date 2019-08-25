using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Character.Movement {

    public class FollowMovement : BaseMovement
    {

        [Space]

        [SerializeField]
        private Transform m_followTarget;

        [SerializeField]
        [Range(0.1f, 10f)]
        private float m_followDistance;

        private void OnEnable()
        {
            this.FixedUpdateAsObservable()
                .Where(_ => m_reactiveIsMovEnabled.Value)
                .Select(_ => Vector2.Distance(m_followTarget.position, gameObject.transform.position))
                .Where(distance => (distance >= m_followDistance))
                .Subscribe(distance =>
                {
                    //reference for getting the normalized Vector direction from 2 positions:
                    //https://docs.unity3d.com/Manual/DirectionDistanceFromOneObjectToAnother.html
                    StartMovement((m_followTarget.position - gameObject.transform.position).normalized);
                })
                .AddTo(m_disposables);

        }

        public void SetFollowTarget(Transform newTarget) {
            m_followTarget = newTarget;
        }

    }

}