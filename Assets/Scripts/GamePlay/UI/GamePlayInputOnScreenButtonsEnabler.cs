using UnityEngine;
using UnityEngine.UI;
using GamePlay.Input;
using Zenject;
using UniRx.Triggers;
using UniRx;
using GamePlay.Stats;

namespace GamePlay.UI {

    public class GamePlayInputOnScreenButtonsEnabler : MonoBehaviour
    {

        [Inject]
        private BaseInputModel m_modelInput;

        [Inject]
        private GamePlayStatsModel.Getter m_modelGamePlayStats;

        [SerializeField]
        private Button[] m_buttonsMovement;

        [SerializeField]
        private Button[] m_buttonsSkillInOrder;

        private bool m_tempInputEnabled;

        private void Start()
        {
            if (m_modelInput.m_inputType != InputType.OnScreenButtons)
            {
                Destroy(gameObject);
            }

            this.FixedUpdateAsObservable()
                .Select(_ => m_modelInput.m_isEnabled)
                .Where(isEnabled => (isEnabled != m_tempInputEnabled))
                .Subscribe(isEnabled => {
                    m_tempInputEnabled = isEnabled;
                    SetAllButtonsEnabled(isEnabled);
                })
                .AddTo(this);

            m_modelGamePlayStats.GetCharacterStamina()
                .Subscribe(stamina => {
                    for (int x=0; x<m_buttonsSkillInOrder.Length; x++) {
                        m_buttonsSkillInOrder[x].interactable = 
                            (stamina >= m_modelGamePlayStats.GetCharacter().Value.m_skillCosts[x]);
                    }
                })
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