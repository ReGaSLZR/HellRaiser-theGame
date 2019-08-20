using GamePlay.Stats;
using GamePlay.Timer;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GamePlay.UI {

    public class GamePlayHUD : MonoBehaviour
    {

        private const string PREFIX_CHARACTER_LEVEL = "Lv. ";

        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStats;

        [Inject]
        private readonly GamePlayTimerModel.Getter m_modelTimer;

        [SerializeField]
        private RawImage m_characterAvatar;
        [SerializeField]
        private TextMeshProUGUI m_textCharacterLevel;

        [Space]

        [SerializeField]
        private Slider m_sliderHealth;
        [SerializeField]
        private Image m_sliderHealthFill;
        [SerializeField]
        private Color m_colorHealthNormal = Color.green;
        [SerializeField]
        private Color m_colorHealthCritical = Color.red;

        [Space]

        [SerializeField]
        private Slider m_sliderStamina;
        
        [Space]

        [SerializeField]
        private TextMeshProUGUI m_textTimer;

        private void Start()
        {
            m_modelTimer.GetTimer()
                .Subscribe(currentTime => {
                    m_textTimer.text = currentTime.ToString();
                })
                .AddTo(this);

            m_modelStats.GetCharacterLevel()
                .Subscribe(level => {
                    m_textCharacterLevel.text = PREFIX_CHARACTER_LEVEL + level.ToString();
                })
                .AddTo(this);

            m_modelStats.GetCharacterAvatar()
                .Subscribe(avatar => m_characterAvatar.texture = avatar)
                .AddTo(this);

            m_modelStats.GetCharacterHealth()
                .Subscribe(health => {
                    m_sliderHealth.value = health;
                    m_sliderHealthFill.color = (health <= Scriptables.CharacterInfo.HEALTH_CRITICAL) ?
                        m_colorHealthCritical : m_colorHealthNormal;
                })
                .AddTo(this);

            m_modelStats.GetCharacterStamina()
                .Subscribe(stamina => m_sliderStamina.value = stamina)
                .AddTo(this);

        }

    }

}