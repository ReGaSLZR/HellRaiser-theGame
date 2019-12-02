using UnityEngine;
using Scriptables;
using Utils;
using UnityEngine.UI;
using UniRx;
using NaughtyAttributes;
using Zenject;
using Audio;
using static Scriptables.PlaySettings;

namespace MainMenu.Mission {

    public class MissionButtonsManager : MonoBehaviour
    {

        [Inject]
        private AudioModel.SFXSetter m_sfx;
        [Inject]
        private readonly AudioTheme m_audioTheme;

        [SerializeField]
        private MissionInfo[] m_missions;

        [SerializeField]
        private MissionButton[] m_buttons;

        [SerializeField]
        [Required]
        private Button m_buttonPrevious;

        [SerializeField]
        [Required]
        private Button m_buttonNext;
        
        private int m_page;

        private void Awake()
        {
            if ((m_missions.Length == 0) || (m_buttons.Length == 0)) {
                LogUtil.PrintWarning(gameObject, GetType(), "Awake(): No missions nor buttons set.");
                Destroy(this);
            }

        }

        private void Start()
        {
            m_buttonNext.OnClickAsObservable()
                .Subscribe(_ => {
                    m_page++;
                    OnPaginationButtonClick();
                })
                .AddTo(this);

            m_buttonPrevious.OnClickAsObservable()
                .Subscribe(_ => {
                    m_page--;
                    OnPaginationButtonClick();
                })
                .AddTo(this);

            m_page = 0;
            ConfigButtons();
        }

        private void OnPaginationButtonClick()
        {
            ConfigButtons();
            m_sfx.PlaySFX(m_audioTheme.m_sfxButtonClick);
        }

        private void ConfigButtons() {
            int startingIndex = m_page * m_buttons.Length;
            int maxIndex = startingIndex + m_buttons.Length;
            int buttonIndex = 0;

            for (int x=startingIndex; x<maxIndex; x++) {
                if (x < m_missions.Length)
                {
                    m_buttons[buttonIndex].SetMission(m_missions[x]);
                }
                else {
                    m_buttons[buttonIndex].Hide();
                }

                buttonIndex++;
            }

            m_buttonPrevious.gameObject.SetActive(m_page > 0);
            m_buttonNext.gameObject.SetActive(maxIndex < m_missions.Length);
        }

    }

}