using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace GamePlayInput {

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
        private KeyCode m_keyAttack = KeyCode.J;
        [SerializeField]
        private KeyCode m_keySkill1 = KeyCode.K;
        [SerializeField]
        private KeyCode m_keySkill2 = KeyCode.L;

        private void Start()
        {
            SetUpHorizontalMovementControls();
            SetUpSkillControls();

            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => Input.GetKeyDown(m_keyJump))
                .Subscribe(isPressed => m_jump = isPressed)
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => Input.GetKeyDown(m_keyAttack))
                .Subscribe(isPressed => m_attack = isPressed)
                .AddTo(this);

        }

        private void SetUpSkillControls()
        {
            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => Input.GetKeyDown(m_keySkill1))
                .Subscribe(isPressed => m_skill1 = isPressed)
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => Input.GetKeyDown(m_keySkill2))
                .Subscribe(isPressed => m_skill2 = isPressed)
                .AddTo(this);
        }

        private void SetUpHorizontalMovementControls()
        {
            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => Input.GetKey(m_keyMoveRight))
                .Where(isPressed => isPressed)
                .Subscribe(isPressed => m_run = movementBaseSpeed)
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => Input.GetKey(m_keyMoveLeft))
                .Where(isPressed => isPressed)
                .Subscribe(isPressed => m_run = (movementBaseSpeed * -1))
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .Where(_ => m_isEnabled)
                .Select(_ => (!Input.GetKey(m_keyMoveRight) && !Input.GetKey(m_keyMoveLeft)))
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