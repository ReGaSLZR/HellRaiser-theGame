using UnityEngine;

namespace Data.Storage {

    public static class AudioData
    {

        private const string KEY_VOLUME_BGM = "KEY_VOLUME_BGM";
        private const string KEY_VOLUME_SFX = "KEY_VOLUME_SFX";

        public static void SaveVolume(float bgm, float sfx) {
            PlayerPrefs.SetFloat(KEY_VOLUME_BGM, bgm);
            PlayerPrefs.SetFloat(KEY_VOLUME_SFX, sfx);
            PlayerPrefs.Save();
        }

        public static float GetVolumeBGM(float defaultValue) {
            return PlayerPrefs.GetFloat(KEY_VOLUME_BGM, defaultValue);
        }

        public static float GetVolumeSFX(float defaultValue) {
            return PlayerPrefs.GetFloat(KEY_VOLUME_SFX, defaultValue);
        }

    }


}