using NaughtyAttributes;
using GamePlay.Camera;
using UnityEngine;

namespace Scriptables
{

    [CreateAssetMenu(fileName = "Skill-", menuName = "HellRaiser/Create Character Skill")]
    public class CharacterSkill : ScriptableObject
    {

        public string m_name;

        [TextArea]
        public string m_description;

        //[MinMaxSlider(0f, 150f)]
        //public Vector2 m_damageRange;

        [Range(0.1f, 10f)]
        public float m_duration;

        [Range(0, CharacterInfo.STAMINA_MAX)]
        public int m_cost;

        public CameraFXType m_cameraFXType = CameraFXType.None;

        [ShowAssetPreview]
        public Texture2D m_icon;
        
    }

}