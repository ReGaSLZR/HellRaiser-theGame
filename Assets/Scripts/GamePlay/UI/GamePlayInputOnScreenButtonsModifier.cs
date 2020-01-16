using UnityEngine;
using UnityEngine.UI;
using GamePlay.Input;
using Zenject;
using UniRx.Triggers;
using UniRx;
using GamePlay.Stats;

namespace GamePlay.UI {

    /// <summary>
    /// Acts as
    /// [1] the enabler/disabler of OnScreen buttons
    /// when active Playable Character stamina is
    ///     [A] not sufficient for skill execution,
    ///     [B] not allowed (m_reactiveSkill_X_enabled) (possibly for tutorial)
    /// [2] the updater of skill buttons' icons.
    /// </summary>
    public class GamePlayInputOnScreenButtonsModifier : MonoBehaviour
    {

        [Inject]
        private BaseInputModel m_modelInput;

        [Inject]
        private GamePlayStatsModel.Getter m_modelGamePlayStats;

        [SerializeField]
        private Button[] m_buttonsMovement;

        [SerializeField]
        private Button[] m_buttonsSkillInOrder;

        [SerializeField]
        private RawImage[] m_rawImagesSkillInOrder;

        private bool m_tempInputEnabled;

        private void Start()
        {
            if (m_modelInput.m_inputType != InputType.OnScreenButtons)
            {
                Destroy(this);
            }

            //enable/disable all buttons when input is enabled or not
            this.FixedUpdateAsObservable()
                .Select(_ => m_modelInput.m_isEnabled)
                .Where(isEnabled => (isEnabled != m_tempInputEnabled))
                .Subscribe(isEnabled => {
                    m_tempInputEnabled = isEnabled;
                    SetAllButtonsEnabled(isEnabled);
                })
                .AddTo(this);

            //enable/disable SKILL buttons depending on stamina bar and skill cost
            m_modelGamePlayStats.GetActiveCharacterStamina()
                .Subscribe(stamina => {
                    for (int x=0; x<m_buttonsSkillInOrder.Length; x++) {
                        m_buttonsSkillInOrder[x].interactable = 
                            (stamina >= m_modelGamePlayStats.GetActiveCharacter().Value.m_skillsInOrder[x].m_cost);
                    }
                })
                .AddTo(this);

            //update skill icons when active playable character is changed
            m_modelGamePlayStats.GetActiveCharacter()
                .Subscribe(activeChar => {
                    for (int x=0; x<activeChar.m_skillsInOrder.Length; x++) {
                        m_rawImagesSkillInOrder[x].texture = activeChar.m_skillsInOrder[x].m_icon;
                    }           
                })
                .AddTo(this);

            SetSkillButtonsEnabled();

        }

        private void SetSkillButtonsEnabled()
        {
            m_modelInput.m_reactiveSkillMain_enabled
                .Subscribe(isSkillEnabled =>
                    m_buttonsSkillInOrder[0].gameObject.SetActive(isSkillEnabled))
                .AddTo(this);

            m_modelInput.m_reactiveSkill2_enabled
                .Subscribe(isSkillEnabled =>
                    m_buttonsSkillInOrder[1].gameObject.SetActive(isSkillEnabled))
                .AddTo(this);

            m_modelInput.m_reactiveSkill3_enabled
                .Subscribe(isSkillEnabled =>
                    m_buttonsSkillInOrder[2].gameObject.SetActive(isSkillEnabled))
                .AddTo(this);
        }

        private void SetButtonsEnabled(Button[] buttons, bool isEnabled) {
            for (int x = 0; x < buttons.Length; x++)
            {
                buttons[x].interactable = isEnabled;
            }
        }

        private void SetAllButtonsEnabled(bool isEnabled) {
            SetButtonsEnabled(m_buttonsMovement, isEnabled);
            SetButtonsEnabled(m_buttonsSkillInOrder, isEnabled);
        }

    }

}