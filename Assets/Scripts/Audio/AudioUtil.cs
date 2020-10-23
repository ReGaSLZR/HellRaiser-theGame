using System.Collections;
using UnityEngine;

namespace Audio
{

    public static class AudioUtil
    {

        private const float AUDIO_TRANSITION_TICK = 0.03f;

        public static void SafelyPlayRandom(AudioSource audioSource, AudioClip[] clips)
        {
            if (clips.Length == 0)
            {
                return;
            }

            SafelyPlay(audioSource, clips[Random.Range(0, clips.Length)]);
        }


        public static void SafelyPlayOneshotRandom(AudioSource audioSource, AudioClip[] clips)
        {
            if (clips.Length == 0)
            {
                return;
            }

            SafelyPlayOneshot(audioSource, clips[Random.Range(0, clips.Length)]);
        }

        public static void SafelyPlay(AudioSource audioSource, AudioClip clip)
        {
            if ((audioSource != null) && (clip != null))
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        public static void SafelyPlayOneshot(AudioSource audioSource, AudioClip clip)
        {
            if ((audioSource!= null) && (clip != null))
            {
                audioSource.PlayOneShot(clip);
            }
        }

        public static void SafelyTransitionToClip(MonoBehaviour caller, AudioSource audioSource, AudioClip newClip) {
            if ((caller != null) && (audioSource != null) && (newClip != null)) {
                caller.StopAllCoroutines();
                caller.StartCoroutine(CorTransitionAudio(audioSource, newClip));
            }
        }

        private static IEnumerator CorTransitionAudio(AudioSource audioSource, AudioClip newClip) {
            float tempVolume = audioSource.volume;

            //fade out
            while (audioSource.volume > 0)
            {
                audioSource.volume -= AUDIO_TRANSITION_TICK;
                yield return new WaitForSeconds(AUDIO_TRANSITION_TICK);
            }

            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();

            //fade in

            while (audioSource.volume < tempVolume)
            {
                audioSource.volume += AUDIO_TRANSITION_TICK;
                yield return new WaitForSeconds(AUDIO_TRANSITION_TICK);
            }
        }

    }
}