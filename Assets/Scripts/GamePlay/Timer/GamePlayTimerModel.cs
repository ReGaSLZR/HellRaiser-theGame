using System.Collections;
using UniRx;
using UnityEngine;

namespace GamePlay.Timer {


    public class GamePlayTimerModel : MonoBehaviour, GamePlayTimerModel.Getter, GamePlayTimerModel.Setter
    {

        #region Interfaces

        public interface Getter {
            ReactiveProperty<int> GetTimer();
        }

        public interface Setter {
            void StartTimer();
            void PauseTimer();
            void AddToTimer(int extraTime);
        }

        #endregion

        [SerializeField]
        private int m_countdown = 180;

        private const float TIMER_TICK = 1f;
        private ReactiveProperty<int> m_reactiveTimer = new ReactiveProperty<int>();

        private void Awake()
        {
            m_reactiveTimer.Value = m_countdown;
        }

        private IEnumerator CorStartTimer() {
            while(m_reactiveTimer.Value > 0) {
                yield return new WaitForSeconds(TIMER_TICK);
                m_reactiveTimer.Value -= 1;
            }            
        }

        public ReactiveProperty<int> GetTimer()
        {
            return m_reactiveTimer;
        }

        public void StartTimer()
        {
            StartCoroutine(CorStartTimer());
        }

        public void PauseTimer()
        {
            StopAllCoroutines();
        }

        public void AddToTimer(int extraTime)
        {
            m_reactiveTimer.Value += extraTime;
        }
    }


}