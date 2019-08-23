using Character.AI;
using Cinemachine;
using GamePlay.Input;
using GamePlay.Mission;
using GamePlay.Stats;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace GamePlay.Playable {

    public class PlayablesManager : MonoBehaviour
    {

        [Inject]
        private readonly BaseInputModel m_modelInput;

        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStats;

        [Inject]
        private readonly MissionModel.MissionSetter m_modelMission;

        [SerializeField]
        private CinemachineVirtualCamera m_playerCamera;

        [SerializeField]
        private PlayableAI[] m_playableChars;

        private int m_index;
        private int m_deadCharactersCount;

        private void OnEnable()
        {
            SetAllCharactersEnabled(false);

            m_index = 0;
            m_playableChars[m_index].enabled = true;
        }

        private void Start()
        {
            //on current playable character death...
            m_modelStats.GetCharacterHealth()
                .Where(health => health <= 0)
                .Subscribe(_ => {
                    
                    //force setting to null value to let the "death" be recognized in this class,
                    //in case there is a destruction delay on other behaviours
                    m_playableChars[m_index] = null;

                    if (AreAllCharactersDead())
                    {   
                        m_modelMission.EndMission(false);
                    }
                    else
                    {
                        EnableNextCharacter();
                    }
                })
                .AddTo(this);

            //on playable character switch
            this.FixedUpdateAsObservable()
                .Where(_ => m_modelInput.m_charChange)
                .Subscribe(_ => {
                    EnableNextCharacter();
                })
                .AddTo(this);
            
        }

        private bool AreAllCharactersDead() {
            for (int x=0; x<m_playableChars.Length; x++) {
                if (m_playableChars[x] != null) {
                    return false;
                }
            }

            return true;
        }

        private void SetAllCharactersEnabled(bool isEnabled) {
            for (int x = 0; x < m_playableChars.Length; x++)
            {
                if (m_playableChars[x] != null) {
                    m_playableChars[x].enabled = isEnabled;
                }
            }
        }

        private void EnableNextCharacter() {
            do {
                MoveToNextIndex();
            } while (m_playableChars[m_index] == null);

            SetAllCharactersEnabled(false);
            m_playableChars[m_index].enabled = true;
            m_playerCamera.m_Follow = m_playableChars[m_index].gameObject.transform;
        }

        private void MoveToNextIndex() {
            m_index = ((m_index + 1) >= m_playableChars.Length) ? 0 : (m_index + 1);
        }

    }

}