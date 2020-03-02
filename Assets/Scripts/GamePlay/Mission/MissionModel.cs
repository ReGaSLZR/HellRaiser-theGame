using Data.Storage;
using NaughtyAttributes;
using Scriptables;
using System.Collections;
using TMPro;
using UniRx;
using UnityEngine;
using Utils;
using static Data.Storage.PlayerData;

namespace GamePlay.Mission {


    public class MissionModel : MonoBehaviour, MissionModel.TimerGetter, MissionModel.TimerSetter, 
        MissionModel.MissionGetter, MissionModel.MissionSetter
    {

        #region Interfaces

        public interface TimerGetter {
            ReactiveProperty<int> GetTimer();
            bool IsTimed();
        }

        public interface TimerSetter {
            void AddToTimer(int extraTime);
            void StartTimer();
            void PauseTimer();
        }

        public interface MissionGetter {
            ReactiveProperty<MissionStatus> GetMissionStatus();
            bool ShouldAllCharactersSurvive();
        }

        public interface MissionSetter {
            void CollectMissionKey();
            void ShowMissionObjective();
            void EndMission(bool isCleared);
        }

        #endregion

        [SerializeField]
        [Required]
        private MissionInfo m_missionInfo;

        [SerializeField]
        [Range(1f, 10f)]
        private float m_objectiveShowLength = 1f;

        [SerializeField]
        private TextMeshProUGUI[] m_textObjective;

        private const float TIMER_TICK = 1f;

        private ReactiveProperty<int> m_reactiveTimer = new ReactiveProperty<int>();
        private ReactiveProperty<MissionStatus> m_missionStatus = new ReactiveProperty<MissionStatus>();
        private int m_missionKeysCollected;

        private MissionProgression m_missionProgression;

        private void Awake()
        {
            m_reactiveTimer.Value = m_missionInfo.m_timeLimit;
            m_missionStatus.Value = MissionStatus.ONGOING;

            m_missionProgression = PlayerData.LoadMissionProgression();
        }

        private void Start()
        {
            m_missionStatus
                .Where(status => (MissionStatus.CLEARED != status))
                .Subscribe(status => {
                    UpdateObjectiveTexts();
                })
                .AddTo(this);
        }

        private void OnDestroy()
        {
            if (MissionStatus.CLEARED == m_missionStatus.Value)
            {
                if (m_missionInfo.IsMainMission() &&
                    (m_missionInfo.m_buildIndex > m_missionProgression.m_mainCleared))
                {
                    LogUtil.PrintInfo(gameObject, GetType(), "Saving MAIN mission progression...");
                    PlayerData.Save(new MissionProgression(
                        m_missionInfo.m_buildIndex,
                        m_missionProgression.m_sideCleared));
                }
                else if (!m_missionInfo.IsMainMission() &&
                    (m_missionInfo.m_buildIndex > m_missionProgression.m_sideCleared))
                {
                    LogUtil.PrintInfo(gameObject, GetType(), "Saving SIDE mission progression...");
                    PlayerData.Save(new MissionProgression(
                        m_missionProgression.m_mainCleared,
                        m_missionInfo.m_buildIndex));
                }
                else {
                    LogUtil.PrintInfo(gameObject, GetType(),
                        "Not saving mission progression. " +
                        "This mission was cleared by the player before.");
                }
            }
        }

        private IEnumerator CorStartTimer() {
            while(m_reactiveTimer.Value > 0) {
                yield return new WaitForSeconds(TIMER_TICK);
                m_reactiveTimer.Value -= 1;

                if (m_reactiveTimer.Value == 0) {
                    m_missionStatus.Value = MissionStatus.FAILED;
                }
            }            
        }

        private IEnumerator CorShowObjective() {
            m_missionStatus.Value = MissionStatus.SHOWN;
            yield return new WaitForSeconds(m_objectiveShowLength);
            m_missionStatus.Value = MissionStatus.ONGOING;
        }

        private void UpdateObjectiveTexts()
        {
            string objective = GetMissionObjective();

            for (int x = 0; x < m_textObjective.Length; x++)
            {
                m_textObjective[x].text = objective;
            }
        }

        public ReactiveProperty<MissionStatus> GetMissionStatus() {
            return m_missionStatus;
        }

        public bool ShouldAllCharactersSurvive() {
            return m_missionInfo.m_allCharactersMustSurvive;
        }

        public string GetMissionObjective()
        {
            string objective = m_missionInfo.m_objective;

            if (m_missionInfo.m_isWithKey) {
                objective = "(" + m_missionKeysCollected.ToString() + "/" 
                    + m_missionInfo.m_keyCount.ToString() + ") "+ objective;
            }

            return objective;
        }

        public ReactiveProperty<int> GetTimer()
        {
            return m_reactiveTimer;
        }

        public bool IsTimed()
        {
            return m_missionInfo.m_isTimed;
        }

        public void AddToTimer(int extraTime)
        {
            if (IsTimed())
            {
                m_reactiveTimer.Value += extraTime;
            }
            else
            {
                LogUtil.PrintInfo(gameObject, GetType(), "AddToTimer(): Mission is NOT timed.");
            }
        }

        public void StartTimer()
        {
            if (IsTimed())
            {
                StartCoroutine(CorStartTimer());
            }
            else
            {
                LogUtil.PrintInfo(gameObject, GetType(), "StartTimer(): Mission is NOT timed.");
            }
        }

        public void PauseTimer()
        {
            StopAllCoroutines();
        }

        public void CollectMissionKey()
        {
            if (m_missionInfo.m_isWithKey)
            {
                m_missionKeysCollected++;

                if (m_missionKeysCollected == m_missionInfo.m_keyCount)
                {
                    PauseTimer();
                    m_missionStatus.Value = MissionStatus.CLEARED;
                }
            }
            else {
                LogUtil.PrintInfo(gameObject, GetType(), "CollectMissionKey(): Mission does NOT require keys.");
            }
        }

        public void ShowMissionObjective() {
            StartCoroutine(CorShowObjective());
        }

        public void EndMission(bool isCleared) {
            PauseTimer();
            m_missionStatus.Value = isCleared ? MissionStatus.CLEARED : MissionStatus.FAILED;
        }

    }

}