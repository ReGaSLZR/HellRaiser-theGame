using Character.Movement;
using UnityEngine;

namespace Character.AI {

    public abstract class BaseCharacterAI : MonoBehaviour
    {

        [SerializeField]
        private BaseCharacterMovement m_movement;
        
        private void Awake()
        {

        }

    }

}