namespace Parallax
{

	using System;
    using UnityEngine;

	/// <summary>
	/// Reference link:
	/// https://github.com/ReGaSLZR/DigiDrop-Unity/blob/master/Scripts/AcceleroControl.cs
	/// </summary>
	public class AcceleroParallax : MonoBehaviour
    {

		[SerializeField]
        private float m_tiltDamping = 1.0f;

		[Space]

		[SerializeField]
		private bool m_isHorizontal;
		[SerializeField]
		private float m_minHorizontalForce = 1f;

        [Space]

		[SerializeField]
		private bool m_isVertical;
		[SerializeField]
		private float m_minVerticalForce = 1f;

		private Vector3 m_positionBuffer;
		private Bounds m_screenBounds;

		private float m_transitionWindowMin = 0.1f;
		private float m_previousAccelX = 5f; //temp value

		private void Start()
		{
			m_positionBuffer = transform.position;
			InitializeBounds();
		}

		private void InitializeBounds()
		{
			float height = (Camera.main.orthographicSize * 2) / 5;
			float width = (height * Screen.width / Screen.height) / 5;

			m_screenBounds = new Bounds(Vector3.zero, new Vector3(width, height, 0));
		}

		private void Update()
		{
			if (m_isHorizontal
                && ((Input.acceleration.x > 0f) ? (Input.acceleration.x >= m_minHorizontalForce) : false
                || (Input.acceleration.x < 0f) ? (Input.acceleration.x <= (m_minHorizontalForce * -1)) : false)
                )
			{
				m_positionBuffer.x += GetTiltX();
			}

			transform.position = m_positionBuffer;

			/*
			//get speed based on delta (fps rate)
			float tiltSpeed = Time.deltaTime * m_tiltDamping; 

			if (m_isHorizontal
                //&& (Input.acceleration.x >= m_minHorizontalForce)
                )
			{
				m_positionBuffer.x += Input.acceleration.x * tiltSpeed;
			}

			if (m_isVertical
                //&& (Input.acceleration.y >= m_minVerticalForce)
                )
			{
				m_positionBuffer.y += Input.acceleration.y * tiltSpeed;
			}

			transform.position = m_positionBuffer;
            */
		}

		private float GetTiltX()
		{
			//get speed based on delta (fps rate)
			//float speed = Time.deltaTime * m_tiltDamping;
			//float tiltX = (Input.acceleration.x + speed);
			float tiltX = (Time.deltaTime) / 2;

			float clampedTiltX = Mathf.Clamp(tiltX, m_screenBounds.min.x, m_screenBounds.max.x);
			float roundedTiltX = ((float)Math.Round(clampedTiltX, 2));

			if ((Mathf.Abs((Mathf.Abs(m_previousAccelX) - Mathf.Abs(roundedTiltX))) <= m_transitionWindowMin))
			{
				return m_previousAccelX;
			}

			m_previousAccelX = roundedTiltX;
			return roundedTiltX;
		}

	}

}