using Data.Storage;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static Data.Storage.PlayerData;

namespace GamePlay.Stats
{

    public class GamePlayStatsModel : MonoBehaviour, GamePlayStatsModel.Getter, GamePlayStatsModel.Setter
    {

        #region Interfaces

        public interface Getter
        {
            ReactiveProperty<Scriptables.CharacterInfo> GetActiveCharacter();
            ReactiveProperty<int> GetActiveCharacterHealth();
            ReactiveProperty<int> GetActiveCharacterStamina();
            ReactiveProperty<bool> HasACharacterDied();

            ReactiveProperty<int> GetInventoryMoney();
        }

        public interface Setter
        {
            void RegisterCharacterForStats(Scriptables.CharacterInfo characterInfo);
            void AddActiveCharacterHealth(int additionalHealth);
            void AddActiveCharacterStamina(int additionalStamina);
            void UpdateCharacterHealth(string charName, int newHealth);
            void UpdateCharacterStamina(string charName, int newStamina);

            void AddInventoryMoney(int inventoryMoney);
        }

        #endregion

        private ReactiveProperty<Scriptables.CharacterInfo> m_activeCharInfo = new ReactiveProperty<Scriptables.CharacterInfo>();

        /// <summary>
        /// Determines if one of the Playable Characters (regardless of being active or not) has died. 
        /// </summary>
        private ReactiveProperty<bool> m_charHasDied = new ReactiveProperty<bool>(false);

        private List<Scriptables.CharacterInfo> m_listCharacterInfo = new List<Scriptables.CharacterInfo>();

        private ReactiveProperty<int> m_charHealth = new ReactiveProperty<int>();
        private ReactiveProperty<int> m_charStamina = new ReactiveProperty<int>();

        private ReactiveProperty<int> m_inventoryMoney = new ReactiveProperty<int>();

        private void Awake()
        {
            SetPlayerValues();
        }

        private void OnDestroy()
        {
            PlayerData.Save(new Inventory(m_inventoryMoney.Value));
        }

        private void SetActiveCharacter(Scriptables.CharacterInfo charInfo)
        {
            m_activeCharInfo.Value = charInfo;

            m_charHealth.Value = charInfo.m_health;
            m_charStamina.Value = charInfo.m_stamina;
        }

        private void SetPlayerValues()
        {
            Inventory inventory = PlayerData.LoadInventory();
            m_inventoryMoney.Value = inventory.m_money;
        }

        public ReactiveProperty<Scriptables.CharacterInfo> GetActiveCharacter()
        {
            return m_activeCharInfo;
        }

        public ReactiveProperty<int> GetActiveCharacterHealth()
        {
            return m_charHealth;
        }

        public ReactiveProperty<int> GetActiveCharacterStamina()
        {
            return m_charStamina;
        }

        public ReactiveProperty<bool> HasACharacterDied()
        {
            return m_charHasDied;
        }

        public ReactiveProperty<int> GetInventoryMoney()
        {
            return m_inventoryMoney;
        }

        public void RegisterCharacterForStats(Scriptables.CharacterInfo characterInfo)
        {
            Scriptables.CharacterInfo charInfoFromCache = GetCharacterInfoFromCache(characterInfo.m_avatar.m_name, false);

            if (charInfoFromCache == null)
            {
                m_listCharacterInfo.Add(characterInfo);
                SetActiveCharacter(characterInfo);
            }
            else
            {
                Scriptables.CharacterInfo previousCharacter = GetCharacterInfoFromCache(m_activeCharInfo.Value.m_avatar.m_name, true);
                previousCharacter.m_health = m_charHealth.Value;
                previousCharacter.m_stamina = m_charStamina.Value;
                m_listCharacterInfo.Add(previousCharacter);

                SetActiveCharacter(charInfoFromCache);
            }
        }

        private Scriptables.CharacterInfo GetCharacterInfoFromCache(string charName, bool shouldRemoveIfExists)
        {
            for (int x = 0; x < m_listCharacterInfo.Count; x++)
            {
                if (charName.Equals(m_listCharacterInfo[x].m_avatar.m_name))
                {
                    Scriptables.CharacterInfo info = m_listCharacterInfo[x];
                    if (shouldRemoveIfExists)
                    {
                        m_listCharacterInfo.RemoveAt(x);
                    }
                    return info;
                }
            }

            return null;
        }

        public void AddActiveCharacterHealth(int additionalHealth)
        {
            m_charHealth.Value = Mathf.Clamp(m_charHealth.Value + additionalHealth,
                0, Scriptables.CharacterInfo.HEALTH_MAX);
        }

        public void AddActiveCharacterStamina(int additionalStamina)
        {
            m_charStamina.Value = Mathf.Clamp(m_charStamina.Value + additionalStamina,
                0, Scriptables.CharacterInfo.STAMINA_MAX);
        }

        public void UpdateCharacterHealth(string charName, int newHealth)
        {
            UpdateCharacterHealthOrStamina(true, charName, newHealth, 0, Scriptables.CharacterInfo.HEALTH_MAX);
        }

        public void UpdateCharacterStamina(string charName, int newStamina)
        {
            UpdateCharacterHealthOrStamina(false, charName, newStamina, 0, Scriptables.CharacterInfo.STAMINA_MAX);
        }

        private void UpdateCharacterHealthOrStamina(bool isHealth, string charName, int value, int minValue, int maxValue)
        {
            int clampedValue = Mathf.Clamp(value, minValue, maxValue);

            if (charName.Equals(m_activeCharInfo.Value.m_avatar.m_name))
            {
                if (isHealth)
                {
                    m_charHealth.Value = clampedValue;
                    m_charHasDied.Value = (clampedValue <= 0);
                }
                else
                {
                    m_charStamina.Value = clampedValue;
                }
            }
            else
            {
                foreach (Scriptables.CharacterInfo charInfo in m_listCharacterInfo)
                {
                    if (charName.Equals(charInfo.m_avatar.m_name))
                    {
                        if (isHealth)
                        {
                            charInfo.m_health = clampedValue;
                            m_charHasDied.Value = (clampedValue <= 0);
                        }
                        else
                        {
                            charInfo.m_stamina = clampedValue;
                        }
                    }
                }
            }
        }

        public void AddInventoryMoney(int inventoryMoney)
        {
            m_inventoryMoney.Value = Mathf.Clamp(inventoryMoney + m_inventoryMoney.Value, 0, 99999);
        }

    }

}
