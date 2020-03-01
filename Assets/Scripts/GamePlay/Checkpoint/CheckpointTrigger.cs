namespace GamePlay.Checkpoint
{

    using GamePlay.Base;
    using NaughtyAttributes;
    using UnityEngine;
    using Zenject;

    public class CheckpointTrigger : BaseTrigger
    {

        [Inject]
        private readonly CheckpointModel.Setter m_checkpointSetter;

        [Required]
        [SerializeField] private Animator m_compAnimator;

        [Tooltip("The anim param for a boolean trigger.")]
        [SerializeField] private string m_animOnTrigger;

        [Space]

        [SerializeField] private Transform m_spawnPoint;

        [Space]

        [Required]
        [SerializeField] private Transform m_triggerFX;

        [Space]
        [SerializeField] private BaseTrigger m_chainedTrigger;

        private void Awake()
        {
            m_triggerFX.gameObject.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();

            if (m_checkpointGetter.IsCheckpoint(gameObject.GetInstanceID()))
            {
                m_isTriggered = true;
                m_compAnimator.SetBool(m_animOnTrigger, true);

                ExecuteChainedTrigger();
            }
        }

        public override void Execute()
        {
            m_isTriggered = true;

            m_triggerFX.gameObject.SetActive(true);
            m_compAnimator.SetBool(m_animOnTrigger, true);

            m_checkpointSetter.SaveCheckpoint(
                gameObject.GetInstanceID(),
                m_spawnPoint.position);

            ExecuteChainedTrigger();
        }

        private void ExecuteChainedTrigger()
        {
            if(m_chainedTrigger != null)
            {
                m_chainedTrigger.Execute();
            }
        }

    }


}