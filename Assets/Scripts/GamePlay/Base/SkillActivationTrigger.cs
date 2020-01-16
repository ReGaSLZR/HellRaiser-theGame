namespace GamePlay.Base
{

    using Zenject;
    using Input;
    using UnityEngine;
    using Utils;

    public class SkillActivationTrigger : BaseTrigger
    {

        [Inject]
        private readonly BaseInputModel m_modelInput;

        [SerializeField]
        [Range(0, 2)]
        private int m_skillIndex = 0;

        [SerializeField]
        private bool m_isSkillEnabled;

        [SerializeField]
        private BaseTrigger m_chainedTriggerAfter;

        public override void Execute()
        {
            m_modelInput.SetSkillEnabled(m_skillIndex, m_isSkillEnabled);

            if (m_chainedTriggerAfter != null)
            {
                m_chainedTriggerAfter.Execute();
            }

            LogUtil.PrintInfo(this, GetType(), "Execute(): " +
                "Done with skill de/activation. Destroying gameObject.");
            Destroy(gameObject);
        }

    }

}