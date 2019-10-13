using NaughtyAttributes;
using UnityEngine;

namespace Common.Randomizer {

    public class RotationRandomizer : MonoBehaviour
    {

        [SerializeField]
        [MinMaxSlider(-180f, 180f)]
        private Vector2 m_rotation;

        private void Start()
        {
            gameObject.transform.localRotation = new Quaternion(0, 0, Mathf.RoundToInt(Random.Range(m_rotation.x, m_rotation.y)), 0);
            Destroy(this);      
        }

    }

}