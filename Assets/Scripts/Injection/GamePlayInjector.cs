using Audio;
using Character.Ground;
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
        private PlayerGroundModel m_modelPlayerGround;
        [SerializeField]
        [Required]
        private GamePlayInputController m_gamePlayInputController;

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
            //Player Ground
            Container.Bind<PlayerGroundModel.Getter>().FromInstance(m_modelPlayerGround);
            Container.Bind<PlayerGroundModel.Setter>().FromInstance(m_modelPlayerGround);

            //Player Input
            BaseInputModel baseInputModel = m_gamePlayInputController.GetBaseInput();
            LogUtil.PrintInfo(gameObject, GetType(), "base input is: " + baseInputModel.GetType());
            Container.Bind<BaseInputModel>().FromInstance(baseInputModel);

        }


    }

}