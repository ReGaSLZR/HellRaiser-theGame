using Audio;
using GamePlayInput;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Injection {


    public class GamePlayInjector : MonoInstaller<GamePlayInjector>
    {

        [SerializeField]
        [Required]
        private AudioModel m_modelAudio;

        [SerializeField]
        [Required]
        private GamePlayInputManager m_gamePlayInputManager;

        public override void InstallBindings()
        {
            BindModelAudio();

            BindModelPlayer();
        }

        private void BindModelAudio()
        {
            Container.Bind<AudioModel.Getter>().FromInstance(m_modelAudio);
            Container.Bind<AudioModel.Setter>().FromInstance(m_modelAudio);
        }

        private void BindModelPlayer()
        {
            //Player Input
            BaseInputModel baseInputModel = m_gamePlayInputManager.GetBaseInput();
            LogUtil.PrintInfo(gameObject, GetType(), "base input is: " + baseInputModel.GetType());
            Container.Bind<BaseInputModel>().FromInstance(baseInputModel);

        }


    }

}