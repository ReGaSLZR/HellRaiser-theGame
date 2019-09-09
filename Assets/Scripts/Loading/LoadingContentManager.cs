using UnityEngine;
using Scriptables;
using Zenject;
using Audio;

namespace Loading {
    
    public class LoadingContentManager : MonoBehaviour
    {

        [Inject]
        private readonly AudioModel.BGMSetter m_modelBGM;

        [SerializeField]
        private PlaySettings m_playSettings;

        private void Awake()
        {
            m_modelBGM.ReplaceOriginalBGM(m_playSettings.m_audioTheme.m_bgmLoading);
        }

        private void OnEnable()
        {
            m_modelBGM.PlayOriginalBGM();
        }

    }

}