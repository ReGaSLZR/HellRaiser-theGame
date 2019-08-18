using UnityEngine;

namespace Character.Movement {

    public class IdleMovement : BaseMovement
    {

        [Header("Custom stun length")]
        [SerializeField]
        [Range(0.25f, 10f)]
        private float m_stunLength = 0.25f;

        private void Start()
        {
            SetStatMovement(new Stats.StatMovement(0f, m_stunLength)); //supercedes any other call to SetStatMovement.
        }

    }

}