using NaughtyAttributes;
using UnityEngine;
using UniRx;
using Zenject;

namespace Audio {

    [RequireComponent(typeof(AudioSource))]
    public class VolumeAdjuster : MonoBehaviour
    {

        [Inject]
        private AudioModel.VolumeGetter m_modelAudio;

        private AudioSource m_compAudioSource;
        private readonly DropdownList<bool> m_dropdownListAudioType =
            new DropdownList<bool>(){
                {"SFX", false},
                {"BGM", true}
        };

        [Dropdown("m_dropdownListAudioType")]
        public bool m_audioType;

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
            if (m_audioType) //if BGM
            {
                m_modelAudio.GetVolumeBGM()
               .Subscribe(volumeBGM => {
                   m_compAudioSource.volume = volumeBGM;
               })
               .AddTo(this);
            }

            else //if SFX
            {
                m_modelAudio.GetVolumeSFX()
                .Subscribe(volumeSFX => {
                    m_compAudioSource.volume = volumeSFX;
                })
                .AddTo(this);
            }
        }

    }


}