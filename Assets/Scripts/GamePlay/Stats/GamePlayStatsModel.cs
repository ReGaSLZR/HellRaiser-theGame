using Data.Storage;
using GamePlay.Mission;
using UniRx;
using UnityEngine;

namespace GamePlay.Stats
{

    public class GamePlayStatsModel : MonoBehaviour, GamePlayStatsModel.Getter, GamePlayStatsModel.Setter
    {

        #region Interfaces

        public interface Getter
        {
            ReactiveProperty<MissionStatus> GetMissionStatus();

            ReactiveProperty<Scriptables.CharacterInfo> GetCharacter();
            ReactiveProperty<int> GetCharacterHealth();
            ReactiveProperty<int> GetCharacterStamina();

            ReactiveProperty<int> GetAllianceLevel();
            ReactiveProperty<int> GetExperience();
            ReactiveProperty<int> GetInventoryMoney();
            ReactiveProperty<int> GetInventoryFood();

        }

        public interface Setter
        {
            void ConfigStatsForCharacter(Scriptables.CharacterInfo characterInfo);
            void UpdateCharacterHealth(int newHealth);
            void UpdateCharacterStamina(int newStamina);

            void AddExperience(int experience);
            void AddInventoryMoney(int inventoryMoney);
            void AddInventoryFood(int inventoryFood);

            void SetMissionStatus(MissionStatus missionStatus);
        }

        #endregion

        private ReactiveProperty<MissionStatus> m_missionStatus = new ReactiveProperty<MissionStatus>();

        private ReactiveProperty<Scriptables.CharacterInfo> m_charInfo = new ReactiveProperty<Scriptables.CharacterInfo>();
        private ReactiveProperty<int> m_charHealth = new ReactiveProperty<int>();
        private ReactiveProperty<int> m_charStamina = new ReactiveProperty<int>();
        private int m_skill2Cost = 0;
        private int m_skill3Cost = 0;

        private ReactiveProperty<int> m_allianceLevel = new ReactiveProperty<int>();
        private ReactiveProperty<int> m_experience = new ReactiveProperty<int>();
        private ReactiveProperty<int> m_inventoryMoney = new ReactiveProperty<int>();
        private ReactiveProperty<int> m_inventoryFood = new ReactiveProperty<int>();

        private void Awake()
        {
            m_missionStatus.Value = MissionStatus.IS_STARTED;
            SetCharacterValues();
            SetPlayerValues();
        }

        private void Start()
        {
            m_charHealth
                .Where(health => (health <= 0))
                .Subscribe(health => {
                    //TODO code logic for switching to next vignette

                    SetMissionStatus(MissionStatus.IS_FAILED); //TODO for now, auto-fail mission upon character death
                })
                .AddTo(this);
        }

        private void SetCharacterValues() {
            m_charHealth.Value = CharacterData.GetCharacterHealth();
            m_charStamina.Value = CharacterData.GetCharacterStamina();
        }

        private void SetPlayerValues() {
            m_allianceLevel.Value = PlayerData.GetAllianceLevel();
            m_experience.Value = PlayerData.GetExperience();
            m_inventoryMoney.Value = PlayerData.GetInventoryMoney();
            m_inventoryFood.Value = PlayerData.GetInventoryFood();
        }

        public void SaveProgress() {
            CharacterData.SaveCharacterStats(Scriptables.CharacterInfo.HEALTH_MAX, Scriptables.CharacterInfo.STAMINA_MAX);

            PlayerData.SaveExperience(m_experience.Value);
            PlayerData.SaveInventory(m_inventoryMoney.Value, m_inventoryFood.Value);
        }

        public ReactiveProperty<MissionStatus> GetMissionStatus()
        {
            return m_missionStatus;
        }

        public ReactiveProperty<Scriptables.CharacterInfo> GetCharacter() {
            return m_charInfo;
        }

        public ReactiveProperty<int> GetCharacterHealth()
        {
            return m_charHealth;
        }

        public ReactiveProperty<int> GetCharacterStamina()
        {
            return m_charStamina;
        }

        public ReactiveProperty<int> GetAllianceLevel()
        {
            return m_allianceLevel;
        }

        public ReactiveProperty<int> GetExperience()
        {
            return m_experience;
        }

        public ReactiveProperty<int> GetInventoryMoney()
        {
            return m_inventoryMoney;
        }

        public ReactiveProperty<int> GetInventoryFood()
        {
            return m_inventoryFood;
        }

        public void ConfigStatsForCharacter(Scriptables.CharacterInfo characterInfo) {
            m_charInfo.Value = characterInfo;
            CharacterData.SetCharacterName(name);
            SetCharacterValues();
        }

        public void UpdateCharacterHealth(int newHealth)
        {
            m_charHealth.Value = Mathf.Clamp(newHealth, 0, Scriptables.CharacterInfo.HEALTH_MAX);
        }

        public void UpdateCharacterStamina(int newStamina)
        {
            m_charStamina.Value = Mathf.Clamp(newStamina, 0, Scriptables.CharacterInfo.STAMINA_MAX);
        }

        public void AddExperience(int experience)
        {
            m_experience.Value += experience;
            //TODO: code logic for leveling up
        }

        public void AddInventoryMoney(int inventoryMoney)
        {
            m_inventoryMoney.Value += inventoryMoney;
        }

        public void AddInventoryFood(int inventoryFood)
        {
            m_inventoryFood.Value += inventoryFood;
        }

        public void SetMissionStatus(MissionStatus missionStatus)
        {
            m_missionStatus.Value = missionStatus;
        }
    }

}
