namespace GamePlay.Base
{

    using NaughtyAttributes;
    using Input;

    using System.Collections;

    using UnityEngine;
    using UnityEngine.Playables;

    using Zenject;

    public class CutsceneTrigger : BaseTrigger
    {

        [Inject]
        private readonly BaseInputModel m_modelInput;

        [Required]
        [SerializeField]
        private PlayableDirector m_playableCutscene;

        [SerializeField]
        private bool m_isFrozenInput;

        [Space]

        [SerializeField]
        private BaseTrigger m_chainedTrigger;

        [SerializeField]
        [Range(0.1f, 10f)]
        private float m_delayBeforeChainedTrigger = 0.1f;

        public override void Execute()
        {
            StopAllCoroutines();
            StartCoroutine(CorPlayCutscene());
        }

        private IEnumerator CorPlayCutscene()
        {
            m_playableCutscene.Play();

            if (m_isFrozenInput)
            {
                m_modelInput.DisableControls();
            }

            yield return new WaitForSeconds((float)m_playableCutscene.duration);

            if (m_isFrozenInput)
            {
                m_modelInput.EnableControls();
            }
            
            if ((m_chainedTrigger != null))
            {
                yield return new WaitForSeconds(m_delayBeforeChainedTrigger);
                m_chainedTrigger.Execute();
            }

            Destroy(this.gameObject);
            
        }

    }

}