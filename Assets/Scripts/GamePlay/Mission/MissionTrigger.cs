using Audio;
using GamePlay.Base;
using UnityEngine;
using Zenject;
using static Scriptables.PlaySettings;

namespace GamePlay.Mission {

    public class MissionTrigger : BaseTrigger
    {

        public enum TriggerMode
        {
            DISPLAY_OBJECTIVE,
            COLLECT_KEY_DISPLAY_OBJ,
            FAIL,
            CLEAR
        }

        [Inject]
        private readonly MissionModel.MissionSetter m_modelMission;
        [Inject]
        private readonly AudioModel.SFXSetter m_modelSFX;
        [Inject]
        private readonly Checkpoint.CheckpointModel.Setter m_modelCheckpoint;
        [Inject]
        private readonly AudioTheme m_audioTheme;

        [SerializeField]
        private TriggerMode m_triggerType;

        [SerializeField]
        private BaseTrigger m_chainedTriggerAfterDialogue;

        public override void Execute()
        {
            m_isTriggered = true;
            m_modelSFX.PlaySFX(m_audioTheme.m_sfxMissionUpdate);

            switch (m_triggerType) {
                default:
                case TriggerMode.DISPLAY_OBJECTIVE: {
                        m_modelMission.ShowMissionObjective();
                        break;
                    }
                case TriggerMode.COLLECT_KEY_DISPLAY_OBJ: {
                        m_modelMission.CollectMissionKey();
                        m_modelMission.ShowMissionObjective();
                        break;
                    }
                case TriggerMode.FAIL: {
                        m_modelMission.EndMission(false);
                        break;
                    }
                case TriggerMode.CLEAR: {
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