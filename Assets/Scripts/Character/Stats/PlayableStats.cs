using UniRx;
using Zenject;
using GamePlay.Stats;

namespace Character.Stats
{

    public class PlayableStats : BaseStats
    {

        [Inject]
        private readonly GamePlayStatsModel.Setter m_modelStatsSetter;
        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStatsGetter;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            m_modelStatsSetter.ConfigStatsForCharacter(m_stats.m_name, m_stats.m_avatar);
        }

        private void Start()
        {
            m_modelStatsGetter.GetCharacterHealth()
                .Where(health => m_stats.m_name.Equals(m_modelStatsGetter.GetCharacterName()))
                .Subscribe(modelHealth => {
                    m_reactiveHealth.Value = modelHealth;
                })
                .AddTo(this);
        }

        public override void DealDamage(int damage, bool isCritical)
        {
            base.DealDamage(damage, isCritical);
            m_modelStatsSetter.UpdateCharacterHealth(m_reactiveHealth.Value);
        }

        public override void RecoverHealth(int health)
        {
            base.RecoverHealth(health);
            m_modelStatsSetter.UpdateCharacterHealth(m_reactiveHealth.Value);
        }

    }

}