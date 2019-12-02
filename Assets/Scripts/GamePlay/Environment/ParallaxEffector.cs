namespace GamePlay.Environment
{

    using UnityEngine;

    /// <summary>
    /// Reference link: https://techdibble.com/post/how-to-add-a-2d-parallax-effect-in-unity-3d/
    /// </summary>
    public class ParallaxEffector : MonoBehaviour
    {
        
        [SerializeField] private Transform m_elementToMove;
        [Range(0.1f, 10f)]
        [SerializeField] private float m_speedCoefficient = 0.1f;

        [SerializeField] private bool m_isForward = true;

        private Transform m_mainCamera;
        private Vector3 m_mainCameraLastPosition;

        private void Start()
        {
            m_mainCamera = Camera.main.transform;
            m_mainCameraLastPosition = m_mainCamera.position;
        }

        private void Update()
        {
            m_elementToMove.position += (((m_mainCameraLastPosition - m_mainCamera.position) * m_speedCoefficient)
                * (m_isForward ? 1 : -1)) * Time.deltaTime;
            m_mainCameraLastPosition = m_mainCamera.position;
        }

    }

}