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
            ReactiveProperty<float> GetReactiveVolumeBGM();
            ReactiveProperty<float> GetReactiveVolumeSFX();
        }

        #endregion

        private ReactiveProperty<float> m_reactiveVolumeBGM = new ReactiveProperty<float>();
        private ReactiveProperty<float> m_reactiveVolumeSFX = new ReactiveProperty<float>();

        [SerializeField]
        [Slider(0f, 100f)]
        private float m_startingVolBGM = 0.75f;
        [SerializeField]
        [Slider(0f, 100f)]
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
            m_reactiveVolumeBGM.Value = m_startingVolBGM;
            m_reactiveVolumeSFX.Value = m_startingVolSFX;
        }

        private float ClampVolume(float volume)
        {
            return Mathf.Clamp(volume, 0f, 100f);
        }

        public void SetVolumeBGM(float volume)
        {
            m_reactiveVolumeBGM.Value = ClampVolume(volume);
        }

        public void SetVolumeSFX(float volume)
        {
            m_reactiveVolumeSFX.Value = ClampVolume(volume);
        }

        public ReactiveProperty<float> GetReactiveVolumeBGM()
        {
            return m_reactiveVolumeBGM;
        }

        public ReactiveProperty<float> GetReactiveVolumeSFX()
        {
            return m_reactiveVolumeSFX;
        }

    }


}