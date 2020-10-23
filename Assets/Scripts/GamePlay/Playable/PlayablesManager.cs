using Character.AI;
using GamePlay.Input;
using GamePlay.Mission;
using GamePlay.Stats;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils;
using Zenject;

namespace GamePlay.Playable
{

    /// <summary>
    /// The holder of all PlayableAI instances (Playable Characters) in the scene.
    /// Works as a switcher of characters when [1] the active character dies, and [2] the Player switches out the active one.
    /// Works as an automatic mission end trigger (Failed scenario) when all PlayableAIs are null / all PCs are "dead".
    /// 
    /// Also works as a mass teleporter of Playables to a location.
    /// </summary>
    public class PlayablesManager : MonoBehaviour
    {

        [Inject]
        private readonly BaseInputModel m_modelInput;
        [Inject]
        private readonly GamePlayStatsModel.Getter m_modelStats;
        [Inject]
        private readonly MissionModel.MissionGetter m_modelMissionGetter;
        [Inject]
        private readonly MissionModel.MissionSetter m_modelMissionSetter;

        private PlayableAI[] m_playableChars;

        [SerializeField]
        private float m_playableCharSwitchDuration = 0.5f;

        [SerializeField]
        private float m_distanceXMassTeleport = 1.5f;

        //TODO code FX for teleportation

        private bool m_isCharacterSwitchingReady;
        private int m_index;

        private void Awake()
        {
            if (GameObject.FindObjectsOfType<PlayablesManager>().Length > 1)
            {
                LogUtil.PrintError(gameObject, GetType(), "Cannot have more than 1 PlayablesManager. Destroying...");
                Destroy(this);
            }

            m_playableChars = GameObject.FindObjectsOfType<PlayableAI>();
        }

        private void Start()
        {
            InitObservers();

            SetAllCharactersEnabled(false);

            m_isCharacterSwitchingReady = true;
            m_index = m_playableChars.Length;

            StopAllCoroutines();
            StartCoroutine(CorEnableNextCharacter());
        }

        public void MassTeleportPlayables(Transform location)
        {
            for (int x = 0; x < m_playableChars.Length; x++)
            {
                if (m_playableChars[x] != null)
                {
                    m_playableChars[x].gameObject.SetActive(false);

                    Vector3 teleportedLocation = location.position;
                    teleportedLocation.x += (m_distanceXMassTeleport * x);
                    m_playableChars[x].gameObject.transform.position = teleportedLocation;

                    m_playableChars[x].gameObject.SetActive(true);
                }
            }
        }

        private void InitObservers()
        {
            //on ACTIVE playable character death...
            m_modelStats.GetActiveCharacterHealth()
                .Where(health => health <= 0)
                .Subscribe(_ => {
                    //force setting to null value to let the "death" be recognized in this class,
                    //in case there is a destruction delay on other behaviours
                    m_playableChars[m_index] = null;

                    if (AreAllCharactersDead())
                    {
                        m_modelMissionSetter.EndMission(false);
                    }
                    else
                    {
                        StartCoroutine(CorEnableNextCharacter());
                    }

                })
                .AddTo(this);

            //on ANY playable character death... (regardless of being active or not)
            m_modelStats.HasACharacterDied()
                .Where(hasDied => hasDied && m_modelMissionGetter.ShouldAllCharactersSurvive())
                .Subscribe(_ => m_modelMissionSetter.EndMission(false))
                .AddTo(this);

            //on playable character switch
            this.UpdateAsObservable()
                .Where(_ => m_modelInput.m_charChange && m_isCharacterSwitchingReady)
                .Subscribe(_ => {
                    StopAllCoroutines();

                    StartCoroutine(CorEnableNextCharacter());
                    StartCoroutine(CorStartSwitchTick());
                })
                .AddTo(this);
        }

        private IEnumerator CorStartSwitchTick()
        {
            m_isCharacterSwitchingReady = false;
            yield return new WaitForSeconds(m_playableCharSwitchDuration);
            m_isCharacterSwitchingReady = true;
        }

        private bool AreAllCharactersDead()
        {
            for (int x = 0; x < m_playableChars.Length; x++)
            {
                if ((m_playableChars[x] != null) && !m_playableChars[x].m_isNotOnCycle)
                {
                    return false;
                }
            }

            return true;
        }

        private void SetCharacterEnabled(int index, bool isEnabled)
        {
            m_playableChars[index].enabled = isEnabled;
        }

        private void SetAllCharactersEnabled(bool isEnabled)
        {
            for (int x = 0; x < m_playableChars.Length; x++)
            {
                if (m_playableChars[x] != null)
                {
                    SetCharacterEnabled(x, isEnabled);
                }
            }
        }

        private IEnumerator CorEnableNextCharacter()
        {
            //yield statement is to prevent a race condition caused by active character's death and the immediate switching to another character
            //in the race condition, the next active character receives the last damage taken by the previously killed one
            //(brought by the immediate switching to another character and registering of the next character's info to the model)
            yield return null; 

            do
            {
                MoveToNextIndex();
            } while ((m_playableChars[m_index] == null) || m_playableChars[m_index].m_isNotOnCycle);

            SetAllCharactersEnabled(false);
            SetCharacterEnabled(m_index, true);
        }

        private void MoveToNextIndex()
        {
            m_index = ((m_index + 1) >= m_playableChars.Length) ? 0 : (m_index + 1);
        }

    }

}