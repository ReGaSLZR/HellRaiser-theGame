using UnityEngine;
using UniRx;
using UniRx.Triggers;
using GamePlay.Camera;
using Utils;
using Scriptables;

namespace GamePlay.Input {

    public class InputKeyboard : BaseInputModel
    {
        [SerializeField]
        private InputSettings input;

        private void Start()
        {
            SetUpHorizontalMovementControls();
            SetUpSkillControls();
            SetUpCameraPanControls();

            //jump control
            this.UpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKeyDown(input.m_keyJump))
                .Subscribe(isPressed => m_jump = isPressed)
                .AddTo(this);

            //change playable character control
            this.UpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKeyDown(input.m_keyChangeChar))
                .Subscribe(isPressed => m_charChange = isPressed)
                .AddTo(this);
        }

        private void SetUpCameraPanControls() {
            SetUpCameraPanControl(input.m_keyCameraUp, CameraPanDirection.PAN_UP);
            SetUpCameraPanControl(input.m_keyCameraDown, CameraPanDirection.PAN_DOWN);
            SetUpCameraPanControl(input.m_keyCameraLeft, CameraPanDirection.PAN_LEFT);
            SetUpCameraPanControl(input.m_keyCameraRight, CameraPanDirection.PAN_RIGHT);
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
                .Select(_ => UnityEngine.Input.GetKeyDown(input.m_keySkillMain))
                .Subscribe(isPressed => m_skillMain = isPressed)
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => m_isEnabled && m_reactiveSkill2_enabled.Value)
                .Select(_ => UnityEngine.Input.GetKeyDown(input.m_keySkill2))
                .Subscribe(isPressed => m_skill2 = isPressed)
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => m_isEnabled && m_reactiveSkill3_enabled.Value)
                .Select(_ => UnityEngine.Input.GetKeyDown(input.m_keySkill3))
                .Subscribe(isPressed => m_skill3 = isPressed)
                .AddTo(this);
        }

        private void SetUpHorizontalMovementControls()
        {
            this.UpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKey(input.m_keyMoveRight))
                .Where(isPressed => isPressed)
                .Subscribe(isPressed => m_run = m_movementBaseSpeed)
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKey(input.m_keyMoveLeft))
                .Where(isPressed => isPressed)
                .Subscribe(isPressed => m_run = (m_movementBaseSpeed * -1))
                .AddTo(this);

            this.UpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => (!UnityEngine.Input.GetKey(input.m_keyMoveRight) && !UnityEngine.Input.GetKey(input.m_keyMoveLeft)))
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