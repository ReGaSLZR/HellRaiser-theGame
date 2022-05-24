using Audio;
using GamePlay.Camera;
using GamePlay.Checkpoint;
using GamePlay.Dialogue;
using GamePlay.Input;
using GamePlay.Stats;
using GamePlay.Mission;
using NaughtyAttributes;
using UnityEngine;
using Scriptables;

namespace Injection {

    public class GamePlayInjector : BaseInjector<GamePlayInjector>, Instantiator
    {

        [SerializeField]
        [Required]
        private PlaySettings m_playSettings;

        [Space]

        [SerializeField]
        [Required]
        private CameraFXModel m_cameraFX;

        [SerializeField]
        [Required]
        private AudioModel m_audio;

        [Space]

        [SerializeField]
        [Required]
        private GamePlayInputManager m_inputManager;

        [SerializeField]
        [Required]
        private GamePlayStatsModel m_gamePlayStats;

        [Space]

        [SerializeField]
        [Required]
        private DialogueModel m_dialogue;

        [SerializeField]
        [Required]
        private MissionModel m_mission;

        [SerializeField]
        [Required]
        private CheckpointModel m_checkpoint;

        [Space]

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
            Container.Bind<PlaySettings.AudioTheme>().FromInstance(m_playSettings.m_audioTheme);

            BindModelAudio();

            BindModelGamePlay();
        }

        private void BindModelAudio()
        {
            Container.Bind<AudioModel.VolumeGetter>().FromInstance(m_audio);
            Container.Bind<AudioModel.VolumeSetter>().FromInstance(m_audio);

            Container.Bind<AudioModel.BGMSetter>().FromInstance(m_audio);
            Container.Bind<AudioModel.SFXSetter>().FromInstance(m_audio);
        }

        private void BindModelGamePlay()
        {
            Container.Bind<CameraFXModel.Setter>().FromInstance(m_cameraFX);

            //Player Input
            BaseInputModel baseInputModel = m_inputManager.GetBaseInput(m_playSettings.GamePlayInput);
            Container.Bind<BaseInputModel>().FromInstance(baseInputModel);

            //Dialogue
            Container.Bind<DialogueModel.Getter>().FromInstance(m_dialogue);
            Container.Bind<DialogueModel.Setter>().FromInstance(m_dialogue);

            //GamePlay Stats
            Container.Bind<GamePlayStatsModel.Getter>().FromInstance(m_gamePlayStats);
            Container.Bind<GamePlayStatsModel.Setter>().FromInstance(m_gamePlayStats);

            //Checkpoint
            Container.Bind<CheckpointModel.Getter>().FromInstance(m_checkpoint);
            Container.Bind<CheckpointModel.Setter>().FromInstance(m_checkpoint);

            //Timer and Mission
            Container.Bind<MissionModel.TimerGetter>().FromInstance(m_mission);
            Container.Bind<MissionModel.TimerSetter>().FromInstance(m_mission);
            Container.Bind<MissionModel.MissionGetter>().FromInstance(m_mission);
            Container.Bind<MissionModel.MissionSetter>().FromInstance(m_mission);

            //Merchant
            Container.Bind<MerchantModel.Getter>().FromInstance(m_merchant);
            Container.Bind<MerchantModel.Setter>().FromInstance(m_merchant);

        }


    }

}