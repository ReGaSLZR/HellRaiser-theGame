using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Input {

    public class InputOnScreenButtons : BaseInputModel
    {

        [SerializeField]
        private Button m_buttonJump;
        [SerializeField]
        private Button m_buttonMoveRight;
        [SerializeField]
        private Button m_buttonMoveLeft;

        [Space]

        [SerializeField]
        private Button m_buttonSkillMain;
        [SerializeField]
        private Button m_buttonSkillSecondary;
        [SerializeField]
        private Button m_buttonSkillTertiary;

        private void Start()
        {
            SetUpSkillMain();
            SetUpSkill2();
            SetUpSkill3();
            SetUpJump();

            SetUpMovement();

        }

        private void SetUpMovement()
        {
            m_buttonMoveRight.OnPointerDownAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => m_run = m_movementBaseSpeed)
                .AddTo(this);

            m_buttonMoveLeft.OnPointerDownAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => m_run = (m_movementBaseSpeed * -1))
                .AddTo(this);

            m_buttonMoveRight.OnPointerUpAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => m_run = 0f)
                .AddTo(this);

            m_buttonMoveLeft.OnPointerUpAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => m_run = 0f)
                .AddTo(this);
        }

        private void SetUpSkillMain()
        {
            m_buttonSkillMain.OnPointerDownAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => {
                    m_skillMain = true;
                })
                .AddTo(this);

            m_buttonSkillMain.OnPointerUpAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => {
                    m_skillMain = false;
                })
                .AddTo(this);
        }

        private void SetUpSkill2()
        {
            m_buttonSkillSecondary.OnPointerDownAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => {
                    m_skill2 = true;
                })
                .AddTo(this);

            m_buttonSkillSecondary.OnPointerUpAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => {
                    m_skill2 = false;
                })
                .AddTo(this);
        }

        private void SetUpSkill3()
        {
            m_buttonSkillTertiary.OnPointerDownAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => {
                    m_skill3 = true;
                })
                .AddTo(this);

            m_buttonSkillTertiary.OnPointerUpAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => {
                    m_skill3 = false;
                })
                .AddTo(this);
        }

        private void SetUpJump()
        {
            m_buttonJump.OnPointerDownAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => {
                    m_jump = true;
                })
                .AddTo(this);

            m_buttonJump.OnPointerUpAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => {
                    m_jump = false;
                })
                .AddTo(this);
        }

        protected override void SetInputType()
        {
            m_inputType = InputType.OnScreenButtons;
        }

    }

}