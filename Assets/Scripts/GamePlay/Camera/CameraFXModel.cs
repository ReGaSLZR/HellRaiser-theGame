namespace GamePlay.Camera
{

    using UnityEngine;
    using System.Collections;
    using Cinemachine;
    using NaughtyAttributes;

    public class CameraFXModel : MonoBehaviour, CameraFXModel.Setter
    {

        #region Interface and Enum

        public interface Setter
        {
            void ShowFX(CameraFXType cameraFXType, CinemachineVirtualCamera vCam);
        }

        #endregion

        [Header("Camera Shake params")]

        [SerializeField]
        private float m_shakeDelay;
        [SerializeField]
        private float m_shakeDuration = 0.3f;
        [SerializeField]
        private float m_shakeAmplitude = 1.2f;
        [SerializeField]
        private float m_shakeFrequency = 2.0f;

        [Header("Focus Speedlines params")]

        [SerializeField]
        private float m_focusDuration;

        [SerializeField]
        [Required]
        private GameObject m_focusFX;

        [Header("Grunge params")]

        [SerializeField]
        private float m_grungeDuration;

        [SerializeField]
        [Required]
        private GameObject m_grungeFX;

        private void Awake()
        {
            m_focusFX.SetActive(false);
            m_grungeFX.SetActive(false);
        }

        public void ShowFX(CameraFXType cameraFXType, CinemachineVirtualCamera vCam)
        {
            StopAllCoroutines();

            switch (cameraFXType)
            {
                case CameraFXType.Shake:
                    {
                        StartCoroutine(CorShake(vCam));
                        break;
                    }
                case CameraFXType.Focus_Speedlines:
                    {
                        StartCoroutine(CorShowSpeedlines(m_focusDuration, m_focusFX));
                        break;
                    }
                case CameraFXType.Focus_Gradient_Bottom:
                    {
                        StartCoroutine(CorShowSpeedlines(m_grungeDuration, m_grungeFX));
                        break;
                    }
            }
        }

        //The Idea was taken from:
        //https://github.com/Lumidi/CameraShakeInCinemachine/blob/master/SimpleCameraShakeInCinemachine.cs
        private IEnumerator CorShake(CinemachineVirtualCamera vCam)
        {
            if (vCam == null)
            {
                yield return null;
            }

            yield return new WaitForSeconds(m_shakeDelay);

            CinemachineBasicMultiChannelPerlin virtualCameraNoise
                = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (virtualCameraNoise != null)
            {
                //Start shake
                virtualCameraNoise.m_AmplitudeGain = m_shakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = m_shakeFrequency;

                yield return new WaitForSeconds(m_shakeDuration);

                //Stop shake
                virtualCameraNoise.m_AmplitudeGain = 0f;
            }
        }

        private IEnumerator CorShowSpeedlines(float duration, GameObject speedFX)
        {
            speedFX.SetActive(true);
            yield return new WaitForSeconds(duration);
            speedFX.SetActive(false);
        }

    }

}
