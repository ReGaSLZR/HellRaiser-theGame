using UnityEngine;
using NaughtyAttributes;
using Pooling;
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
        private bool m_isPooled;

        [SerializeField]
        [DisableIf("m_isPooled")]
        private GameObject m_prefabProjectile;

        [SerializeField]
        [EnableIf("m_isPooled")]
        private ObjectPooler m_projectilePooler;

        [Space]

        [SerializeField]
        private float m_throwForce = 1f;
        [SerializeField]
        private float m_throwTorque = 1f;

        [SerializeField]
        private bool m_shouldInfluenceProjectileFlipX;

        [Space]

        [SerializeField]
        private bool m_hasSingleSpawnPoint = true;

        [SerializeField]
        [DisableIf("m_hasSingleSpawnPoint")]
        private SpawnPoint m_spawnPointMinus;
        [SerializeField]
        [Required]
        private SpawnPoint m_spawnPointPlus;

        protected override void Awake()
        {
            base.Awake();

            if (m_isPooled)
            {
                if (m_projectilePooler == null)
                {
                    LogUtil.PrintWarning(this, GetType(), "Awake(): Configged to use " +
                        "ObjectPooler but the pooler reference is missing. " +
                        "Setting back to default instantiation of projectiles.");
                    m_isPooled = false;
                }
            }

            if (!m_isPooled && m_prefabProjectile == null)
            {
                LogUtil.PrintError(this, GetType(), "Awake(): No ObjectPooler " +
                    "nor Prefab Projectile set. Destroying...");
                Destroy(this);
            }
        }

        protected override void OnSkillStart()
        {
            base.OnSkillStart();

            SpawnPoint spawner = GetProjectileSpawner();
            Transform spawnTransform = spawner.gameObject.transform;
            GameObject projectile;

            if (m_isPooled)
            {
                projectile = m_projectilePooler.GetObjectFromPool(
                    spawnTransform.position, spawnTransform.rotation);
            }
            else
            {
                projectile = m_instantiator.InstantiateInjectPrefab(
                    m_prefabProjectile, spawnTransform);
            }

            PassStatOffenseTo(projectile);
            InfluenceProjectileFlipX(projectile);
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

        private void InfluenceProjectileFlipX(GameObject projectile)
        {
            if (m_shouldInfluenceProjectileFlipX)
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