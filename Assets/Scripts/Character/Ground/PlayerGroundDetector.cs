﻿using NaughtyAttributes;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Character.Ground {

    [RequireComponent(typeof(Collider2D))]
    public class PlayerGroundDetector : MonoBehaviour
    {

        [Inject]
        private PlayerGroundModel.Setter m_modelGround;

        [SerializeField]
        private PlayerGround m_groundType;

        [SerializeField]
        [BoxGroup("TAGS")]
        [Tag]
        private string m_groundTag1;

        [SerializeField]
        [BoxGroup("TAGS")]
        [Tag]
        private string m_groundTag2;

        private void Awake()
        {
            if (m_groundTag2 == null || m_groundTag2.Equals(""))
            {
                m_groundTag2 = "UNSET";
            }
        }

        private void Start()
        {
            this.OnTriggerEnter2DAsObservable()
                .Where(otherCollider2D => IsGroundTagMet(otherCollider2D))
                .Subscribe(_ => m_modelGround.SetGround(m_groundType, true))
                .AddTo(this);

            this.OnTriggerExit2DAsObservable()
                .Where(otherCollider2D => IsGroundTagMet(otherCollider2D))
                .Subscribe(_ => m_modelGround.SetGround(m_groundType, false))
                .AddTo(this);
        }

        private bool IsGroundTagMet(Collider2D otherCollider2D)
        {
            return (m_groundTag1.Equals(otherCollider2D.tag) || m_groundTag2.Equals(otherCollider2D.tag));
        }

    }

}