using GamePlay.Stats;
using GamePlay.Mission;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static Scriptables.PlaySettings;

namespace GamePlay.UI {

    public class GamePlayHUD : MonoBehaviour
    {

        private const string PREFIX_CHARACTER_RANK = "Rank ";

        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStats;
        [Inject]
        private readonly MissionModel.TimerGetter m_modelTimer;
        [Inject]
        private readonly ColorScheme m_colorScheme;

        [SerializeField]
        private RawImage m_characterAvatar;
        [SerializeField]
        private TextMeshProUGUI m_textCharacterRank;

        [Space]

        [SerializeField]
        private Slider m_sliderHealth;
        [SerializeField]
        private Image m_sliderHealthFill;

        [Space]

        [SerializeField]
        private Slider m_sliderStamina;

        [Space]

        [SerializeField]
        private TextMeshProUGUI m_textMoneyCount;

        [Space]

        [SerializeField]
        private TextMeshProUGUI m_textTimer;

        private void Start()
        {
            m_textTimer.color = m_colorScheme.m_time;
            m_textMoneyCount.color = m_colorScheme.m_moneyGain;

            InitObservers();
        }

        private void InitObservers() {
            if (m_modelTimer.IsTimed())
            {
                m_textTimer.gameObject.SetActive(true);
                m_modelTimer.GetTimer()
                    .Subscribe(currentTime =>
                    {
                        m_textTimer.text = currentTime.ToString();
                    })
                    .AddTo(this);
            }
            else
            {
                m_textTimer.gameObject.SetActive(false);
            }

            m_modelStats.GetActiveCharacter()
                .Subscribe(characterInfo => {
                    if (characterInfo.m_rank == Scriptables.CharacterRank.F)
                    {
                        m_textCharacterRank.gameObject.SetActive(false);
                    }
                    else {
                        m_textCharacterRank.gameObject.SetActive(true);
                        m_textCharacterRank.text = PREFIX_CHARACTER_RANK + characterInfo.m_rank.ToString();
                    }

                    m_characterAvatar.texture = characterInfo.m_avatar.m_avatarMain;
                })
                .AddTo(this);

            m_modelStats.GetInventoryMoney()
                .Subscribe(money => {
                    m_textMoneyCount.text = money.ToString("N0");
                })
                .AddTo(this);

            m_modelStats.GetActiveCharacterHealth()
                .Subscribe(health => {
                    m_sliderHealth.value = health;
                    m_sliderHealthFill.color = (health <= Scriptables.CharacterInfo.HEALTH_CRITICAL) ?
                        m_colorScheme.m_healthLoss : m_colorScheme.m_healthGain;
                })
                .AddTo(this);

            m_modelStats.GetActiveCharacterStamina()
                .Subscribe(stamina => m_sliderStamina.value = stamina)
                .AddTo(this);
        }

    }

}