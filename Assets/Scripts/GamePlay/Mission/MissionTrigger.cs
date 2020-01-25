using Audio;
using GamePlay.Base;
using NaughtyAttributes;
using UnityEngine;
using Zenject;
using static Scriptables.PlaySettings;

namespace GamePlay.Mission {

    public class MissionTrigger : BaseTrigger
    {

        [Inject]
        private readonly MissionModel.MissionSetter m_modelMission;
        [Inject]
        private readonly AudioModel.SFXSetter m_modelSFX;
        [Inject]
        private readonly Checkpoint.CheckpointModel.Setter m_modelCheckpoint;
        [Inject]
        private readonly AudioTheme m_audioTheme;

        private const int TRIGGER_DISPLAY_OBJECTIVE = 0;
        private const int TRIGGER_COLLECT_KEY = 1;
        private const int TRIGGER_FAIL = 2;
        private const int TRIGGER_CLEAR = 3;
        private readonly DropdownList<int> m_dropdownTriggerTypes = new DropdownList<int>() {
            {"Display Objective",  TRIGGER_DISPLAY_OBJECTIVE},
            {"Collect Key, Display Objective",  TRIGGER_COLLECT_KEY},
            {"Fail Mission",  TRIGGER_FAIL},
            {"Clear Mission",  TRIGGER_CLEAR}
        };

        [SerializeField]
        [Dropdown("m_dropdownTriggerTypes")]
        private int m_triggerType;

        [SerializeField]
        private BaseTrigger m_chainedTriggerAfterDialogue;

        public override void Execute()
        {
            m_isTriggered = true;
            m_modelSFX.PlaySFX(m_audioTheme.m_sfxMissionUpdate);

            switch (m_triggerType) {
                default:
                case TRIGGER_DISPLAY_OBJECTIVE: {
                        m_modelMission.ShowMissionObjective();
                        break;
                    }
                case TRIGGER_COLLECT_KEY: {
                        m_modelMission.CollectMissionKey();
                        m_modelMission.ShowMissionObjective();
                        break;
                    }
                case TRIGGER_FAIL: {
                        m_modelMission.EndMission(false);
                        break;
                    }
                case TRIGGER_CLEAR: {
                        m_modelCheckpoint.ClearCheckpoint();
                        m_modelMission.EndMission(true);
                        break;
                    }
            }

            if (m_chainedTriggerAfterDialogue != null)
            {
                m_chainedTriggerAfterDialogue.Execute();
            }

            Destroy(gameObject, 0.1f);
        }

    }

}