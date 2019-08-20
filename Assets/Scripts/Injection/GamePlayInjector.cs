using Audio;
using GamePlay.Input;
using GamePlay.Stats;
using GamePlay.Timer;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Injection {

    public class GamePlayInjector : MonoInstaller<GamePlayInjector>, Instantiator
    {

        [SerializeField]
        [Required]
        private AudioModel m_modelAudio;

        [SerializeField]
        [Required]
        private GamePlayInputManager m_gamePlayInputManager;

        [SerializeField]
        [Required]
        private GamePlayStatsModel m_gamePlayStats;

        [SerializeField]
        [Required]
        private GamePlayTimerModel m_gamePlayTimer;

        public void InjectPrefab(GameObject prefab)
        {
            Container.InjectGameObject(prefab);
        }

        public GameObject InstantiateInjectPrefab(GameObject prefab, Transform parent)
        {
            GameObject instantiatedObject = Instantiate(prefab, parent.position, parent.rotation);
            InjectPrefab(instantiatedObject);
            return instantiatedObject;
        }

        public override void InstallBindings()
        {
            Container.Bind<Instantiator>().FromInstance(this);

            BindModelAudio();

            BindModelGamePlay();
        }

        private void BindModelAudio()
        {
            Container.Bind<AudioModel.Getter>().FromInstance(m_modelAudio);
            Container.Bind<AudioModel.Setter>().FromInstance(m_modelAudio);
        }

        private void BindModelGamePlay()
        {
            //Player Input
            BaseInputModel baseInputModel = m_gamePlayInputManager.GetBaseInput();
            LogUtil.PrintInfo(gameObject, GetType(), "base input is: " + baseInputModel.GetType());
            Container.Bind<BaseInputModel>().FromInstance(baseInputModel);

            Container.Bind<GamePlayStatsModel.Getter>().FromInstance(m_gamePlayStats);
            Container.Bind<GamePlayStatsModel.Setter>().FromInstance(m_gamePlayStats);

            Container.Bind<GamePlayTimerModel.Getter>().FromInstance(m_gamePlayTimer);
            Container.Bind<GamePlayTimerModel.Setter>().FromInstance(m_gamePlayTimer);

        }


    }

}