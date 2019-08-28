using GamePlay.Base;
using GamePlay.Mission;
using GamePlay.Stats;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace GamePlay.Item {

    public class PickableItemTrigger : BaseTrigger
    {

        //INJECTABLES
        [Inject]
        private readonly MissionModel.TimerSetter m_modelTimer;
        [Inject]
        private readonly GamePlayStatsModel.Setter m_modelStats;

        private const string STAT_HEALTH = "STAT_HEALTH";
        private const string STAT_STAMINA = "STAT_STAMINA";
        private const string TIMER = "TIMER";
        private const string INVENTORY_MONEY = "INVENTORY_MONEY";
        private readonly string[] m_dropdownItemOptions = new string[] {
            STAT_HEALTH,
            STAT_STAMINA,
            TIMER,
            INVENTORY_MONEY
        };

        [SerializeField]
        [Dropdown("m_dropdownItemOptions")]
        private string m_itemType;

        [SerializeField]
        [MinMaxSlider(1, 100)]
        private Vector2 m_valueRange;

        public override void Execute()
        {
            switch (m_itemType) {
                case STAT_HEALTH: {
                        m_modelStats.AddActiveCharacterHealth(GetValueFromRange());
                        break;
                    }
                case STAT_STAMINA: {
                        m_modelStats.AddActiveCharacterStamina(GetValueFromRange());
                        break;
                    }
                case TIMER: {
                        m_modelTimer.AddToTimer(GetValueFromRange());
                        break;
                    }
                default:
                case INVENTORY_MONEY: {
                        m_modelStats.AddInventoryMoney(GetValueFromRange());
                        break;
                    }
            }

            Destroy(gameObject);
        }

        private int GetValueFromRange() {
            return Mathf.RoundToInt(Random.Range(m_valueRange.x, m_valueRange.y));
        }

    }

}