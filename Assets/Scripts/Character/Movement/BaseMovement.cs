using Audio;
using NaughtyAttributes;
using Scriptables;
using UniRx;
using UnityEngine;
using Zenject;
using static UnityEngine.RectTransform;

namespace Character.Movement {

    public abstract class BaseMovement : MonoBehaviour
    {

        [Inject]
        protected readonly AudioModel.SFXSetter m_modelSFX;

        [Header("----- Base variables -----")]

        //COMPONENTS
        [SerializeField]
        [Required]
        protected Animator m_compAnimator;
        [SerializeField]
        [Required]
        protected Rigidbody2D m_compRigidBody2D;
        [SerializeField]
        [Required]
        protected SpriteRenderer m_compSpriteRenderer;

        [SerializeField]
        protected Axis m_movementDirection;

        [SerializeField]
        protected bool m_shouldFlipSprite;

        [Header("Audio Settings")]

        [SerializeField]
        [Required]
        protected AudioSource m_audioSource;
        [SerializeField]
        protected AudioClip[] m_clipsStunned;
        [SerializeField]
        protected AudioClip m_clipDeath;

        [Space]

        [SerializeField]
        protected string m_animMove;
        [SerializeField]
        private string m_animStunned;
        [SerializeField]
        private string m_animDead;

        protected StatMovement m_statMovement = new StatMovement(0f);

        protected ReactiveProperty<bool> m_reactiveIsMovEnabled = new ReactiveProperty<bool>(true);
        protected CompositeDisposable m_disposables = new CompositeDisposable();

        protected virtual void Awake()
        {
            if(m_animDead.Equals(""))
            {
                m_animDead = m_animStunned;
            }
        }

        protected virtual void OnEnable()
        {
            SetMovementEnabled(true);
        }

        protected virtual void OnDisable()
        {
            SetMovementEnabled(false);
            m_disposables.Clear();
        }

        /// <summary>
        /// Force the rigidbody2D to start moving. TIP: check m_reactiveIsMoveEnabled.Value before calling.
        /// </summary>
        /// <param name="movementDirection">The direction of movement that's ready to be multiplied by speed and framerate.</param>
        protected void StartMovement(Vector2 movementDirection) {
            if (m_statMovement.m_movSpeed == 0f) {
                return;
            }

            //if (!m_compAnimator.GetBool(m_animMove))
            //{ //to prevent jitters
                AnimateMovement(m_animMove, true);
            //}

            if (RigidbodyType2D.Dynamic == m_compRigidBody2D.bodyType)
            {
                m_compRigidBody2D.position = (m_compRigidBody2D.position +
                    (movementDirection * m_statMovement.m_movSpeed * Time.deltaTime));
            }
            else {
                m_compRigidBody2D.MovePosition(m_compRigidBody2D.position +
                    (movementDirection * m_statMovement.m_movSpeed * Time.deltaTime));
            }
        }

        protected bool ShouldFlip(Transform transformToFace) {
            if (!m_shouldFlipSprite) { //quickly return if condition is satisfied; else, perform direction calculation
                return false;
            }

            float direction = ((Axis.Horizontal == m_movementDirection) ? 
                gameObject.transform.position.x : gameObject.transform.position.y) 
                - ((Axis.Horizontal == m_movementDirection) ? 
                transformToFace.position.x : transform.position.y);

            return (direction >= 0f);
        }

        protected bool ShouldFlip(float movement) {
            return m_shouldFlipSprite ? (movement < 0f) : false;
        }

        private void AnimateMovement(string animParam, bool shouldAnimate) {
            if (animParam.Equals("") || m_compAnimator == null || m_compAnimator.runtimeAnimatorController == null) {
                return;
            }

            if (m_compAnimator.GetBool(animParam) != shouldAnimate) { //to prevent jitters
                m_compAnimator.SetBool(animParam, shouldAnimate);
            }
        }

        public void Face(Transform transformToFace) {
            bool shouldFlip = ShouldFlip(transformToFace);

            if (Axis.Horizontal == m_movementDirection)
            {
                m_compSpriteRenderer.flipX = shouldFlip;
            }
            else
            {
                m_compSpriteRenderer.flipY = shouldFlip;
            }
        }

        public void SetStatMovement(StatMovement statMovement) {
            m_statMovement = statMovement;
        }

        public void SetMovementEnabled(bool isEnabled) {
            m_reactiveIsMovEnabled.Value = isEnabled;
        }

        public void StunMovement() {
            SetMovementEnabled(false);

            AnimateMovement(m_animMove, false);
            AnimateMovement(m_animStunned, true);

            AudioUtil.SafelyPlayRandom(m_audioSource, m_clipsStunned);
        }

        public void UnStunMovement() {
            SetMovementEnabled(true);
            AnimateMovement(m_animStunned, false);
        }

        public void TerminateMovement() {
            SetMovementEnabled(false);
            m_compRigidBody2D.bodyType = RigidbodyType2D.Static;

            m_audioSource.Stop();
            m_modelSFX.PlaySFX(m_clipDeath);

            AnimateMovement(m_animMove, false);
            AnimateMovement(m_animStunned, false);
            AnimateMovement(m_animDead, true);
        }

    }

}