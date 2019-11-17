using UnityEngine;
using NaughtyAttributes;
using Injection;
using Zenject;
using Common;
using Utils;

namespace Character.Skill {

    public class ProjectileThrowSkill : BaseSkill
    {

        [Header("----- Child variables -----")]

        //COMPONENTS
        [SerializeField]
        [Required]
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

        [SerializeField]
        private bool m_shouldInfluenceProjecileFlipX;

        [Space]

        [SerializeField]
        private bool m_hasSingleSpawnPoint = true;

        [SerializeField]
        [DisableIf("m_hasSingleSpawnPoint")]
        private SpawnPoint m_spawnPointMinus;
        [SerializeField]
        [Required]
        private SpawnPoint m_spawnPointPlus;

        protected override void OnSkillStart()
        {
            base.OnSkillStart();

            SpawnPoint spawner = GetProjectileSpawner();
            GameObject projectile = m_instantiator.InstantiateInjectPrefab(m_prefabProjectile, 
                spawner.gameObject.transform);

            PassStatOffenseTo(projectile);
            InFluenceProjectileFlipX(projectile);
            ApplyForceToProjectile(projectile, spawner);
        }

        private SpawnPoint GetProjectileSpawner()
        {
            if (m_hasSingleSpawnPoint) {
                return m_spawnPointPlus;
            }

            return (m_compSpriteRenderer.flipX) ? 
                m_spawnPointMinus : m_spawnPointPlus;
        }

        private void ApplyForceToProjectile(GameObject projectile, SpawnPoint spawnPoint)
        {
            Rigidbody2D rigidBody2DProjectile = projectile.GetComponent<Rigidbody2D>();

            if (rigidBody2DProjectile == null)
            {
                LogUtil.PrintWarning(gameObject, GetType(), "ExecuteUseSkill(): Prefab projectile is missing a Rigidbody2D component.");
                return;
            }

            rigidBody2DProjectile.AddForce(spawnPoint.m_direction * m_throwForce);
            rigidBody2DProjectile.AddTorque(m_throwTorque, ForceMode2D.Force);
        }

        private void InFluenceProjectileFlipX(GameObject projectile)
        {
            if (m_shouldInfluenceProjecileFlipX)
            {
                SpriteRenderer spriteRenderer = projectile.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.flipX = m_compSpriteRenderer.flipX;
                }
            }
        }

    }

}