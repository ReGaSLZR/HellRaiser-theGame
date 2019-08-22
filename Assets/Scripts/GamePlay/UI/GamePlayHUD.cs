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
        private readonly MissionModel.TimerGetter m_modelTimer;

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
            else {
                m_textTimer.gameObject.SetActive(false);
            }

            m_modelStats.GetCharacter()
                .Subscribe(characterInfo => {
                    m_textCharacterLevel.text = PREFIX_CHARACTER_LEVEL + characterInfo.m_level;
                    m_characterAvatar.texture = characterInfo.m_infoUI.m_avatarMain;
                })
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