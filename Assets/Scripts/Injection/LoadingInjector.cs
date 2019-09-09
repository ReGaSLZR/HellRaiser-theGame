using Audio;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Injection {

    public class LoadingInjector : MonoInstaller<LoadingInjector>
    {

        [SerializeField]
        [Required]
        private AudioModel m_audio;

        public override void InstallBindings()
        {
            Container.Bind<AudioModel.BGMSetter>().FromInstance(m_audio);
            Container.Bind<AudioModel.VolumeGetter>().FromInstance(m_audio);
        }

    }


}