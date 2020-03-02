using UnityEngine;
using UniRx;
using Zenject;

namespace Audio {

    [RequireComponent(typeof(AudioSource))]
    public class VolumeAdjuster : MonoBehaviour
    {

        public enum AudioType
        {
            SFX,
            BGM
        }

        [Inject]
        private AudioModel.VolumeGetter m_modelAudio;

        private AudioSource m_compAudioSource;

        [SerializeField]
        private AudioType m_audioType;

        private void Awake()
        {
            m_compAudioSource = GetComponent<AudioSource>();
            //m_compAudioSource.loop = m_audioType;
        }

        private void Start()
        {
            SetVolumeListener();

        }

        private void SetVolumeListener()
        {
            switch (m_audioType)
            {
                case AudioType.BGM:
                {
                    m_modelAudio.GetVolumeBGM()
                   .Subscribe(volumeBGM => {
                       m_compAudioSource.volume = volumeBGM;
                   })
                   .AddTo(this);

                    break;
                }

                case AudioType.SFX:
                {
                    m_modelAudio.GetVolumeSFX()
                    .Subscribe(volumeSFX => {
                        m_compAudioSource.volume = volumeSFX;
                    })
                    .AddTo(this);

                    break;
                }
                default:
                    {
                        break;
                    }
            }
        }

    }


}