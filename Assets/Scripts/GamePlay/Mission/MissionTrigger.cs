﻿using GamePlay.Base;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace GamePlay.Mission {

    public class MissionTrigger : BaseTrigger
    {

        [Inject]
        private MissionModel.MissionSetter m_modelMission;

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

        public override void Execute()
        {
            m_isTriggered = true;

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
                        m_modelMission.EndMission(true);
                        break;
                    }
            }
            
            Destroy(gameObject);
        }

    }

}