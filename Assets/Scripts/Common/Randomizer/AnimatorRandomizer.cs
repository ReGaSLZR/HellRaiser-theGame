namespace Common.Randomizer
{

    using UnityEngine;
    using Utils;

    [RequireComponent(typeof(Animator))]
    public class AnimatorRandomizer : MonoBehaviour
    {

        private Animator m_compAnimator;

        [SerializeField] private RuntimeAnimatorController[] m_controllers;

        private void Awake()
        {
            
            if (m_controllers.Length == 0)
            {
                LogUtil.PrintInfo(this, GetType(),
                    "Awake(): No controllers defined. Destroying...");
                Destroy(this);
            }

            m_compAnimator = GetComponent<Animator>();
            Randomize();
        }

        private void Randomize()
        {
            m_compAnimator.runtimeAnimatorController =
                m_controllers[Random.Range(0, m_controllers.Length)];
            Destroy(this);
        }

    }

}