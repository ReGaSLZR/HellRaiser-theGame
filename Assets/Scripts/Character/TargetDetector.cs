﻿using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Character {

    [RequireComponent(typeof(Collider2D))]
    public class TargetDetector : MonoBehaviour
    {

        [SerializeField]
        [TagSelector]
        private List<string> m_targetTags;

        [Tooltip("If set to FALSE, this will capture ALL targets within its range upon detection." +
            "If set to TRUE, disregard the value of Range.")]
        [SerializeField]
        private bool m_isLockedToFirstSingleTarget;

        [SerializeField]
        [DisableIf("m_isLockedToFirstSingleTarget")]
        private float m_detectionRange = 5f;

        private ReactiveProperty<bool> m_isTargetDetected = new ReactiveProperty<bool>(false);
        private List<Collider2D> m_targets = new List<Collider2D>();

        private CompositeDisposable m_disposables = new CompositeDisposable();

        private void OnDisable()
        {
            m_disposables.Clear();
        }

        private void OnEnable()
        {
            InitControlledObservers();
        }

        private void InitControlledObservers() {
            this.OnTriggerEnter2DAsObservable()
                .Where(otherCollider2D => IsMatchingTag(otherCollider2D.tag))
                .Subscribe(otherCollider2D => CaptureTargets(otherCollider2D))
                .AddTo(m_disposables);

            this.OnTriggerExit2DAsObservable()
                .Where(otherCollider2D => IsMatchingTag(otherCollider2D.tag))
                .Subscribe(otherCollider2D => ClearTargets())
                .AddTo(m_disposables);

            this.OnCollisionEnter2DAsObservable()
                .Where(otherCollision2D => IsMatchingTag(otherCollision2D.gameObject.tag))
                .Subscribe(otherCollision2D => CaptureTargets(otherCollision2D.collider))
                .AddTo(m_disposables);

            this.OnCollisionExit2DAsObservable()
                .Where(otherCollision2D => IsMatchingTag(otherCollision2D.gameObject.tag))
                .Subscribe(otherCollider2D => ClearTargets())
                .AddTo(m_disposables);

            //self-check list of targets for null items, then reset m_isTargetDetected.Value
            Observable.Interval(System.TimeSpan.FromSeconds(1))
                .Where(_ => m_isTargetDetected.Value)
                .Subscribe(_ => CheckTargetsListForDestruction())
                .AddTo(m_disposables);
        }

        public ReactiveProperty<bool> IsTargetDetected() {
            return m_isTargetDetected;
        }

        public List<Collider2D> GetTargets() {
            return m_targets;
        }

        public void CheckTargetsListForDestruction() {
            for(int x=(m_targets.Count - 1); x>=0; x--) {
                if (m_targets[x] == null) {
                    m_targets.RemoveAt(x);
                }
            }

            if (m_targets.Count == 0) {
                m_isTargetDetected.Value = false;
            }
        }

        private void CaptureTargets(Collider2D targetCollider) {
            if (m_isTargetDetected.Value == false)
            {
                RefreshTargets(targetCollider);
                m_isTargetDetected.Value = true;
            }
        }

        private void ClearTargets() {
            m_isTargetDetected.Value = false;
            m_targets.Clear();
        }

        private void RefreshTargets(Collider2D detectedCollider)
        {
            m_targets.Clear();

            if (m_isLockedToFirstSingleTarget && IsMatchingTag(detectedCollider.tag))
            {
                m_targets.Add(detectedCollider);
            }
            else
            {
                Collider2D[] tempTargets = Physics2D.OverlapCircleAll(transform.position, m_detectionRange);

                //filter targets by tags
                foreach (Collider2D collider2D in tempTargets)
                {
                    if (IsMatchingTag(collider2D.tag))
                    {
                        m_targets.Add(collider2D);
                    }
                }
            }
        }

        private bool IsMatchingTag(string tag)
        {
            return m_targetTags.Contains(tag);
        }

    }


}