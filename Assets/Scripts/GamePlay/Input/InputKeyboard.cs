using UnityEngine;
using UniRx;
using UniRx.Triggers;
using GamePlay.Camera;
using Utils;

namespace GamePlay.Input {

    public class InputKeyboard : BaseInputModel
    {

        [Header("Movement Keys")]

        [SerializeField]
        private KeyCode m_keyJump = KeyCode.I;
        [SerializeField]
        private KeyCode m_keyMoveRight = KeyCode.D;
        [SerializeField]
        private KeyCode m_keyMoveLeft = KeyCode.A;

        [Header("Skill Keys")]

        [SerializeField]
        private KeyCode m_keySkillMain = KeyCode.J;
        [SerializeField]
        private KeyCode m_keySkill2 = KeyCode.K;
        [SerializeField]
        private KeyCode m_keySkill3 = KeyCode.L;

        [Header("Change Playable Character Key")]

        [SerializeField]
        private KeyCode m_keyChangeChar = KeyCode.O;

        [Header("Camera Pan Keys")]

        [SerializeField]
        private KeyCode m_keyCameraUp = KeyCode.UpArrow;

        [SerializeField]
        private KeyCode m_keyCameraDown = KeyCode.DownArrow;

        [SerializeField]
        private KeyCode m_keyCameraLeft = KeyCode.LeftArrow;

        [SerializeField]
        private KeyCode m_keyCameraRight = KeyCode.RightArrow;

        private void Start()
        {
            SetUpHorizontalMovementControls();
            SetUpSkillControls();
            SetUpCameraPanControls();

            //jump control
            this.UpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKeyDown(m_keyJump))
                .Subscribe(isPressed => m_jump = isPressed)
                .AddTo(this);

            //change playable character control
            this.UpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKeyDown(m_keyChangeChar))
                .Subscribe(isPressed => m_charChange = isPressed)
                .AddTo(this);
        }

        private void SetUpCameraPanControls() {
            SetUpCameraPanControl(m_keyCameraUp, CameraPanDirection.PAN_UP);
            SetUpCameraPanControl(m_keyCameraDown, CameraPanDirection.PAN_DOWN);
            SetUpCameraPanControl(m_keyCameraLeft, CameraPanDirection.PAN_LEFT);
            SetUpCameraPanControl(m_keyCameraRight, CameraPanDirection.PAN_RIGHT);
        }

        private void SetUpCameraPanControl(KeyCode key, CameraPanDirection value) {
            this.UpdateAsObservable()
                .Where(_ => m_isEnabled && UnityEngine.Input.GetKey(key))
                .Subscribe(_ => {
                    m_cameraPanDirection.Value = value;

                })
                .AddTo(this);

            this.UpdateAsObservable()
               .Where(_ => m_isEnabled && UnityEngine.Input.GetKeyUp(key))
               .Subscribe(_ => {
                   m_cameraPanDirection.Value = CameraPanDirection.NO_PAN;

               })
               .AddTo(this);
        }

        private void SetUpSkillControls()
        {
            this.UpdateAsObservable()
                .Where(_ => m_isEnabled && m_reactiveSkillMain_enabled.Value)
                .Select(_ => UnityEngine.Input.GetKeyDown(m_keySkillMain))
                .Subscribe(isPressed => m_skillMain = isPressed)
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => m_isEnabled && m_reactiveSkill2_enabled.Value)
                .Select(_ => UnityEngine.Input.GetKeyDown(m_keySkill2))
                .Subscribe(isPressed => m_skill2 = isPressed)
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => m_isEnabled && m_reactiveSkill3_enabled.Value)
                .Select(_ => UnityEngine.Input.GetKeyDown(m_keySkill3))
                .Subscribe(isPressed => m_skill3 = isPressed)
                .AddTo(this);
        }

        private void SetUpHorizontalMovementControls()
        {
            this.UpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKey(m_keyMoveRight))
                .Where(isPressed => isPressed)
                .Subscribe(isPressed => m_run = m_movementBaseSpeed)
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKey(m_keyMoveLeft))
                .Where(isPressed => isPressed)
                .Subscribe(isPressed => m_run = (m_movementBaseSpeed * -1))
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => (!UnityEngine.Input.GetKey(m_keyMoveRight) && !UnityEngine.Input.GetKey(m_keyMoveLeft)))
                .Where(isLetGo => isLetGo)
                .Subscribe(_ => m_run = 0f)
                .AddTo(this);

        }

        protected override void SetInputType()
        {
            m_inputType = InputType.Keyboard;
        }
    }

}