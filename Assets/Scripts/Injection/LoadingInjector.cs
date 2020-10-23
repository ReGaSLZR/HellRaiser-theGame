using Audio;
using NaughtyAttributes;
using Scriptables;
using UnityEngine;
using Zenject;

namespace Injection {

    public class LoadingInjector : BaseInjector<LoadingInjector>
    {

        [SerializeField]
        [Required]
        private PlaySettings m_playSettings;

        [SerializeField]
        [Required]
        private AudioModel m_audio;

        public override void InstallBindings()
        {
            Container.Bind<PlaySettings.AudioTheme>().FromInstance(m_playSettings.m_audioTheme);

            Container.Bind<AudioModel.BGMSetter>().FromInstance(m_audio);
            Container.Bind<AudioModel.VolumeGetter>().FromInstance(m_audio);
        }

    }


}