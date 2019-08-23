using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GamePlay.Input {

    public class InputKeyboard : BaseInputModel
    {

        [SerializeField]
        private KeyCode m_keyJump = KeyCode.I;
        [SerializeField]
        private KeyCode m_keyMoveRight = KeyCode.D;
        [SerializeField]
        private KeyCode m_keyMoveLeft = KeyCode.A;

        [Space]

        [SerializeField]
        private KeyCode m_keySkillMain = KeyCode.J;
        [SerializeField]
        private KeyCode m_keySkill2 = KeyCode.K;
        [SerializeField]
        private KeyCode m_keySkill3 = KeyCode.L;

        [Space]

        [SerializeField]
        private KeyCode m_keyChangeChar = KeyCode.O;

        private void Start()
        {
            SetUpHorizontalMovementControls();
            SetUpSkillControls();

            //jump control
            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKeyDown(m_keyJump))
                .Subscribe(isPressed => m_jump = isPressed)
                .AddTo(this);

            //change playable character control
            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKeyDown(m_keyChangeChar))
                .Subscribe(isPressed => m_charChange = isPressed)
                .AddTo(this);
        }

        private void SetUpSkillControls()
        {
            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKeyDown(m_keySkillMain))
                .Subscribe(isPressed => m_skillMain = isPressed)
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKeyDown(m_keySkill2))
                .Subscribe(isPressed => m_skill2 = isPressed)
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKeyDown(m_keySkill3))
                .Subscribe(isPressed => m_skill3 = isPressed)
                .AddTo(this);
        }

        private void SetUpHorizontalMovementControls()
        {
            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKey(m_keyMoveRight))
                .Where(isPressed => isPressed)
                .Subscribe(isPressed => m_run = m_movementBaseSpeed)
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => UnityEngine.Input.GetKey(m_keyMoveLeft))
                .Where(isPressed => isPressed)
                .Subscribe(isPressed => m_run = (m_movementBaseSpeed * -1))
                .AddTo(this);

            this.FixedUpdateAsObservable()
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