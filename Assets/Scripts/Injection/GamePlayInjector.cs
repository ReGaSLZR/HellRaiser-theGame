﻿using Audio;
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
        private AudioModel m_modelAudio;

        [SerializeField]
        [Required]
        private GamePlayInputManager m_inputManager;

        [SerializeField]
        [Required]
        private GamePlayStatsModel m_gamePlayStats;

        [SerializeField]
        [Required]
        private GamePlayDialogueModel m_dialogue;

        [SerializeField]
        [Required]
        private MissionModel m_missionModel;

        [SerializeField]
        [Required]
        private MerchantModel m_merchantModel;

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
            Container.Bind<AudioModel.Getter>().FromInstance(m_modelAudio);
            Container.Bind<AudioModel.Setter>().FromInstance(m_modelAudio);
        }

        private void BindModelGamePlay()
        {
            //Player Input
            BaseInputModel baseInputModel = m_inputManager.GetBaseInput(m_playSettings.m_gamePlayInput);
            Container.Bind<BaseInputModel>().FromInstance(baseInputModel);

            Container.Bind<GamePlayDialogueModel.Getter>().FromInstance(m_dialogue);
            Container.Bind<GamePlayDialogueModel.Setter>().FromInstance(m_dialogue);

            Container.Bind<GamePlayStatsModel.Getter>().FromInstance(m_gamePlayStats);
            Container.Bind<GamePlayStatsModel.Setter>().FromInstance(m_gamePlayStats);

            Container.Bind<MissionModel.TimerGetter>().FromInstance(m_missionModel);
            Container.Bind<MissionModel.TimerSetter>().FromInstance(m_missionModel);
            Container.Bind<MissionModel.MissionGetter>().FromInstance(m_missionModel);
            Container.Bind<MissionModel.MissionSetter>().FromInstance(m_missionModel);

            Container.Bind<MerchantModel.Getter>().FromInstance(m_merchantModel);
            Container.Bind<MerchantModel.Setter>().FromInstance(m_merchantModel);

        }


    }

}