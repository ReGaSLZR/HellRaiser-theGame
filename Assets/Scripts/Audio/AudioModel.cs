using Data.Storage;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using Utils;

namespace Audio {

    public class AudioModel : MonoBehaviour, AudioModel.VolumeSetter, AudioModel.VolumeGetter, AudioModel.BGMSetter, AudioModel.SFXSetter
    {

        #region Interfaces

        public interface BGMSetter {
            void PlayTemporaryBGM(AudioClip clip);
            void PlayOriginalBGM(bool shouldUseTransition);
            void ReplaceOriginalBGM(AudioClip clip);
            void StopBGM();
        }

        public interface SFXSetter {
            void PlaySFX(AudioClip clip);
        }

        public interface VolumeSetter
        {
            void SetVolumeBGM(float volume);
            void SetVolumeSFX(float volume);
        }

        public interface VolumeGetter
        {
            ReactiveProperty<float> GetVolumeBGM();
            ReactiveProperty<float> GetVolumeSFX();
        }

        #endregion

        [Header("Volumes")]

        [SerializeField]
        [Slider(0f, 1)]
        private float m_startingVolBGM = 0.75f;
        [SerializeField]
        [Slider(0f, 1f)]
        private float m_startingVolSFX = 0.75f;

        [Space]

        [SerializeField]
        [Required]
        private AudioSource m_audioSourceBGM;

        [SerializeField]
        [Required]
        private AudioSource m_audioSourceSFX;

        #region Inspector-only
        [ShowNativeProperty]
        public float CurrentVolBGM
        {
            get
            {
                return m_reactiveVolumeBGM.Value;
            }
        }
        [ShowNativeProperty]
        public float CurrentVolSFX
        {
            get
            {
                return m_reactiveVolumeSFX.Value;
            }
        }
        #endregion

        private ReactiveProperty<float> m_reactiveVolumeBGM = new ReactiveProperty<float>();
        private ReactiveProperty<float> m_reactiveVolumeSFX = new ReactiveProperty<float>();

        private AudioClip m_tempAudioClipBGM;

        private void Awake()
        {
            m_reactiveVolumeBGM.Value = AudioData.GetVolumeBGM(m_startingVolBGM);
            m_reactiveVolumeSFX.Value = AudioData.GetVolumeSFX(m_startingVolSFX);

            m_tempAudioClipBGM = m_audioSourceBGM.clip;

            if (m_tempAudioClipBGM == null) {
                LogUtil.PrintWarning(gameObject, GetType(), "Awake(): No BGM clip applied.");
            }
        }

        private void OnDestroy()
        {
            AudioData.SaveVolume(m_reactiveVolumeBGM.Value, m_reactiveVolumeSFX.Value);
        }

        private float ClampVolume(float volume)
        {
            return Mathf.Clamp(volume, 0f, 1f);
        }

        public void SetVolumeBGM(float volume)
        {
            m_reactiveVolumeBGM.Value = ClampVolume(volume);
        }

        public void SetVolumeSFX(float volume)
        {
            m_reactiveVolumeSFX.Value = ClampVolume(volume);
        }

        public ReactiveProperty<float> GetVolumeBGM()
        {
            return m_reactiveVolumeBGM;
        }

        public ReactiveProperty<float> GetVolumeSFX()
        {
            return m_reactiveVolumeSFX;
        }

        public void PlayTemporaryBGM(AudioClip clip)
        {
            //if(clip != null) {
                //m_audioSourceBGM.clip = clip;
                //m_audioSourceBGM.Play();

                AudioUtil.SafelyTransitionToClip(this, m_audioSourceBGM, clip);
            //}
        }

        public void PlayOriginalBGM(bool shouldUseTransition) {
            if ((m_audioSourceBGM.clip == m_tempAudioClipBGM) && m_audioSourceBGM.isPlaying) {
                return; //no need to play the BGM as it is already playing
            }

            if (shouldUseTransition)
            {
                AudioUtil.SafelyTransitionToClip(this, m_audioSourceBGM, m_tempAudioClipBGM);
            }
            else {
                m_audioSourceBGM.clip = m_tempAudioClipBGM;
                m_audioSourceBGM.Play();
            }
        }

        public void ReplaceOriginalBGM(AudioClip clip) {
            if (clip != null) {
                m_tempAudioClipBGM = clip;
            }
        }

        public void StopBGM() {
            m_audioSourceBGM.Stop();
        }

        public void PlaySFX(AudioClip clip) {
            if(clip != null) {
                m_audioSourceSFX.PlayOneShot(clip);
            }
        }

    }

}