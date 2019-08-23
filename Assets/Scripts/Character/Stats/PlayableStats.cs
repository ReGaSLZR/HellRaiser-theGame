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
            m_modelStatsSetter.ConfigStatsForCharacter(m_info);
            InitObservers();
        }

        private void InitObservers()
        {
            m_modelStatsGetter.GetCharacterHealth()
                .Where(health => m_info.m_infoUI.m_name.Equals(
                    m_modelStatsGetter.GetCharacter().Value.m_infoUI.m_name))
                .Subscribe(modelHealth =>
                {
                    m_reactiveHealth.Value = modelHealth;
                })
                .AddTo(m_disposables);
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