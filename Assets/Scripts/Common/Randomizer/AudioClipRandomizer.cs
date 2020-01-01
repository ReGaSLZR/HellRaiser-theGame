namespace Common.Randomizer
{

    using Utils;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class AudioClipRandomizer : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] m_audioClips;

        [SerializeField]
        [Tooltip("This is for GameObjects that were inactive at game start" +
            " and got activated by runtime event. Toggle this ON if an automatic" +
            " playback is desired.")]
        private bool m_shouldForcePlay;

        private void Awake()
        {
            if (m_audioClips.Length == 0)
            {
                LogUtil.PrintWarning(this, GetType(), "Awake(): " +
                    "No AudioClips set. Destroying...");
                Destroy(this);
            }

            AudioSource source = GetComponent<AudioSource>();
            source.clip = m_audioClips[Random.Range(0, m_audioClips.Length)];

            if (m_shouldForcePlay)
            {
                source.Play();
            }

            Destroy(this);
        }
    }


}