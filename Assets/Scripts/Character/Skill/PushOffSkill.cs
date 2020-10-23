using UnityEngine;
using Common;
using NaughtyAttributes;
using System.Collections.Generic;

namespace Character.Skill {

    public class PushOffSkill : BaseSkill
    {

        [Header("----- Child variables -----")]

        [SerializeField]
        [Required]
        private TargetDetector m_targetDetector;

        [SerializeField]
        [Required]
        private SpawnPoint m_spawnPoint;

        [SerializeField]
        [Range(1f, 1000f)]
        private float m_pushForce;

        [SerializeField]
        [Range(0f, 100f)]
        private float m_pushTorque;

        protected override void OnSkillStart()
        {
            base.OnSkillStart();

            List<Rigidbody2D> pushableTargets = GetPushableTargets();

            for (int x=0; x<pushableTargets.Count; x++) {
                pushableTargets[x].velocity = (m_spawnPoint.m_direction * m_pushForce);
            }
        }

        private List<Rigidbody2D> GetPushableTargets() {
            List<Collider2D> targetsDetected = m_targetDetector.GetTargets();
            List<Rigidbody2D> targetsRigidbody2D = new List<Rigidbody2D>();

            for (int x = 0; x < targetsDetected.Count; x++)
            {
                //safe check to handle cases where the item is destroyed when this statement fired
                if (targetsDetected[x] != null)
                {
                    Rigidbody2D targetRigidbody2D = targetsDetected[x].GetComponent<Rigidbody2D>();
                    if (targetRigidbody2D != null)
                    {
                        targetsRigidbody2D.Add(targetRigidbody2D);
                    }
                }
            }

            return targetsRigidbody2D;
        }

    }

}