using Data.Storage;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

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

            ReactiveProperty<int> GetInventoryMoney();
            ReactiveProperty<int> GetInventoryScroll();

        }

        public interface Setter
        {
            void RegisterCharacterForStats(Scriptables.CharacterInfo characterInfo);
            void UpdateCharacterHealth(string charName, int newHealth);
            void UpdateCharacterStamina(string charName, int newStamina);

            void AddInventoryMoney(int inventoryMoney);
            void AddInventoryScroll(int inventoryScroll);
        }

        #endregion

        private ReactiveProperty<Scriptables.CharacterInfo> m_activeCharInfo = new ReactiveProperty<Scriptables.CharacterInfo>();
        private List<Scriptables.CharacterInfo> m_listCharacterInfo = new List<Scriptables.CharacterInfo>();

        private ReactiveProperty<int> m_charHealth = new ReactiveProperty<int>();
        private ReactiveProperty<int> m_charStamina = new ReactiveProperty<int>();

        private ReactiveProperty<int> m_inventoryMoney = new ReactiveProperty<int>();
        private ReactiveProperty<int> m_inventoryScroll = new ReactiveProperty<int>();

        private void Awake()
        {
            SetPlayerValues();
        }

        private void OnDestroy()
        {
            SaveProgress();
        }

        private void SetActiveCharacter(Scriptables.CharacterInfo charInfo) {
            m_activeCharInfo.Value = charInfo;

            m_charHealth.Value = charInfo.m_health;
            m_charStamina.Value = charInfo.m_stamina;
        }

        private void SetPlayerValues() {
            m_inventoryMoney.Value = PlayerData.GetInventoryMoney();
            m_inventoryScroll.Value = PlayerData.GetInventoryScroll();
        }

        //TODO call this upon mission end (whether cleared or not) 
        private void SaveProgress() {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            PlayerData.SaveInventory(m_inventoryMoney.Value, m_inventoryScroll.Value);
        }

        public ReactiveProperty<Scriptables.CharacterInfo> GetActiveCharacter() {
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

        public ReactiveProperty<int> GetInventoryMoney()
        {
            return m_inventoryMoney;
        }

        public ReactiveProperty<int> GetInventoryScroll()
        {
            return m_inventoryScroll;
        }

        public void RegisterCharacterForStats(Scriptables.CharacterInfo characterInfo) {
            Scriptables.CharacterInfo charInfoFromCache = GetCharacterInfoFromCache(characterInfo.m_infoUI.m_name, false);

            if (charInfoFromCache == null)
            {
                m_listCharacterInfo.Add(characterInfo);
                SetActiveCharacter(characterInfo);
            }
            else
            {
                Scriptables.CharacterInfo previousCharacter = GetCharacterInfoFromCache(m_activeCharInfo.Value.m_infoUI.m_name, true);
                previousCharacter.m_health = m_charHealth.Value;
                previousCharacter.m_stamina = m_charStamina.Value;
                m_listCharacterInfo.Add(previousCharacter);

                SetActiveCharacter(charInfoFromCache);
            }
        }

        private Scriptables.CharacterInfo GetCharacterInfoFromCache(string charName, bool shouldRemoveIfExists) {
            for (int x = 0; x < m_listCharacterInfo.Count; x++)
            {
                if (charName.Equals(m_listCharacterInfo[x].m_infoUI.m_name))
                {
                    Scriptables.CharacterInfo info = m_listCharacterInfo[x];
                    if (shouldRemoveIfExists) {
                        m_listCharacterInfo.RemoveAt(x);
                    }
                    return info;
                }
            }

            return null;
        }

        public void UpdateCharacterHealth(string charName, int newHealth)
        {
            UpdateCharacterHealthOrStamina(true, charName, newHealth, 0, Scriptables.CharacterInfo.HEALTH_MAX);
        }

        public void UpdateCharacterStamina(string charName, int newStamina)
        {
            UpdateCharacterHealthOrStamina(false, charName, newStamina, 0, Scriptables.CharacterInfo.STAMINA_MAX);
        }

        private void UpdateCharacterHealthOrStamina(bool isHealth, string charName, int barValue, int minValue, int maxValue) {
            int clampedValue = Mathf.Clamp(barValue, minValue, maxValue);

            if (charName.Equals(m_activeCharInfo.Value.m_infoUI.m_name))
            {
                if (isHealth)
                {
                    m_charHealth.Value = clampedValue;
                }
                else {
                    m_charStamina.Value = clampedValue;
                }
            }
            else
            {
                foreach (Scriptables.CharacterInfo charInfo in m_listCharacterInfo)
                {
                    if (charName.Equals(charInfo.m_infoUI.m_name))
                    {
                        if (isHealth)
                        {
                            charInfo.m_health = clampedValue;
                        }
                        else {
                            charInfo.m_stamina = clampedValue;
                        }
                    }
                }
            }
        }

        public void AddInventoryMoney(int inventoryMoney)
        {
            m_inventoryMoney.Value += inventoryMoney;
        }

        public void AddInventoryScroll(int inventoryScroll)
        {
            m_inventoryScroll.Value += inventoryScroll;
        }

    }

}
