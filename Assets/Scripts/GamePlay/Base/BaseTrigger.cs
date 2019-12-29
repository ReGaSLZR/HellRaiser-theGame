using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;

namespace GamePlay.Base {

    /// <summary>
    /// A component triggerable only by Player-gameObject upon Collider TriggerEnter
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public abstract class BaseTrigger : MonoBehaviour
    {

        public abstract void Execute();

        [SerializeField]
        protected bool m_isTriggeredOnGamePlayStart;

        protected Collider2D m_triggerer;
        protected bool m_isTriggered;

        protected virtual void Start()
        {

            if (m_isTriggeredOnGamePlayStart) {
                LogUtil.PrintInfo(gameObject, GetType(), "Triggered on gamePlay start.");
                Execute();
            }
            else {
                this.OnTriggerEnter2DAsObservable()
                    .Where(otherCollider2D => IsTriggerable(otherCollider2D.gameObject))
                    .Subscribe(otherCollider2D =>
                    {
                        m_triggerer = otherCollider2D;
                        Execute();
                    })
                    .AddTo(this);

                this.OnCollisionEnter2DAsObservable()
                    .Where(otherCollision => IsTriggerable(otherCollision.gameObject))
                    .Subscribe(otherCollision =>
                    {
                        m_triggerer = otherCollision.collider;
                        Execute();
                    })
                    .AddTo(this);
            }

        }

        protected virtual bool IsTriggerable(GameObject collidedObject)
        {
            return (collidedObject.tag.Contains("Player")) && !m_isTriggered;
        }

    }

}