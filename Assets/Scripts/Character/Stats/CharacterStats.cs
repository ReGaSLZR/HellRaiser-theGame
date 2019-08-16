using NaughtyAttributes;
using UnityEngine;

namespace Character.Stats {

    public abstract class CharacterStats : MonoBehaviour
    {

        private const int FULL_HEALTH = 100;

        [Range(1, FULL_HEALTH)]
        private int m_health;

        private void Start()
        {

        }


    }

}