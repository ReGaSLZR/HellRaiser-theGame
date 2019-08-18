﻿using UnityEngine;
using NaughtyAttributes;
using Injection;
using Zenject;
using Common;

namespace Character.Skill {

    [RequireComponent(typeof(SpriteRenderer))]
    public class ProjectileThrowSkill : BaseSkill
    {

        //COMPONENTS
        private SpriteRenderer m_compSpriteRenderer;

        //INJECTIBLES
        [Inject]
        private Instantiator m_instantiator;

        [SerializeField]
        [Required]
        private GameObject m_prefabProjectile;
        [SerializeField]
        private float m_throwForce = 1f;
        [SerializeField]
        private float m_throwTorque = 1f;

        [Space]

        [SerializeField]
        private bool m_hasSingleSpawnPoint = true;

        [SerializeField]
        [DisableIf("m_hasSingleSpawnPoint")]
        private SpawnerDirection m_spawnPointMinus;
        [SerializeField]
        [Required]
        private SpawnerDirection m_spawnPointPlus;

        protected override void Awake()
        {
            base.Awake();
            m_compSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void ExecuteUseSkill()
        {
            SpawnerDirection spawner = GetProjectileSpawner();
            GameObject projectile = m_instantiator.InstantiateInjectPrefab(m_prefabProjectile, 
                spawner.gameObject.transform);
            Rigidbody2D rigidBody2DProjectile = projectile.GetComponent<Rigidbody2D>();

            rigidBody2DProjectile.AddForce(spawner.m_direction * m_throwForce);
            rigidBody2DProjectile.AddTorque(m_throwTorque, ForceMode2D.Force);
        }

        private SpawnerDirection GetProjectileSpawner()
        {
            if (m_hasSingleSpawnPoint) {
                return m_spawnPointPlus;
            }

            return (m_compSpriteRenderer.flipX) ? 
                m_spawnPointMinus : m_spawnPointPlus;
        }

    }

}