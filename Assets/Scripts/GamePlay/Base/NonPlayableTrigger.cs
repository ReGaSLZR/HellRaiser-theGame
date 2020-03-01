namespace GamePlay.Base
{

    using NaughtyAttributes;
    using UnityEngine;

    public class NonPlayableTrigger : BaseTrigger
    {

        [Required]
        [SerializeField] private Transform m_objectToActivate;

        [Tooltip("Optional field")]
        [SerializeField] private Transform m_objectToDeactivate;

        [SerializeField] private BaseTrigger m_chainedTrigger;

        private void Awake()
        {
            m_objectToActivate.gameObject.SetActive(false);

            if (m_objectToDeactivate != null)
            {
                m_objectToDeactivate.gameObject.SetActive(true);
            }
        }

        protected override void Start()
        {
            //base.Start();
            //NOTE: Purposely not going to register for Trigger/Collision events.
        }

        protected override bool IsTriggerable(GameObject collidedObject)
        {
            return !m_isTriggered;
        }

        public override void Execute()
        {
            if (m_objectToDeactivate != null)
            {
                m_objectToDeactivate.gameObject.SetActive(false);
            }

            m_objectToActivate.gameObject.SetActive(true);

            if (m_chainedTrigger != null)
            {
                m_chainedTrigger.Execute();
            }
        }

    }


}