using Data.Storage;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace Audio {

    public class AudioModel : MonoBehaviour, AudioModel.Setter, AudioModel.Getter
    {

        #region Interfaces

        public interface Setter
        {
            void SetVolumeBGM(float volume);
            void SetVolumeSFX(float volume);
        }

        public interface Getter
        {
            ReactiveProperty<float> GetVolumeBGM();
            ReactiveProperty<float> GetVolumeSFX();
        }

        #endregion

        private ReactiveProperty<float> m_reactiveVolumeBGM = new ReactiveProperty<float>();
        private ReactiveProperty<float> m_reactiveVolumeSFX = new ReactiveProperty<float>();

        [SerializeField]
        [Slider(0f, 1)]
        private float m_startingVolBGM = 0.75f;
        [SerializeField]
        [Slider(0f, 1f)]
        private float m_startingVolSFX = 0.75f;

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

        private void Awake()
        {
            m_reactiveVolumeBGM.Value = AudioData.GetVolumeBGM(m_startingVolBGM);
            m_reactiveVolumeSFX.Value = AudioData.GetVolumeSFX(m_startingVolSFX);
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

    }


}