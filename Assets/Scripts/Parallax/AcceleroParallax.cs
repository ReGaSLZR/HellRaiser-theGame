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
		private void Start()
		{
			m_positionBuffer = transform.position;
		}

		private void Update()
		{	
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
            
		}

	}

}