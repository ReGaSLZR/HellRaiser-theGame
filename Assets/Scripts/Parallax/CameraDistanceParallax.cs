namespace Parallax
{

    using UnityEngine;

    /// <summary>
    /// Implements Parallax behaviour (horizontal only, at the time of writing)
    /// based on Camera distance/movement.
    /// Reference link: https://techdibble.com/post/how-to-add-a-2d-parallax-effect-in-unity-3d/
    /// </summary>
    public class CameraDistanceParallax : MonoBehaviour
    {

        [SerializeField]
        [Range(0.1f, 10f)]
        private float m_speedCoefficient = 4f;

        [SerializeField]
        private bool m_isForward = true;

        [Space]

        [SerializeField]
        [Range(10f, 40f)]
        private float m_cameraDistance = 20f;


        private Transform m_mainCamera;
        private float m_mainCameraLastPositionX;

        private float m_originalPosX;

        private void Start()
        {
            m_mainCamera = Camera.main.transform;
            m_originalPosX = transform.position.x;

            SaveCameraPosition();
        }

        private void Update()
        {
            //only move parallax when the camera is nearby
            if(Mathf.Abs(m_originalPosX - m_mainCamera.position.x) <= m_cameraDistance)
            {
                gameObject.transform.position += ((
                    (GetLastCameraPosition() - GetCurrentCameraPosition())
                    * m_speedCoefficient)
                    * (m_isForward ? 1 : -1))
                    * Time.deltaTime;
            }

            SaveCameraPosition();
        }

        private void SaveCameraPosition()
        {
            m_mainCameraLastPositionX = m_mainCamera.position.x;
        }

        private Vector3 GetCurrentCameraPosition()
        {
            return new Vector3(m_mainCamera.position.x, 0, 0);
        }

        private Vector3 GetLastCameraPosition()
        {
            return new Vector3(m_mainCameraLastPositionX, 0, 0);
        }

    }

}