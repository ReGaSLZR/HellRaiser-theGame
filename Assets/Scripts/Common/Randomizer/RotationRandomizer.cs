using NaughtyAttributes;
using UnityEngine;

namespace Common.Randomizer {

    public class RotationRandomizer : MonoBehaviour
    {

        [SerializeField]
        [MinMaxSlider(-180f, 180f)]
        private Vector2 m_rotation;

        private void Awake()
        {
            gameObject.transform.localRotation =
                new Quaternion(0, 0, 
                    Random.Range(m_rotation.x, m_rotation.y), 1);
            Destroy(this);      
        }

    }

}