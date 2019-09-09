using UnityEngine;

namespace Audio
{

    public static class AudioUtil
    {

        public static void SafelyPlayOneshot(AudioSource audioSource, AudioClip clip)
        {
            if ((audioSource!= null) && (clip != null))
            {
                audioSource.PlayOneShot(clip);
            }
        }

    }
}