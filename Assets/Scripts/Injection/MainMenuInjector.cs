using UnityEngine;
using MainMenu.Mission;
using NaughtyAttributes;
using Scriptables;
using Audio;

namespace Injection {


    public class MainMenuInjector : BaseInjector<MainMenuInjector>
    {

        [SerializeField]
        [Required]
        private PlaySettings m_playSettings;

        [SerializeField]
        [Required]
        private AudioModel m_audio;

        [SerializeField]
        [Required]
        private MissionModel m_mission;

        public override void InstallBindings()
        {
            Container.Bind<PlaySettings.AudioTheme>().FromInstance(m_playSettings.m_audioTheme);
            Container.Bind<PlaySettings.ColorScheme>().FromInstance(m_playSettings.m_colorScheme);

            Container.Bind<AudioModel.BGMSetter>().FromInstance(m_audio);
            Container.Bind<AudioModel.SFXSetter>().FromInstance(m_audio);
            Container.Bind<AudioModel.VolumeGetter>().FromInstance(m_audio);
            Container.Bind<AudioModel.VolumeSetter>().FromInstance(m_audio);

            Container.Bind<MissionModel.Getter>().FromInstance(m_mission);
            Container.Bind<MissionModel.Setter>().FromInstance(m_mission);
        }

    }


}