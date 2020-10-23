using Audio;
using Data.Storage;
using NaughtyAttributes;
using Scriptables;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static Scriptables.PlaySettings;

namespace MainMenu.Mission
{

    public class MissionLoader : MonoBehaviour
    {

        [Inject]
        private readonly AudioModel.SFXSetter m_modelSFX;
        [Inject]
        private readonly AudioTheme m_audioTheme;

        [SerializeField]
        [Required]
        private Button m_button;

        [SerializeField]
        [Required]
        private Image m_panelLoading;

        private MissionInfo m_mission;

        private void Start()
        {
            m_panelLoading.gameObject.SetActive(false);

            m_button.OnClickAsObservable()
                .Subscribe(_ => LoadMission())
                .AddTo(this);
        }

        private void LoadMission() {
            m_button.interactable = false;
            m_modelSFX.PlaySFX(m_audioTheme.m_sfxButtonClick);
            m_panelLoading.gameObject.SetActive(true);

            SceneData.StoreLevelThenLoad(m_mission.m_buildIndex);
        }

        public void SetMission(MissionInfo mission, bool isUnlocked) {
            m_button.interactable = isUnlocked;
            m_mission = mission;
        }

    }

}
