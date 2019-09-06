using UnityEngine;
using Character.Movement;
using NaughtyAttributes;

namespace Character.Skill {

    public class SeekTargetToFollowSkill : BaseSkill
    {

        [Header("----- Child variables -----")]

        [SerializeField]
        [Required]
        private TargetDetector m_targetDetector;

        [SerializeField]
        [Required]
        private FollowMovement m_followMovement;

        protected override void OnSkillStart()
        {
            base.OnSkillStart();

            //safe check just in case the targets have been refreshed by the time this code fires
            if (m_targetDetector.GetTargets().Count > 0 && 
                (m_targetDetector.GetTargets()[0] != null)) {
                    m_followMovement.SetFollowTarget(m_targetDetector.GetTargets()[0].transform);
            }
        }

    }


}