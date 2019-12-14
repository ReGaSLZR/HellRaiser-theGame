﻿namespace GamePlay.Environment
{

    using UnityEngine;

    /// <summary>
    /// Reference link: https://techdibble.com/post/how-to-add-a-2d-parallax-effect-in-unity-3d/
    /// </summary>
    public class HorizontalParallaxEffector : MonoBehaviour
    {

        [Range(0.1f, 10f)]
        [SerializeField] private float m_speedCoefficient = 0.1f;

        [SerializeField] private bool m_isForward = true;

        private Transform m_mainCamera;
        private float m_mainCameraLastPositionX;

        private void Start()
        {
            m_mainCamera = Camera.main.transform;

            SaveCameraPosition();
        }

        private void Update()
        {
            gameObject.transform.position += (((GetLastCameraPosition() - GetCurrentCameraPosition()) * m_speedCoefficient)
              * (m_isForward ? 1 : -1)) * Time.deltaTime;

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