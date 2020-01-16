using GamePlay.Camera;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Input {

    public class InputOnScreenButtons : BaseInputModel
    {

        [Header("Movement Buttons")]

        [SerializeField]
        private Button m_buttonJump;
        [SerializeField]
        private Button m_buttonMoveRight;
        [SerializeField]
        private Button m_buttonMoveLeft;

        [Header("Skill Buttons")]

        [SerializeField]
        private Button m_buttonSkillMain;
        [SerializeField]
        private Button m_buttonSkillSecondary;
        [SerializeField]
        private Button m_buttonSkillTertiary;

        [Header("Character Change Button")]

        [SerializeField]
        private Button m_buttonChangeChar;

        [Header("Camera Pan Buttons")]

        [SerializeField]
        private Button m_buttonCameraUp;

        [SerializeField]
        private Button m_buttonCameraDown;

        [SerializeField]
        private Button m_buttonCameraLeft;

        [SerializeField]
        private Button m_buttonCameraRight;

        private void Start()
        {
            SetUpSkillMain();
            SetUpSkill2();
            SetUpSkill3();

            SetUpJump();
            SetUpMovement();

            SetUpChangeCharControl();

            SetUpCameraPanControls();
        }

        private void SetUpCameraPanControls() {
            SetUpCameraPanControl(m_buttonCameraUp, CameraPanDirection.PAN_UP);
            SetUpCameraPanControl(m_buttonCameraDown, CameraPanDirection.PAN_DOWN);
            SetUpCameraPanControl(m_buttonCameraLeft, CameraPanDirection.PAN_LEFT);
            SetUpCameraPanControl(m_buttonCameraRight, CameraPanDirection.PAN_RIGHT);
        }

        private void SetUpCameraPanControl(Button button, CameraPanDirection direction) {
            button.OnPointerDownAsObservable()
                .Where(_ => m_isEnabled && (m_cameraPanDirection.Value == CameraPanDirection.NO_PAN))
                .Subscribe(_ => {
                    m_cameraPanDirection.Value = direction;
                })
                .AddTo(this);

            button.OnPointerUpAsObservable()
               .Where(_ => m_isEnabled)
               .Subscribe(_ => {
                   m_cameraPanDirection.Value = CameraPanDirection.NO_PAN;
               })
               .AddTo(this);
        }

        private void SetUpChangeCharControl() {
            m_buttonChangeChar.OnPointerDownAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => {
                    m_charChange = true;
                })
                .AddTo(this);

            m_buttonChangeChar.OnPointerUpAsObservable()
                .Where(_ => m_isEnabled)
                .Subscribe(_ => {
                    m_charChange = false;
                })
                .AddTo(this);
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
                .Where(_ => m_isEnabled && m_reactiveSkillMain_enabled.Value)
                .Subscribe(_ => {
                    m_skillMain = true;
                })
                .AddTo(this);

            m_buttonSkillMain.OnPointerUpAsObservable()
                .Where(_ => m_isEnabled && m_reactiveSkillMain_enabled.Value)
                .Subscribe(_ => {
                    m_skillMain = false;
                })
                .AddTo(this);
        }

        private void SetUpSkill2()
        {
            m_buttonSkillSecondary.OnPointerDownAsObservable()
                .Where(_ => m_isEnabled && m_reactiveSkill2_enabled.Value)
                .Subscribe(_ => {
                    m_skill2 = true;
                })
                .AddTo(this);

            m_buttonSkillSecondary.OnPointerUpAsObservable()
                .Where(_ => m_isEnabled && m_reactiveSkill2_enabled.Value)
                .Subscribe(_ => {
                    m_skill2 = false;
                })
                .AddTo(this);
        }

        private void SetUpSkill3()
        {
            m_buttonSkillTertiary.OnPointerDownAsObservable()
              .Where(_ => m_isEnabled && m_reactiveSkill3_enabled.Value)
              .Subscribe(_ => {
                  m_skill3 = true;
              })
              .AddTo(this);

            m_buttonSkillTertiary.OnPointerUpAsObservable()
                .Where(_ => m_isEnabled && m_reactiveSkill3_enabled.Value)
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