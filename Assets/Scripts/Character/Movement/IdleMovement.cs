using UnityEngine;

namespace Character.Movement {

    public class IdleMovement : BaseMovement
    {

        [Header("----- Child variables -----")]

        [SerializeField]
        [Range(0.25f, 10f)]
        private float m_customStunLength = 0.25f;

        private void Start()
        {
            SetStatMovement(new Scriptables.StatMovement(0f, m_customStunLength)); //supercedes any other call to SetStatMovement.
        }

    }

}