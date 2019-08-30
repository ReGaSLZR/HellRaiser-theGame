using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Audio {

    public class VolumePresenter : MonoBehaviour
    {

        [Inject]
        private AudioModel.VolumeGetter m_modelAudioModelGetter;
        [Inject]
        private AudioModel.VolumeSetter m_modelAudioModelSetter;

        [SerializeField]
        private Slider m_sliderVolumeBGM;
        [SerializeField]
        private Slider m_sliderVolumeSFX;


        private void Start()
        {
            SetStartingVolumes();
            SetVolumeInputListener();
        }

        private void SetStartingVolumes()
        {
            m_sliderVolumeBGM.value = m_modelAudioModelGetter.GetVolumeBGM().Value;
            m_sliderVolumeSFX.value = m_modelAudioModelGetter.GetVolumeSFX().Value;
        }

        private void SetVolumeInputListener()
        {
            m_sliderVolumeBGM.OnPointerUpAsObservable()
                .Subscribe(_ => {
                    m_modelAudioModelSetter.SetVolumeBGM(m_sliderVolumeBGM.value);
                })
                .AddTo(this);

            m_sliderVolumeSFX.OnPointerUpAsObservable()
             .Subscribe(_ => {
                 m_modelAudioModelSetter.SetVolumeSFX(m_sliderVolumeSFX.value);
             })
             .AddTo(this);
        }

    }


}