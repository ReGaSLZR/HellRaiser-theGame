using Audio;
using Character.Stats;
using GamePlay.Base;
using GamePlay.Mission;
using GamePlay.Stats;
using NaughtyAttributes;
using UnityEngine;
using Utils;
using Zenject;

namespace GamePlay.Item {

    public class PickableItemTrigger : BaseTrigger
    {

        //INJECTABLES
        [Inject]
        private readonly MissionModel.TimerSetter m_modelTimer;
        [Inject]
        private readonly GamePlayStatsModel.Setter m_modelStats;
        [Inject]
        private readonly AudioModel.SFXSetter m_modelSFX;

        public enum ItemType
        {
            STAT_HEALTH,
            STAT_STAMINA,
            TIMER,
            INVENTORY_MONEY
        }

        [SerializeField]
        private ItemType m_itemType;

        [SerializeField]
        [MinMaxSlider(1, 100)]
        private Vector2 m_valueRange;

        [Space]

        [SerializeField]
        private AudioClip[] m_clipOnPickUp;

        public override void Execute()
        {
            int valueFromRange = ValuesUtil.GetValueFromVector2Range(m_valueRange);
            m_modelSFX.PlaySFXRandom(m_clipOnPickUp);

            switch (m_itemType) {
                case ItemType.STAT_HEALTH:
                case ItemType.STAT_STAMINA:
                case ItemType.TIMER:
                    {
                        BaseStats triggererStats = GetBaseStatFromTriggerer();

                        if (triggererStats != null)
                        {
                            if (ItemType.TIMER == m_itemType)
                            {
                                m_modelTimer.AddToTimer(valueFromRange);
                                triggererStats.RecoverTime(valueFromRange);
                            }
                            else if (ItemType.STAT_HEALTH == m_itemType)
                            {
                                /*
                                 * the commented statement below is not any good if the Playable Character that picks up the item
                                 * tends to be an inactive character (e.g. the active character pushes the inactive one to pick up)
                                */
                                //m_modelStats.AddActiveCharacterHealth(valueFromRange); 
                                triggererStats.RecoverHealth(valueFromRange, false,
                                    StatInflictionType.PHYSICAL); //apply the recovery to the character that picks up the item
                            }
                            else
                            {
                                /*
                                 * same explanation as the comment above
                                 */
                                //m_modelStats.AddActiveCharacterStamina(valueFromRange);
                                triggererStats.RecoverStamina(valueFromRange, false,
                                    StatInflictionType.PHYSICAL);
                            }
                        }
                        break;
                    }
                default:
                case ItemType.INVENTORY_MONEY: {
                        m_modelStats.AddInventoryMoney(valueFromRange);
                        break;
                    }
            }

            Destroy(gameObject);
        }

        private BaseStats GetBaseStatFromTriggerer() {
            return m_triggerer.gameObject.GetComponent<BaseStats>();
        }

    }

}