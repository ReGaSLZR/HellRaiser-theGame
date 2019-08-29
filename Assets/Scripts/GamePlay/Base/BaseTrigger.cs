﻿using UniRx;
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
                    .Where(otherCollider2D => (otherCollider2D.tag.Contains("Player")) && !m_isTriggered)
                    .Subscribe(otherCollider2D =>
                    {
                        m_triggerer = otherCollider2D;
                        Execute();
                    })
                    .AddTo(this);

                this.OnCollisionEnter2DAsObservable()
                    .Where(otherCollision => (otherCollision.gameObject.tag.Contains("Player")) && !m_isTriggered)
                    .Subscribe(otherCollision =>
                    {
                        m_triggerer = otherCollision.collider;
                        Execute();
                    })
                    .AddTo(this);
            }

        }

    }

}