using Audio;
using GamePlay.Dialogue;
using GamePlay.Input;
using GamePlay.Stats;
using GamePlay.Mission;
using NaughtyAttributes;
using UnityEngine;
using Zenject;
using Scriptables;

namespace Injection {

    public class GamePlayInjector : MonoInstaller<GamePlayInjector>, Instantiator
    {

        [SerializeField]
        [Required]
        private PlaySettings m_playSettings;

        [SerializeField]
        [Required]
        private AudioModel m_audio;

        [SerializeField]
        [Required]
        private GamePlayInputManager m_inputManager;

        [SerializeField]
        [Required]
        private GamePlayStatsModel m_gamePlayStats;

        [SerializeField]
        [Required]
        private DialogueModel m_dialogue;

        [SerializeField]
        [Required]
        private MissionModel m_mission;

        [SerializeField]
        [Required]
        private MerchantModel m_merchant;

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

            Container.Bind<PlaySettings.ColorScheme>().FromInstance(m_playSettings.m_colorScheme);

            BindModelAudio();

            BindModelGamePlay();
        }

        private void BindModelAudio()
        {
            Container.Bind<AudioModel.VolumeGetter>().FromInstance(m_audio);
            Container.Bind<AudioModel.VolumeSetter>().FromInstance(m_audio);

            Container.Bind<AudioModel.BGMSetter>().FromInstance(m_audio);
        }

        private void BindModelGamePlay()
        {
            //Player Input
            BaseInputModel baseInputModel = m_inputManager.GetBaseInput(m_playSettings.m_gamePlayInput);
            Container.Bind<BaseInputModel>().FromInstance(baseInputModel);

            Container.Bind<DialogueModel.Getter>().FromInstance(m_dialogue);
            Container.Bind<DialogueModel.Setter>().FromInstance(m_dialogue);

            Container.Bind<GamePlayStatsModel.Getter>().FromInstance(m_gamePlayStats);
            Container.Bind<GamePlayStatsModel.Setter>().FromInstance(m_gamePlayStats);

            Container.Bind<MissionModel.TimerGetter>().FromInstance(m_mission);
            Container.Bind<MissionModel.TimerSetter>().FromInstance(m_mission);
            Container.Bind<MissionModel.MissionGetter>().FromInstance(m_mission);
            Container.Bind<MissionModel.MissionSetter>().FromInstance(m_mission);

            Container.Bind<MerchantModel.Getter>().FromInstance(m_merchant);
            Container.Bind<MerchantModel.Setter>().FromInstance(m_merchant);

        }


    }

}