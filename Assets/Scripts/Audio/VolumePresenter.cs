using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace Audio {

    public class VolumePresenter : MonoBehaviour
    {

        [Inject]
        private AudioModel.Getter m_modelAudioModelGetter;
        [Inject]
        private AudioModel.Setter m_modelAudioModelSetter;

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
            m_sliderVolumeBGM.value = m_modelAudioModelGetter.GetReactiveVolumeBGM().Value;
            m_sliderVolumeSFX.value = m_modelAudioModelGetter.GetReactiveVolumeSFX().Value;
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